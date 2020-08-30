// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal class TSqlCreateFunctionStatement : TSqlStatement
    {
        public TSqlCreateFunctionStatement(
            TSqlObjectIdentifier name,
            IEnumerable<TSqlModuleParameter> parameters,
            TSqlType returns,
            TSqlClrMethodSpecifier definition)
        {
            FunctionName = name;
            Parameters = parameters.ToArray();
            Returns = returns;
            Definition = definition;
        }

        public TSqlObjectIdentifier FunctionName { get; }
        public IReadOnlyList<TSqlModuleParameter> Parameters { get; }
        public ITSqlFragment Returns { get; }
        public ITSqlFragment Definition { get; }

        public override void WriteTo(IndentedTextWriter writer)
        {
            var initialIndent = writer.Indent;

            writer.Write("CREATE FUNCTION ");
            FunctionName.WriteTo(writer);

            // parameters
            writer.Write(" (");
            WriteParameters(writer);
            writer.WriteLine(')');

            writer.Indent++;
            try
            {
                // returns
                writer.Write("RETURNS ");
                Returns.WriteTo(writer);
                writer.WriteLine();

                // definition
                writer.Write("AS ");
                Definition.WriteTo(writer);
            }
            finally
            {
                writer.Indent--;
            }
            writer.WriteLine();
        }

        private void WriteParameters(IndentedTextWriter writer)
        {
            writer.Indent++;
            Parameters.WriteCommaSeparatedMultilineTo(writer);
            writer.Indent--;
        }
    }
}
