# Script para configurar autenticacao Google no frontend

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Setup Google Authentication" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Navegar para o diretorio do frontend
Set-Location -Path "frontend"

Write-Host "Instalando dependencias do Google Auth..." -ForegroundColor Yellow
npm install expo-auth-session expo-web-browser

if ($LASTEXITCODE -eq 0) {
    Write-Host "Dependencias instaladas com sucesso!" -ForegroundColor Green
} else {
    Write-Host "Erro ao instalar dependencias" -ForegroundColor Red
    Set-Location -Path ".."
    exit 1
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Configuracao Concluida!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Proximos passos:" -ForegroundColor Yellow
Write-Host "1. Execute: npm start" -ForegroundColor White
Write-Host "2. Pressione 'a' para abrir no Android" -ForegroundColor White
Write-Host "3. Teste o botao 'Entrar com Google'" -ForegroundColor White
Write-Host ""
Write-Host "Leia: frontend/GOOGLE-AUTH-SETUP.md para mais detalhes" -ForegroundColor Cyan
Write-Host ""

Set-Location -Path ".."
