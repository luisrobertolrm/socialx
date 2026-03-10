# Script para configurar o banco de dados
Write-Host "Configurando o banco de dados..." -ForegroundColor Green

# Verificar se o PostgreSQL está rodando
Write-Host "Verificando se o PostgreSQL está rodando..." -ForegroundColor Yellow

try {
    $pgService = Get-Service -Name "postgresql*" -ErrorAction SilentlyContinue
    if ($pgService) {
        Write-Host "PostgreSQL encontrado: $($pgService.DisplayName)" -ForegroundColor Green
        if ($pgService.Status -ne "Running") {
            Write-Host "PostgreSQL não está rodando. Iniciando..." -ForegroundColor Yellow
            Start-Service $pgService.Name
        }
    } else {
        Write-Host "Serviço PostgreSQL não encontrado. Certifique-se de que está instalado." -ForegroundColor Red
    }
} catch {
    Write-Host "Não foi possível verificar o status do PostgreSQL." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Criando migrations..." -ForegroundColor Yellow

# Navegar para a raiz do projeto
Set-Location -Path "$PSScriptRoot\.."

# Criar migrations
dotnet ef migrations add InitialCreate --project src/SocialX.Infra --startup-project src/SocialX.Api

if ($LASTEXITCODE -eq 0) {
    Write-Host "Migrations criadas com sucesso!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Aplicando migrations ao banco de dados..." -ForegroundColor Yellow
    
    # Aplicar migrations
    dotnet ef database update --project src/SocialX.Infra --startup-project src/SocialX.Api
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Banco de dados configurado com sucesso!" -ForegroundColor Green
    } else {
        Write-Host "Erro ao aplicar migrations. Verifique a connection string em appsettings.json" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "Erro ao criar migrations." -ForegroundColor Red
    exit 1
}
