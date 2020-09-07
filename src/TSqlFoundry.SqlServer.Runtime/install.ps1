# Copyright (c) Rodrigo Speller. All rights reserved.
# Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using namespace System.Data.SqlClient
using namespace System.IO

param (
    [string] $ConnectionString,
    [string] $DataSource='(LocalDb)\MSSQLLocalDB',
    [string] $InitialCatalog='master'
)

$ErrorActionPreference = "Stop"

$scriptDirectory = [Path]::GetDirectoryName($script:MyInvocation.MyCommand.Path)
$runtimeDllPath = [Path]::Combine($scriptDirectory, 'bin\Release\net462\TSqlFoundry.SqlServer.Runtime.dll')

function main ()
{
    ## BUILD PROJECT
    ## =============

    dotnet build -c Release "$scriptDirectory"

    if (-not $?)
    {
        throw 'Cannot build the TSqlFoundry.SqlServer.Runtime project'
    }

    ## LOAD RUNTIME
    ## ============

    Add-Type -Path $runtimeDllPath

    ## INSTALL IN THE DATABASE
    ## =======================

    $ConnectionString = Get-ConnectionString
    $conn = [SqlConnection]::new($ConnectionString)
    $conn.Open()

    try
    {
        [TSqlFoundry.SqlServer.Runtime.Installer]::InstallTSqlFoundry($conn)
    }
    catch
    {
        $err = $_
        $ex = $err.Exception
        Write-Error $ex
    }
    finally
    {
        $conn.Dispose();
    }
}

function Get-ConnectionString ()
{
    # initial connection string
    if ([string]::IsNullOrEmpty($ConnectionString))
    {
        $ConnectionString = 'Integrated Security=SSPI'
    }

    # Data Source
    $builder = [SqlConnectionStringBuilder]::new($ConnectionString)

    if ([string]::IsNullOrEmpty($builder.DataSource))
    {
        $builder['Data Source'] = $DataSource
    }

    # Initial Catalog
    if ([string]::IsNullOrEmpty($builder.InitialCatalog))
    {
        $builder['Initial Catalog'] = $InitialCatalog
    }

    return $builder.ConnectionString
}

main
