# Script para encontrar o caminho do Java 17

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Encontrar Java 17" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Metodo 1: Usar Get-Command
Write-Host "Metodo 1: Get-Command" -ForegroundColor Yellow
try {
    $javaCmd = Get-Command java -ErrorAction Stop
    $javaPath = $javaCmd.Source
    $javaHome = Split-Path (Split-Path $javaPath -Parent) -Parent
    
    Write-Host "Java encontrado!" -ForegroundColor Green
    Write-Host "Executavel: $javaPath" -ForegroundColor White
    Write-Host "JAVA_HOME: $javaHome" -ForegroundColor Cyan
    Write-Host ""
} catch {
    Write-Host "Nao encontrado via Get-Command" -ForegroundColor Red
    Write-Host ""
}

# Metodo 2: Procurar em locais comuns
Write-Host "Metodo 2: Procurar em locais comuns" -ForegroundColor Yellow

$possiblePaths = @(
    "C:\Program Files\Eclipse Adoptium\jdk-17*",
    "C:\Program Files\Java\jdk-17*",
    "C:\Program Files\OpenJDK\jdk-17*",
    "C:\Program Files (x86)\Eclipse Adoptium\jdk-17*",
    "C:\Program Files (x86)\Java\jdk-17*"
)

$found = $false
foreach ($pattern in $possiblePaths) {
    $paths = Get-ChildItem -Path $pattern -Directory -ErrorAction SilentlyContinue
    
    if ($paths) {
        foreach ($path in $paths) {
            $javaExe = Join-Path $path.FullName "bin\java.exe"
            if (Test-Path $javaExe) {
                Write-Host "Encontrado: $($path.FullName)" -ForegroundColor Green
                
                # Testar versao
                $version = & $javaExe -version 2>&1 | Select-String "version"
                Write-Host "Versao: $version" -ForegroundColor White
                Write-Host ""
                $found = $true
            }
        }
    }
}

if (-not $found) {
    Write-Host "Nenhum JDK 17 encontrado nos locais comuns" -ForegroundColor Red
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Informacoes Atuais" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "JAVA_HOME atual: $env:JAVA_HOME" -ForegroundColor Yellow
Write-Host ""

Write-Host "Versao do Java no PATH:" -ForegroundColor Yellow
java -version 2>&1

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Proximos Passos" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "1. Copie o caminho do JAVA_HOME acima (sem \bin\java.exe)" -ForegroundColor White
Write-Host "2. Execute o script de configuracao:" -ForegroundColor White
Write-Host "   .\scripts\set-java-home.ps1 'CAMINHO_COPIADO'" -ForegroundColor Cyan
Write-Host ""
Write-Host "Ou configure manualmente:" -ForegroundColor White
Write-Host "1. Win + R -> sysdm.cpl" -ForegroundColor White
Write-Host "2. Avancado -> Variaveis de Ambiente" -ForegroundColor White
Write-Host "3. Edite JAVA_HOME com o caminho encontrado" -ForegroundColor White
Write-Host ""
