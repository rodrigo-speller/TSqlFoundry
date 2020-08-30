// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.IO;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal abstract class TSqlBasicType : TSqlType
    {
        private readonly string text;

        protected TSqlBasicType(string text)
        {
            this.text = text;
        }

        public sealed override void WriteTo(TextWriter writer)
        {
            writer.Write(text);
        }
    }
}
