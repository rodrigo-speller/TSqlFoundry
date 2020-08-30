// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.SqlServer.Server;
using TSqlFoundry.SqlServer.Runtime.SqlReflection;
using TSqlFoundry.SqlServer.Runtime.TSqlDom;

namespace TSqlFoundry.SqlServer.Runtime
{
    public static class Installer
    {
        public static void InstallTSqlFoundry(SqlConnection connection)
        {
            var assemblyInfo = DetectInstalledTSqlFoundryAssembly(connection);

            Assembly assembly;
            if (assemblyInfo is null)
            {
                assembly = typeof(Installer).Assembly;
                assemblyInfo = InstallAssembly(connection, assembly);
            }
            else
            {
                assembly = GetAssembly(connection, assemblyInfo.AssemblyId);
            }

            InstallMethods(connection, assembly, assemblyInfo);
        }

        public static void UninstallTSqlFoundry(SqlConnection connection)
        {
            var installedAssembly = DetectInstalledTSqlFoundryAssembly(connection)
                ?? throw new Exception("No TSqlFoundry runtime installed");

            var modules = connection.GetAssemblyModules(installedAssembly.AssemblyId);

            connection.DropObjects(modules);
            connection.DropAssembly(installedAssembly);
        }

        private static SqlAssemblyInfo DetectInstalledTSqlFoundryAssembly(SqlConnection connection)
        {
            var assemblyName = typeof(Installer).Assembly.GetName();
            var installedAssemblies = connection.GetInstalledAssemblies(assemblyName);

            SqlAssemblyInfo detectedAssembly;
            switch (installedAssemblies.Length)
            {
                case 0:
                    return null;
                case 1:
                    detectedAssembly = installedAssemblies[0];
                    break;
                default:
                    throw new Exception("Too many TSqlFoundry runtimes detected");
            }

            if (SqlContext.IsAvailable)
            {
                SqlContext.Pipe.Send(
                    $"TSqlFoundry runtime \"{assemblyName}\" matches \"{detectedAssembly.AssemblyName}\""
                );
            }

            return detectedAssembly;
        }

        private static SqlAssemblyInfo InstallAssembly(SqlConnection connection, Assembly assembly)
        {
            var originalName = assembly.GetName().Name;
            var name = originalName;

            var data = File.ReadAllBytes(assembly.Location);
            var sqlData = string.Join("", data.Select(x => x.ToString("X2")));

            int i = 0;
            while (true)
            {
                var exists = connection.ExecuteScalar<bool>($@"
                    SELECT CAST(IIF(EXISTS(
                        SELECT 1
                            FROM sys.assemblies
                            WHERE name = {name.ToSqlUnicode()}
                    ), 1, 0) AS BIT)
                ");

                if (!exists)
                    break;

                name = $"{originalName}_{(++i).ToInvariantString()}";
            }

            connection.ExecuteNonQuery($@"
                CREATE ASSEMBLY {name.ToSqlIdentifier()}
                    FROM 0x{sqlData}
                    WITH PERMISSION_SET = UNSAFE
            ");

            return DetectInstalledTSqlFoundryAssembly(connection)
                ?? throw new InvalidOperationException("");
        }

        private static Assembly GetAssembly(SqlConnection connection, int assemblyId)
        {
            if (SqlContext.IsAvailable)
                return typeof(Installer).Assembly;

            var data = connection.ExecuteScalar<byte[]>(
                @"
                    SELECT content
                        FROM sys.assembly_files
                        WHERE assembly_id = @p_assembly_id
                ",
                new SqlParameter("@p_assembly_id", assemblyId)
            );

            return Assembly.Load(data);
        }

        private static void InstallMethods(SqlConnection connection, Assembly assembly, SqlAssemblyInfo assemblyInfo)
        {
            var methods = GetInstallableMethods(assembly);

            foreach (var method in methods)
                InstallMethod(connection, assemblyInfo, method);
        }

        private const BindingFlags MethodsFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod;
        private static MethodInfo[] GetInstallableMethods(Assembly assembly)
        {
            return assembly
                .GetExportedTypes()
                .Where(type => !type.IsGenericType && type.IsClass)
                .SelectMany(type => type.GetMethods(MethodsFlags))
                .Where(method =>
                    method.GetCustomAttributes()
                        .Any(attr => attr is SqlFunctionAttribute || attr is SqlProcedureAttribute)
                )
                .ToArray();
        }

        private static bool CreateObject(
            SqlConnection connection,
            TSqlObjectIdentifier identifier,
            TSqlStatement statement)
        {
            if (connection.ObjectExists(identifier))
            {
                // TODO: logging
                return false;
            }

            if (identifier is TSqlSchemaObjectIdentifier schemaIdentifier)
                connection.CreateSchemaIfNotExists(schemaIdentifier.SchemaName);
            
            connection.ExecuteNonQuery(statement.ToString());
            return true;
        }

        private static void InstallMethod(SqlConnection connection, SqlAssemblyInfo assemblyInfo, MethodInfo method)
        {
            bool installed = false;

            var functionInfo = method.GetCustomAttribute<SqlFunctionAttribute>();
            if (functionInfo != null)
            {
                var function = CreateFunction(assemblyInfo, method, functionInfo);

                if (CreateObject(connection, function.FunctionName, function))
                    installed = true;
            }

            var procedureInfo = method.GetCustomAttribute<SqlProcedureAttribute>();
            if (procedureInfo != null)
            {
                var procedure = CreateProcedure(assemblyInfo, method, procedureInfo);

                if (CreateObject(connection, procedure.ProcedureName, procedure))
                    installed = true;
            }

            if (!installed)
            {
                // TODO: logging
            }
        }

        private static TSqlCreateProcedureStatement CreateProcedure(
            SqlAssemblyInfo assemblyInfo,
            MethodInfo method,
            SqlProcedureAttribute info)
        {
            return new TSqlCreateProcedureStatement(
                name: TSqlObjectIdentifier.Create("foundry", method.Name),
                
                parameters: method.GetParameters()
                    .Select(parameter => new TSqlModuleParameter($"@{parameter.Name}", parameter.ParameterType)),
                
                definition: new TSqlClrMethodSpecifier(
                    assemblyInfo.Name,
                    method.DeclaringType.FullName,
                    method.Name
                )
            );
        }

        private static TSqlCreateFunctionStatement CreateFunction(
            SqlAssemblyInfo assemblyInfo,
            MethodInfo method,
            SqlFunctionAttribute info)
        {
            if (method.ReturnType == typeof(IEnumerable) && !(info.FillRowMethodName is null))
                return CreateTableValuedFunction(assemblyInfo, method, info);

            return new TSqlCreateFunctionStatement(
                name:
                    TSqlObjectIdentifier.Create("foundry", method.Name),
                parameters:
                    method.GetParameters()
                        .Select(parameter => new TSqlModuleParameter($"@{parameter.Name}", parameter.ParameterType)),
                returns:
                    TSqlType.CreateFromType(method.ReturnType),
                definition:
                    new TSqlClrMethodSpecifier(
                        assemblyInfo.Name,
                        method.DeclaringType.FullName,
                        method.Name
                    )
            );
        }

        private static TSqlCreateFunctionStatement CreateTableValuedFunction(
            SqlAssemblyInfo assemblyInfo,
            MethodInfo method,
            SqlFunctionAttribute info)
        {
            var fillMethod = method.DeclaringType.GetMethod(info.FillRowMethodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            return new TSqlCreateFunctionStatement(
                name:
                    TSqlObjectIdentifier.Create("foundry", method.Name),
                parameters:
                    method.GetParameters()
                        .Select(parameter => new TSqlModuleParameter($"@{parameter.Name}", parameter.ParameterType)),
                returns:
                    new TSqlClrTableType(
                        fillMethod.GetParameters()
                            .Skip(1)
                            .Select(parameter => new TSqlClrTableTypeColumn(parameter.Name, parameter.ParameterType.GetElementType()))
                    ),
                definition:
                    new TSqlClrMethodSpecifier(
                        assemblyInfo.Name,
                        method.DeclaringType.FullName,
                        method.Name
                    )
            );
        }
    }
}
