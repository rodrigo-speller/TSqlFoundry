// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.Reflection;

namespace TSqlFoundry.SqlServer.Runtime.SqlReflection
{
    internal class SqlAssemblyInfo
    {
        public SqlAssemblyInfo(int assemblyId, string name, string assemblyName)
            : this(assemblyId, name, new AssemblyName(assemblyName))
        { }

        public SqlAssemblyInfo(int assemblyId, string name, AssemblyName assemblyName)
        {
            AssemblyId = assemblyId;
            Name = name;
            AssemblyName = assemblyName;
        }

        public readonly int AssemblyId;
        public readonly string Name;
        public readonly AssemblyName AssemblyName;
    }
}
