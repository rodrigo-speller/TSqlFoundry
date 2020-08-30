// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.IO;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal class TSqlDatabaseObjectIdentifier : TSqlSchemaObjectIdentifier
    {
        public TSqlDatabaseObjectIdentifier(string databaseName, string schemaName, string objectName)
            : base (schemaName, objectName)
        {
            DatabaseName = databaseName;
        }

        public string DatabaseName { get; }

        public override void WriteTo(TextWriter writer)
        {
            writer.Write(DatabaseName.ToSqlIdentifier());
            writer.Write('.');
            writer.Write(SchemaName.ToSqlIdentifier());
            writer.Write('.');
            writer.Write(ObjectName.ToSqlIdentifier());
        }
    }
}
