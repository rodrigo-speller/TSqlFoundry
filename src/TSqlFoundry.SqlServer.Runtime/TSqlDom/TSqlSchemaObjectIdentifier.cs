// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.IO;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal class TSqlSchemaObjectIdentifier : TSqlObjectIdentifier
    {
        public TSqlSchemaObjectIdentifier(string schemaName, string objectName)
            : base (objectName)
        {
            SchemaName = schemaName;
        }

        public string SchemaName { get; }

        public override void WriteTo(TextWriter writer)
        {
            writer.Write(SchemaName.ToSqlIdentifier());
            writer.Write('.');
            writer.Write(ObjectName.ToSqlIdentifier());
        }
    }
}
