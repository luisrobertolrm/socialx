# Script para corrigir erro do NDK

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Corrigir Erro do NDK" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$ndkPath = "$env:LOCALAPPDATA\Android\Sdk\ndk\27.1.12297006"

Write-Host "Verificando NDK em: $ndkPath" -ForegroundColor Yellow

if (Test-Path $ndkPath) {
    Write-Host "NDK encontrado. Removendo pasta corrompida..." -ForegroundColor Yellow
    
    try {
        Remove-Item -Recurse -Force $ndkPath -ErrorAction Stop
        Write-Host "NDK removido com sucesso!" -ForegroundColor Green
    } catch {
        Write-Host "Erro ao remover NDK: $_" -ForegroundColor Red
        Write-Host "Tente remover manualmente: $ndkPath" -ForegroundColor Yellow
    }
} else {
    Write-Host "NDK nao encontrado neste caminho" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Limpando cache do Gradle..." -ForegroundColor Yellow

cd frontend\android

if (Test-Path ".\gradlew.bat") {
    .\gradlew.bat clean
    Write-Host "Cache do Gradle limpo!" -ForegroundColor Green
} else {
    Write-Host "gradlew.bat nao encontrado" -ForegroundColor Red
}

cd ..\..

Write-Host ""
Write-Host "Limpando pastas de build..." -ForegroundColor Yellow

$buildPaths = @(
    "frontend\android\build",
    "frontend\android\app\build",
    "frontend\node_modules\.cache"
)

foreach ($path in $buildPaths) {
    if (Test-Path $path) {
        Remove-Item -Recurse -Force $path -ErrorAction SilentlyContinue
        Write-Host "Removido: $path" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Limpeza Concluida!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Proximos passos:" -ForegroundColor Yellow
Write-Host ""
Write-Host "Opcao 1: Reinstalar NDK via Android Studio" -ForegroundColor Cyan
Write-Host "1. Abra Android Studio" -ForegroundColor White
Write-Host "2. Tools > SDK Manager" -ForegroundColor White
Write-Host "3. SDK Tools > NDK (Side by side)" -ForegroundColor White
Write-Host "4. Desmarque, Apply, Marque, Apply" -ForegroundColor White
Write-Host ""

Write-Host "Opcao 2: Deixar o Gradle baixar automaticamente" -ForegroundColor Cyan
Write-Host "cd frontend" -ForegroundColor White
Write-Host "npx expo run:android" -ForegroundColor White
Write-Host ""

Write-Host "O Gradle vai baixar o NDK automaticamente na primeira vez." -ForegroundColor Yellow
Write-Host "Isso pode demorar alguns minutos." -ForegroundColor Yellow
Write-Host ""
