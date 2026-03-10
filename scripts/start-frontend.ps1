# Script para iniciar o frontend
Write-Host "Iniciando o frontend React Native..." -ForegroundColor Green

# Navegar para a pasta do frontend
Set-Location -Path "$PSScriptRoot\..\frontend"

# Verificar se node_modules existe
if (-not (Test-Path "node_modules")) {
    Write-Host "Instalando dependências..." -ForegroundColor Yellow
    npm install
}

Write-Host ""
Write-Host "Iniciando o Expo..." -ForegroundColor Green
Write-Host "Pressione 'w' para abrir no navegador" -ForegroundColor Cyan
Write-Host "Pressione 'a' para Android" -ForegroundColor Cyan
Write-Host "Pressione 'i' para iOS" -ForegroundColor Cyan
Write-Host ""

# Iniciar o Expo
npm start
