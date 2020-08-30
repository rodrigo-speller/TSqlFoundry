// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.IO;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal interface ITSqlFragment
    {
        void WriteTo(TextWriter writer);
    }
}
