// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.IO;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal class TSqlObjectIdentifier : TSqlFragment
    {
        public TSqlObjectIdentifier(string objectName)
        {
            ObjectName = objectName;
        }

        public string ObjectName { get; }

        public static TSqlObjectIdentifier Create(string objectName)
            => new TSqlObjectIdentifier(objectName);

        public static TSqlSchemaObjectIdentifier Create(string schemaName, string objectName)
            => new TSqlSchemaObjectIdentifier(schemaName, objectName);

        public static TSqlDatabaseObjectIdentifier Create(
            string databaseName,
            string schemaName,
            string objectName)
            => new TSqlDatabaseObjectIdentifier(databaseName, schemaName, objectName);

        public override void WriteTo(TextWriter writer)
        {
            writer.Write(ObjectName.ToSqlIdentifier());
        }
    }
}
