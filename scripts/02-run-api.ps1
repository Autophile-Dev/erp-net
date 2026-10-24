$ErrorActionPreference = 'Stop'
Set-Location (Join-Path $PSScriptRoot '..')
dotnet run --project src/Erp.Api --launch-profile http
