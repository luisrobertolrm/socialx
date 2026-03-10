# Script para iniciar o backend
Write-Host "Iniciando o backend SocialX API..." -ForegroundColor Green

# Navegar para a pasta da API
Set-Location -Path "$PSScriptRoot\..\src\SocialX.Api"

# Verificar se o projeto compila
Write-Host "Compilando o projeto..." -ForegroundColor Yellow
dotnet build

if ($LASTEXITCODE -eq 0) {
    Write-Host "Compilação bem-sucedida!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Iniciando a API..." -ForegroundColor Green
    Write-Host "Swagger estará disponível em: https://localhost:7000/swagger" -ForegroundColor Cyan
    Write-Host ""
    
    # Iniciar a API
    dotnet run
} else {
    Write-Host "Erro na compilação. Verifique os erros acima." -ForegroundColor Red
    exit 1
}
