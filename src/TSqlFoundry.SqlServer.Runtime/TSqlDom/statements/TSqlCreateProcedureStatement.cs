// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal class TSqlCreateProcedureStatement : TSqlStatement
    {
        public TSqlCreateProcedureStatement(
            TSqlObjectIdentifier name,
            IEnumerable<TSqlModuleParameter> parameters,
            TSqlClrMethodSpecifier definition)
        {
            ProcedureName = name;
            Parameters = parameters.ToArray();
            Definition = definition;
        }

        public TSqlObjectIdentifier ProcedureName { get; }
        public IReadOnlyList<TSqlModuleParameter> Parameters { get; }
        public ITSqlFragment Definition { get; }

        public override void WriteTo(IndentedTextWriter writer)
        {
            var initialIndent = writer.Indent;

            writer.Write("CREATE PROCEDURE ");
            ProcedureName.WriteTo(writer);

            // parameters
            if (Parameters.Any())
            {
                writer.Write(" (");
                writer.Indent++;
                Parameters.WriteCommaSeparatedMultilineTo(writer);
                writer.Indent--;
                writer.Write(')');
            }
            
            writer.WriteLine();

            writer.Indent++;
            try
            {
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
    }
}
