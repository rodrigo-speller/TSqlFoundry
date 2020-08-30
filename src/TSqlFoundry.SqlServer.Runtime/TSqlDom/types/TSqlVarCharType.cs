// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using System.IO;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal class TSqlVarCharType : TSqlType
    {
        private readonly int size;

        public TSqlVarCharType()
        {
            this.size = -1;
        }

        public TSqlVarCharType(int size)
        {
            if (size < 0 || size > 8000)
                throw new ArgumentException("Invlid VARCHAR size", nameof(size));

            this.size = size;
        }

        public override void WriteTo(TextWriter writer)
        {
            var size = this.size;

            if (size == -1)
                writer.Write("VARCHAR(max)");
            else
                writer.Write($"VARCHAR({size.ToInvariantString()})");
        }
    }
}
