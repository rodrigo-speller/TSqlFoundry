// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal static class TSqlFragmentExtensions
    {
        public static void WriteCommaSeparatedMultilineTo<T>(this IEnumerable<T> items, TextWriter writer)
            where T : ITSqlFragment
        {
            var writed = false;

            foreach (var parameter in items)
            {
                if (writed)
                    writer.Write(",");

                writer.WriteLine();

                parameter.WriteTo(writer);

                writed = true;
            }

            if (writed)
                writer.WriteLine();
        }
    }
}
