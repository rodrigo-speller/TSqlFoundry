// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using System.IO;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal class TSqlNVarCharType : TSqlType
    {
        private readonly int size;

        public TSqlNVarCharType()
        {
            this.size = -1;
        }

        public TSqlNVarCharType(int size)
        {
            if (size < 0 || size > 4000)
                throw new ArgumentException("Invlid NVARCHAR size", nameof(size));

            this.size = size;
        }

        public override void WriteTo(TextWriter writer)
        {
            var size = this.size;

            if (size == -1)
                writer.Write("NVARCHAR(max)");
            else
                writer.Write($"NVARCHAR({size.ToInvariantString()})");
        }
    }
}
