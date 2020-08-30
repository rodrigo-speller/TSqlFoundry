// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.CodeDom.Compiler;
using System.IO;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal abstract class TSqlStatement : TSqlFragment
    {
        public sealed override void WriteTo(TextWriter writer)
        {
            if (!(writer is IndentedTextWriter indented))
                indented = new IndentedTextWriter(writer);

            WriteTo(indented);
        }

        public abstract void WriteTo(IndentedTextWriter writer);
    }
}
