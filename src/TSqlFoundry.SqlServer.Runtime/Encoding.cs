// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using Microsoft.SqlServer.Server;

namespace TSqlFoundry.SqlServer.Runtime
{
    public static class Encoding
    {
        [SqlFunction(IsDeterministic = true)]
        public static byte[] Encode(string text, string encoding)
        {
            if ("base64".Equals(encoding, StringComparison.InvariantCultureIgnoreCase))
                return Convert.FromBase64String(text);

            var _encoding = System.Text.Encoding.GetEncoding(encoding);

            return _encoding.GetBytes(text);
        }

        [SqlFunction(IsDeterministic = true)]
        public static string Decode(byte[] raw, string encoding)
        {
            if ("base64".Equals(encoding, StringComparison.InvariantCultureIgnoreCase))
                return Convert.ToBase64String(raw);

            var _encoding = System.Text.Encoding.GetEncoding(encoding);

            return _encoding.GetString(raw);
        }
    }
}
