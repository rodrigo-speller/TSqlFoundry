// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.IO;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal class TSqlClrMethodSpecifier : TSqlFragment
    {
        public TSqlClrMethodSpecifier(string assemblyName, string className, string methodName)
        {
            AssemblyName = assemblyName;
            ClassName = className;
            MethodName = methodName;
        }

        public string AssemblyName { get; }
        public string ClassName { get; }
        public string MethodName { get; }

        public override void WriteTo(TextWriter writer)
        {
            writer.Write("EXTERNAL NAME ");
            writer.Write(AssemblyName.ToSqlIdentifier());
            writer.Write('.');
            writer.Write(ClassName.ToSqlIdentifier());
            writer.Write('.');
            writer.Write(MethodName.ToSqlIdentifier());
        }
    }
}
