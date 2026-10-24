$ErrorActionPreference = 'Stop'
Write-Host "Checking .NET SDK..." -ForegroundColor Cyan
dotnet --info
Write-Host "Checking PostgreSQL TCP port..." -ForegroundColor Cyan
Test-NetConnection localhost -Port 5432 | Select-Object ComputerName,RemotePort,TcpTestSucceeded
