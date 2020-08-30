// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace TSqlFoundry.SqlServer.Runtime
{
    public static class Setup
    {
        [SqlProcedure]
        public static void Install()
        {
            using(var connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                Installer.InstallTSqlFoundry(connection);
            }
        }

        [SqlProcedure]
        public static void Uninstall()
        {
            using (var connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                Installer.UninstallTSqlFoundry(connection);
            }
        }
    }
}
