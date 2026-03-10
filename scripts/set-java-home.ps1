# Script para configurar JAVA_HOME temporariamente
# Uso: .\scripts\set-java-home.ps1 "C:\Program Files\Eclipse Adoptium\jdk-17.0.17.10-hotspot"

param(
    [Parameter(Mandatory=$false)]
    [string]$JavaHomePath
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Configurar JAVA_HOME" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Se nao forneceu o caminho, tentar encontrar automaticamente
if (-not $JavaHomePath) {
    Write-Host "Procurando Java 17 automaticamente..." -ForegroundColor Yellow
    
    # Tentar Get-Command
    try {
        $javaCmd = Get-Command java -ErrorAction Stop
        $javaPath = $javaCmd.Source
        $JavaHomePath = Split-Path (Split-Path $javaPath -Parent) -Parent
        Write-Host "Java encontrado em: $JavaHomePath" -ForegroundColor Green
    } catch {
        # Procurar em locais comuns
        $possiblePaths = @(
            "C:\Program Files\Eclipse Adoptium\jdk-17*",
            "C:\Program Files\Java\jdk-17*",
            "C:\Program Files\OpenJDK\jdk-17*"
        )
        
        foreach ($pattern in $possiblePaths) {
            $paths = Get-ChildItem -Path $pattern -Directory -ErrorAction SilentlyContinue | Select-Object -First 1
            
            if ($paths) {
                $JavaHomePath = $paths.FullName
                Write-Host "Java encontrado em: $JavaHomePath" -ForegroundColor Green
                break
            }
        }
    }
}

# Verificar se encontrou
if (-not $JavaHomePath) {
    Write-Host "Erro: Nao foi possivel encontrar o Java 17" -ForegroundColor Red
    Write-Host ""
    Write-Host "Execute primeiro: .\scripts\find-java.ps1" -ForegroundColor Yellow
    Write-Host "Depois execute: .\scripts\set-java-home.ps1 'CAMINHO_DO_JAVA'" -ForegroundColor Yellow
    exit 1
}

# Verificar se o caminho existe
if (-not (Test-Path $JavaHomePath)) {
    Write-Host "Erro: Caminho nao existe: $JavaHomePath" -ForegroundColor Red
    exit 1
}

# Verificar se tem java.exe
$javaExe = Join-Path $JavaHomePath "bin\java.exe"
if (-not (Test-Path $javaExe)) {
    Write-Host "Erro: java.exe nao encontrado em: $javaExe" -ForegroundColor Red
    exit 1
}

# Configurar JAVA_HOME
$env:JAVA_HOME = $JavaHomePath
$env:PATH = "$env:JAVA_HOME\bin;$env:PATH"

Write-Host ""
Write-Host "JAVA_HOME configurado com sucesso!" -ForegroundColor Green
Write-Host ""

# Verificar
Write-Host "Verificando configuracao:" -ForegroundColor Yellow
Write-Host "JAVA_HOME: $env:JAVA_HOME" -ForegroundColor Cyan
Write-Host ""

Write-Host "Versao do Java:" -ForegroundColor Yellow
& $javaExe -version 2>&1

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Configuracao Concluida!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "IMPORTANTE: Esta configuracao e TEMPORARIA!" -ForegroundColor Yellow
Write-Host "Ela so vale para este terminal." -ForegroundColor Yellow
Write-Host ""

Write-Host "Para configurar PERMANENTEMENTE:" -ForegroundColor Cyan
Write-Host "1. Pressione Win + R" -ForegroundColor White
Write-Host "2. Digite: sysdm.cpl" -ForegroundColor White
Write-Host "3. Aba Avancado > Variaveis de Ambiente" -ForegroundColor White
Write-Host "4. Em 'Variaveis do sistema', edite JAVA_HOME" -ForegroundColor White
Write-Host "5. Altere para: $JavaHomePath" -ForegroundColor Cyan
Write-Host "6. Clique OK em tudo" -ForegroundColor White
Write-Host "7. Feche TODOS os terminais e abra um novo" -ForegroundColor White
Write-Host ""

Write-Host "Agora execute o build:" -ForegroundColor Cyan
Write-Host "cd frontend" -ForegroundColor White
Write-Host "npx expo run:android" -ForegroundColor White
Write-Host ""
