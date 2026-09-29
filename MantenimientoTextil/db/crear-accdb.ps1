# Crea db/mantenimiento.accdb ejecutando los scripts de db/schema en orden. Requiere Access/ACE en Windows.
$ruta = Join-Path $PSScriptRoot 'mantenimiento.accdb'
if (Test-Path $ruta) { Remove-Item $ruta }
$cat = New-Object -ComObject ADOX.Catalog
$cat.Create("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=$ruta")
$conn = New-Object -ComObject ADODB.Connection
$conn.Open("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=$ruta")
Get-ChildItem (Join-Path $PSScriptRoot 'schema') -Filter *.sql | Sort-Object Name | ForEach-Object {
  # Access ejecuta una sentencia por llamada
  (Get-Content $_.FullName -Raw) -split ';' | Where-Object { ($_ -replace '--.*','').Trim() } | ForEach-Object { $conn.Execute($_) | Out-Null }
}
$conn.Close()
Write-Host "Creada $ruta"
