// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using System.IO;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal class TSqlClrTableTypeColumn<T> : TSqlClrTableTypeColumn
    {
        public TSqlClrTableTypeColumn(string name)
            : base (name, typeof(T))
        { }
    }

    internal class TSqlClrTableTypeColumn : TSqlFragment
    {
        public TSqlClrTableTypeColumn(string name, Type type)
            : this(name, TSqlType.CreateFromType(type))
        { }

        public TSqlClrTableTypeColumn(string name, TSqlType type)
        {
            ColumnName = name;
            ColumnType = type;
        }

        public string ColumnName { get; }
        public TSqlType ColumnType { get; }

        public override void WriteTo(TextWriter writer)
        {
            writer.Write(ColumnName.ToSqlIdentifier());
            writer.Write(' ');
            ColumnType.WriteTo(writer);
        }
    }
}
