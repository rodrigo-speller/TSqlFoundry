// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal class TSqlClrTableType : TSqlType
    {
        public TSqlClrTableType(IEnumerable<TSqlClrTableTypeColumn> columns)
        {
            Columns = columns.ToArray();
        }

        public IReadOnlyList<TSqlClrTableTypeColumn> Columns { get; }

        public sealed override void WriteTo(TextWriter writer)
        {
            if (!(writer is IndentedTextWriter indented))
                indented = new IndentedTextWriter(writer);

            WriteTo(indented);
        }

        public virtual void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("TABLE (");

            writer.Indent++;
            Columns.WriteCommaSeparatedMultilineTo(writer);
            writer.Indent--;

            writer.Write(")");
        }
    }
}
