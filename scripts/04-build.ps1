$ErrorActionPreference = 'Stop'
Set-Location (Join-Path $PSScriptRoot '..')
dotnet restore src/Erp.Api/Erp.Api.csproj
dotnet restore src/Erp.Web/Erp.Web.csproj
dotnet build src/Erp.Api/Erp.Api.csproj --no-restore
dotnet build src/Erp.Web/Erp.Web.csproj --no-restore
