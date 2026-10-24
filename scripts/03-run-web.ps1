$ErrorActionPreference = 'Stop'
Set-Location (Join-Path $PSScriptRoot '..')
dotnet run --project src/Erp.Web --launch-profile http
