// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TSqlFoundry.SqlServer.Runtime.SqlReflection;
using TSqlFoundry.SqlServer.Runtime.TSqlDom;

namespace TSqlFoundry.SqlServer.Runtime
{
    internal static class SqlClientExtensions
    {
        public static int ExecuteNonQuery(
            this SqlConnection connection,
            string commandText,
            params SqlParameter[] parameters)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.Parameters.AddRange(parameters);

                return command.ExecuteNonQuery();
            }
        }
        
        public static SqlDataReader ExecuteReader(
            this SqlConnection connection,
            string commandText,
            params SqlParameter[] parameters)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.Parameters.AddRange(parameters);

                return command.ExecuteReader();
            }
        }
        public static T ExecuteScalar<T>(
            this SqlConnection connection,
            string commandText,
            params SqlParameter[] parameters)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.Parameters.AddRange(parameters);

                return (T)command.ExecuteScalar();
            }
        }

        public static IEnumerable<TResult> ExecuteReader<TResult>(
            this SqlConnection connection,
            string commandText,
            Func<SqlDataReader, TResult> selector)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;

                using(var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                        yield return selector(reader);
                }
            }
        }

        public static IEnumerable<TResult> Select<TResult>(
            this SqlDataReader reader,
            Func<SqlDataReader, TResult> selector)
        {
            while(reader.Read())
                yield return selector(reader);
        }
        
        public static void CreateSchemaIfNotExists(this SqlConnection connection, string name)
        {
            var identifier = name.ToSqlIdentifier();
            name = name.ToSqlUnicode();

            connection.ExecuteNonQuery($@"
                IF SCHEMA_ID({name}) IS NULL BEGIN
                    DECLARE @cmd NVARCHAR(max) = CONCAT('CREATE SCHEMA ', {identifier.ToSqlUnicode()})
                    EXEC (@cmd)
                END
            ");
        }

        public static SqlAssemblyInfo[] GetInstalledAssemblies(
            this SqlConnection connection,
            AssemblyName assemblyName = null)
        {
            var installedAssemblies = connection.ExecuteReader(
                "SELECT assembly_id, name, clr_name FROM sys.assemblies",
                x => new SqlAssemblyInfo(x.GetInt32(0), x.GetString(1), x.GetString(2))
            );

            if (!(assemblyName is null))
                installedAssemblies = installedAssemblies
                    .Where(x => AssemblyName.ReferenceMatchesDefinition(x.AssemblyName, assemblyName));

            return installedAssemblies.ToArray();
        }

        public static SqlObjectInfo[] GetAssemblyModules(
            this SqlConnection connection,
            int? assemblyId = null)
        {
            var reader = connection.ExecuteReader(
                $@"
                    SELECT
                        o.schema_id,
                        s.name,
                        o.object_id,
                        o.name,
                        o.type

                        FROM sys.assembly_modules m

                        INNER JOIN sys.objects o
                            ON o.object_id = m.object_id

                        INNER JOIN sys.schemas s
                            ON s.schema_id = o.schema_id

                        WHERE m.assembly_id = COALESCE(@p_assembly_id, m.assembly_id)
                ",
                new SqlParameter("@p_assembly_id", assemblyId)
            );

            using (reader)
            {
                return reader
                    .Select(x => new SqlObjectInfo(
                        schemaId: x.GetInt32(0),
                        schemaName: x.GetString(1),
                        objectId: x.GetInt32(2),
                        name: x.GetString(3),
                        type: x.GetString(4)
                    ))
                    .ToArray();
            }
        }

        public static void DropObjects(this SqlConnection connection, IEnumerable<SqlObjectInfo> objects)
        {
            foreach (var obj in objects)
            {
                var schemaName = obj.SchemaName.ToSqlIdentifier();
                var objectName = obj.ObjectName.ToSqlIdentifier();
                
                string sqlCommand;
                switch (obj.Type)
                {
                    case SqlObjectType.AssemblyStoredProcedure:
                        sqlCommand = $"DROP PROCEDURE {schemaName}.{objectName}";
                        break;
                    case SqlObjectType.AssemblyScalarFunction:
                    case SqlObjectType.AssemblyTableValuedFunction:
                        sqlCommand = $"DROP FUNCTION {schemaName}.{objectName}";
                        break;
                    default:
                        continue;
                }

                connection.ExecuteNonQuery(sqlCommand);
            }
        }

        public static bool ObjectExists(this SqlConnection connection, TSqlObjectIdentifier identifier)
        {
            return connection.ExecuteScalar<bool>(
                @"SELECT CAST(IIF(OBJECT_ID(@p_name) IS NULL, 0, 1) AS BIT)",
                new SqlParameter("@p_name", identifier.ToString())
            );
        }

        public static void DropAssembly(this SqlConnection connection, SqlAssemblyInfo assembly)
        {
            connection.ExecuteNonQuery($@"DROP ASSEMBLY {assembly.Name.ToSqlIdentifier()}");
        }
    }
}
