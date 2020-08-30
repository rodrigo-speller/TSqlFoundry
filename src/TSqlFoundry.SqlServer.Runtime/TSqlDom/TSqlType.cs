// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using System.Data;
using TSqlFoundry.SqlServer.Runtime.SqlReflection;

namespace TSqlFoundry.SqlServer.Runtime.TSqlDom
{
    internal abstract class TSqlType : TSqlFragment
    {
        public static TSqlBigIntType BigInt { get; }
            = new TSqlBigIntType();
        public static TSqlBitType Bit { get; }
            = new TSqlBitType();
        public static TSqlIntType Int { get; }
            = new TSqlIntType();
        public static TSqlNVarCharType NVarChar { get; }
            = new TSqlNVarCharType();
        public static TSqlVarBinaryType VarBinary { get; }
            = new TSqlVarBinaryType();
        public static TSqlVarCharType VarChar { get; }
            = new TSqlVarCharType();

        public static TSqlType CreateFromType<T>()
        {
            return CreateFromType(typeof(T));
        }

        public static TSqlType CreateFromType(Type type)
        {
            if (SqlTypeMapper.TryGetDbType(type, out var dbType))
                return CreateFromType(dbType);

            throw new ArgumentException($"{type.FullName} is not a supported CLR type");
        }

        public static TSqlType CreateFromType(SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.BigInt:
                    return BigInt;
                case SqlDbType.Int:
                    return Int;
                case SqlDbType.NVarChar:
                    return NVarChar;
                case SqlDbType.VarChar:
                    return VarChar;
                case SqlDbType.VarBinary:
                    return VarBinary;
                case SqlDbType.Bit:
                    return Bit;
            }

            throw new ArgumentException($"{type} is not a supported type");
        }
    }
}
