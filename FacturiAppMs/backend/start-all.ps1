#.\start-all.ps1 in folderu de la proiect ca sa pornesc tot (pornire)
$root = $PSScriptRoot
$servicii = "ClientService","ProdusService","FacturaService","PlatiService", "AuthService"
foreach ($s in $servicii) {
    Start-Process powershell -ArgumentList "-NoExit","-Command","cd '$root\$s'; dotnet run --launch-profile http"
}
Write-Host "Pornite: ClientService(5101) ProdusService(5102) FacturaService(5103) PlatiService(5104) AuthService(5105)   "
