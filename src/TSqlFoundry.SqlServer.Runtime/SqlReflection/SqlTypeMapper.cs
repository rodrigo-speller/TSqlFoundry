// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;

namespace TSqlFoundry.SqlServer.Runtime.SqlReflection
{
    internal class SqlTypeMapper
    {
        private static IReadOnlyDictionary<Type, SqlDbType> clrTypesMap = new Dictionary<Type, SqlDbType> {
            { typeof(bool),             SqlDbType.Bit },
            { typeof(byte),             SqlDbType.TinyInt },
            { typeof(byte[]),           SqlDbType.VarBinary },
            { typeof(char[]),           SqlDbType.NVarChar },
            { typeof(DateTime),         SqlDbType.DateTime2 },
            { typeof(DateTimeOffset),   SqlDbType.DateTimeOffset },
            { typeof(decimal),          SqlDbType.Money },
            { typeof(double),           SqlDbType.Float },
            { typeof(float),            SqlDbType.Real },
            { typeof(int),              SqlDbType.Int },
            { typeof(long),             SqlDbType.BigInt },
            { typeof(short),            SqlDbType.SmallInt },
            { typeof(string),           SqlDbType.NVarChar },
            { typeof(TimeSpan),         SqlDbType.Time },
        };

        public static bool TryGetDbType(Type type, out SqlDbType dbType)
            => clrTypesMap.TryGetValue(type, out dbType);

        public static bool TryGetDbType<T>(out SqlDbType dbType)
            => TryGetDbType(typeof(T), out dbType);
    }
}
