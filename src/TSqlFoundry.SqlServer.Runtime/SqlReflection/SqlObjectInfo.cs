// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;

namespace TSqlFoundry.SqlServer.Runtime.SqlReflection
{
    internal class SqlObjectInfo
    {
        public SqlObjectInfo(int schemaId, string schemaName, int objectId, string name, string type)
            : this(schemaId, schemaName, objectId, name, ParseType(type))
        { }

        public SqlObjectInfo(int schemaId, string schemaName, int objectId, string name, SqlObjectType type)
        {
            SchemaId = schemaId;
            SchemaName = schemaName;
            ObjectId = objectId;
            ObjectName = name;
            Type = type;
        }

        public int SchemaId { get; }
        public string SchemaName { get; }
        public int ObjectId { get; }
        public string ObjectName { get; }
        public SqlObjectType Type { get; }

        public static SqlObjectType ParseType(string type)
        {
            switch (type)
            {
                case "AF":
                    return SqlObjectType.AssemblyAggregateFunction;
                case "C":
                    return SqlObjectType.CheckConstraint;
                case "D":
                    return SqlObjectType.DefaultConstraint;
                case "F":
                    return SqlObjectType.ForeignKeyConstraint;
                case "FN":
                    return SqlObjectType.ScalarFunction;
                case "FS":
                    return SqlObjectType.AssemblyScalarFunction;
                case "FT":
                    return SqlObjectType.AssemblyTableValuedFunction;
                case "IF":
                    return SqlObjectType.InlineTableValuedFunction;
                case "IT":
                    return SqlObjectType.InternalTable;
                case "P":
                    return SqlObjectType.StoredProcedure;
                case "PC":
                    return SqlObjectType.AssemblyStoredProcedure;
                case "PG":
                    return SqlObjectType.PlanGuide;
                case "PK":
                    return SqlObjectType.PrimaryKeyConstraint;
                case "R":
                    return SqlObjectType.Rule;
                case "RF":
                    return SqlObjectType.ReplicationFilterProcedure;
                case "S":
                    return SqlObjectType.SystemBaseTable;
                case "SN":
                    return SqlObjectType.Synonym;
                case "SO":
                    return SqlObjectType.SequenceObject;
                case "T":
                    return SqlObjectType.Table;
                case "V":
                    return SqlObjectType.View;
                case "EC":
                    return SqlObjectType.EdgeContraint;
            }

            throw new ArgumentException("Invalid object type", nameof(type));
        }
    }
}
