$ErrorActionPreference = 'Stop'
Set-Location (Join-Path $PSScriptRoot '..')
dotnet publish src/Erp.Api -c Release -o artifacts/api
dotnet publish src/Erp.Web -c Release -o artifacts/web
Write-Host "Publish output: artifacts/api and artifacts/web" -ForegroundColor Green
