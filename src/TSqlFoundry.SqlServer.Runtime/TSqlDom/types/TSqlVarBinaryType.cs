// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using System.IO;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal class TSqlVarBinaryType : TSqlType
    {
        private readonly int size;

        public TSqlVarBinaryType()
        {
            this.size = -1;
        }

        public TSqlVarBinaryType(int size)
        {
            if (size < 0 || size > 8000)
                throw new ArgumentException("Invlid VARBINARY size", nameof(size));

            this.size = size;
        }

        public override void WriteTo(TextWriter writer)
        {
            var size = this.size;

            if (size == -1)
                writer.Write("VARBINARY(max)");
            else
                writer.Write($"VARBINARY({size.ToInvariantString()})");
        }
    }
}
