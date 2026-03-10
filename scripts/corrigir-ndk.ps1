# Script para corrigir problema do NDK

Write-Host "Corrigindo problema do NDK..." -ForegroundColor Cyan
Write-Host ""

$ndkPath = "$env:LOCALAPPDATA\Android\Sdk\ndk\27.1.12297006"
$sourcePropertiesPath = "$ndkPath\source.properties"

Write-Host "Verificando NDK em: $ndkPath" -ForegroundColor Yellow

if (-not (Test-Path $ndkPath)) {
    Write-Host "NDK nao encontrado!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Instalando NDK via Android SDK Manager..." -ForegroundColor Yellow
    
    $sdkmanagerPath = "$env:LOCALAPPDATA\Android\Sdk\cmdline-tools\latest\bin\sdkmanager.bat"
    
    if (Test-Path $sdkmanagerPath) {
        $sdkRoot = "$env:LOCALAPPDATA\Android\Sdk"
        & $sdkmanagerPath "ndk;27.1.12297006" "--sdk_root=$sdkRoot"
        Write-Host "NDK instalado!" -ForegroundColor Green
    } else {
        Write-Host "SDK Manager nao encontrado!" -ForegroundColor Red
        Write-Host ""
        Write-Host "Instale manualmente:" -ForegroundColor Yellow
        Write-Host "1. Abra Android Studio" -ForegroundColor White
        Write-Host "2. Tools -> SDK Manager" -ForegroundColor White
        Write-Host "3. SDK Tools -> NDK (Side by side)" -ForegroundColor White
        Write-Host "4. Marque a versao 27.1.12297006" -ForegroundColor White
        Write-Host "5. Clique em Apply" -ForegroundColor White
        exit 1
    }
}

Write-Host "NDK encontrado!" -ForegroundColor Green
Write-Host ""

# Verificar se source.properties existe
if (-not (Test-Path $sourcePropertiesPath)) {
    Write-Host "source.properties nao encontrado!" -ForegroundColor Yellow
    Write-Host "Criando source.properties..." -ForegroundColor Cyan
    
    $sourcePropertiesContent = @"
Pkg.Desc = Android NDK
Pkg.Revision = 27.1.12297006
"@
    
    Set-Content -Path $sourcePropertiesPath -Value $sourcePropertiesContent -Encoding UTF8
    Write-Host "source.properties criado!" -ForegroundColor Green
} else {
    Write-Host "source.properties ja existe!" -ForegroundColor Green
}

Write-Host ""
Write-Host "Limpando builds anteriores..." -ForegroundColor Cyan

Set-Location frontend

# Limpar .cxx
if (Test-Path "android\app\.cxx") {
    Remove-Item -Path "android\app\.cxx" -Recurse -Force
    Write-Host ".cxx removido" -ForegroundColor Green
}

# Limpar build
if (Test-Path "android\app\build") {
    Remove-Item -Path "android\app\build" -Recurse -Force
    Write-Host "build removido" -ForegroundColor Green
}

# Limpar .gradle
if (Test-Path "android\.gradle") {
    Remove-Item -Path "android\.gradle" -Recurse -Force
    Write-Host ".gradle removido" -ForegroundColor Green
}

Write-Host ""
Write-Host "Correcao concluida!" -ForegroundColor Green
Write-Host ""
Write-Host "Proximo passo:" -ForegroundColor Yellow
Write-Host "   npx expo run:android" -ForegroundColor White
Write-Host ""

Set-Location ..

Write-Host "Pressione qualquer tecla para continuar..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
