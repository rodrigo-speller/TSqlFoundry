// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.Globalization;

namespace TSqlFoundry.SqlServer.Runtime
{
    internal static class HelperExtensions
    {
        public static string ToInvariantString(this int value)
            => value.ToString(CultureInfo.InvariantCulture);

        public static string ToSqlIdentifier(this string value)
            => $"[{value.Replace("]", "]]")}]";

        public static string ToSqlUnicode(this string value)
            => $"N'{value.Replace("'", "''")}'";
    }
}
