# Script para corrigir JAVA_HOME temporariamente

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Corrigir JAVA_HOME para Java 17" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Encontrar o caminho do Java 17
Write-Host "Procurando Java 17..." -ForegroundColor Yellow
$javaPath = (Get-Command java -ErrorAction SilentlyContinue).Source

if ($javaPath) {
    # Remover \bin\java.exe do caminho
    $javaHome = Split-Path (Split-Path $javaPath -Parent) -Parent
    
    Write-Host "Java encontrado em: $javaPath" -ForegroundColor Green
    Write-Host "JAVA_HOME sera: $javaHome" -ForegroundColor Green
    Write-Host ""
    
    # Configurar JAVA_HOME
    $env:JAVA_HOME = $javaHome
    $env:PATH = "$env:JAVA_HOME\bin;$env:PATH"
    
    Write-Host "JAVA_HOME configurado!" -ForegroundColor Green
    Write-Host ""
    
    # Verificar
    Write-Host "Verificando configuracao:" -ForegroundColor Yellow
    Write-Host "JAVA_HOME: $env:JAVA_HOME" -ForegroundColor White
    Write-Host ""
    
    Write-Host "Versao do Java:" -ForegroundColor Yellow
    java -version
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "  Configuracao Concluida!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "IMPORTANTE: Esta configuracao e temporaria!" -ForegroundColor Yellow
    Write-Host "Ela so vale para este terminal." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Para configurar permanentemente:" -ForegroundColor Cyan
    Write-Host "1. Pressione Win + R" -ForegroundColor White
    Write-Host "2. Digite: sysdm.cpl" -ForegroundColor White
    Write-Host "3. Aba Avancado > Variaveis de Ambiente" -ForegroundColor White
    Write-Host "4. Edite JAVA_HOME para: $javaHome" -ForegroundColor White
    Write-Host ""
    Write-Host "Agora execute:" -ForegroundColor Cyan
    Write-Host "cd frontend" -ForegroundColor White
    Write-Host "npx expo run:android" -ForegroundColor White
    Write-Host ""
    
} else {
    Write-Host "Erro: Java nao encontrado no PATH" -ForegroundColor Red
    Write-Host "Instale o Java 17 primeiro" -ForegroundColor Red
    exit 1
}
