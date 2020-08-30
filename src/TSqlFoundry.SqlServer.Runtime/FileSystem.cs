// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using System.Collections;
using System.IO;
using Microsoft.SqlServer.Server;

namespace TSqlFoundry.SqlServer.Runtime
{
    public static class FileSystem
    {
        [SqlProcedure]
        public static void AppendToFile(string path, byte[] data)
        {
            using(var file = File.Open(path, FileMode.CreateNew))
                file.Write(data, 0, data.Length);
        }

        [SqlFunction(IsDeterministic = true)]
        public static string CombinePath(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        [SqlProcedure]
        public static void CopyFile(string source, string destination)
        {
            File.Copy(source, destination, false);
        }

        [SqlProcedure]
        public static void CreateFile(string path)
        {
            File.Create(path).Dispose();
        }

        [SqlProcedure]
        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        [SqlFunction]
        public static string GetTempPath()
        {
            return Path.GetTempPath();
        }

        [SqlFunction(IsDeterministic = true)]
        public static bool IsPathRooted(string path)
        {
            return Path.IsPathRooted(path);
        }

        [SqlProcedure]
        public static void MoveFile(string source, string destination)
        {
            File.Move(source, destination);
        }

        [SqlFunction(
            FillRowMethodName = nameof(ReadFile_FillRow)
        )]
        public static IEnumerable ReadFile(string path)
        {
            using(var file = File.OpenRead(path))
            {
                var buffer = new byte[255];

                int count;
                while((count = file.Read(buffer, 0, 255)) > 0)
                {
                    if (count == 255)
                        yield return buffer;
                    else
                    {
                        var dataBuffer = new byte[count];
                        Array.Copy(buffer, 0, dataBuffer, 0, count);
                        yield return dataBuffer;
                    }
                }
            }
        }

        private static void ReadFile_FillRow(object row, out byte[] data)
        {
            data = (byte[])row;
        }

        [SqlProcedure]
        public static void WriteToFile(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }
    }
}
