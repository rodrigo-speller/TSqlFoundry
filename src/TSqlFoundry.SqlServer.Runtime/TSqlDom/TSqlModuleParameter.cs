// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using System.IO;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal class TSqlModuleParameter<T> : TSqlModuleParameter
    {
        public TSqlModuleParameter(string name)
            : base (name, typeof(T))
        { }
    }

    internal class TSqlModuleParameter : TSqlFragment
    {
        public TSqlModuleParameter(string name, Type type)
            : this(name, TSqlType.CreateFromType(type))
        { }

        public TSqlModuleParameter(string name, TSqlType type)
        {
            ParameterName = name;
            ParameterType = type;
        }

        public string ParameterName { get; }
        public TSqlType ParameterType { get; }

        public override void WriteTo(TextWriter writer)
        {
            writer.Write(ParameterName);
            writer.Write(' ');
            ParameterType.WriteTo(writer);
        }
    }
}
