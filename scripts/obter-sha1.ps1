# Script para obter SHA-1 do certificado debug

Write-Host "Obtendo SHA-1 do certificado debug..." -ForegroundColor Cyan
Write-Host ""

$keystorePath = "frontend\android\app\debug.keystore"

if (-not (Test-Path $keystorePath)) {
    Write-Host "Keystore nao encontrado em: $keystorePath" -ForegroundColor Red
    exit 1
}

Write-Host "Keystore encontrado!" -ForegroundColor Green
Write-Host ""

Write-Host "Informacoes do certificado:" -ForegroundColor Yellow
Write-Host ""

# Executar keytool
$keytoolOutput = keytool -list -v -keystore $keystorePath -alias androiddebugkey -storepass android -keypass android 2>&1

# Extrair SHA-1
$sha1 = $keytoolOutput | Select-String -Pattern "SHA1: (.+)" | ForEach-Object { $_.Matches.Groups[1].Value }

if ($sha1) {
    Write-Host "SHA-1:" -ForegroundColor Green
    Write-Host "   $sha1" -ForegroundColor White
    Write-Host ""
    
    # Copiar para clipboard
    $sha1 | Set-Clipboard
    Write-Host "SHA-1 copiado para a area de transferencia!" -ForegroundColor Green
    Write-Host ""
    
    Write-Host "Proximos passos:" -ForegroundColor Yellow
    Write-Host "1. Acesse: https://console.cloud.google.com/" -ForegroundColor White
    Write-Host "2. Selecione o projeto: socialx-9be3f" -ForegroundColor White
    Write-Host "3. Va em: APIs e Servicos -> Credenciais" -ForegroundColor White
    Write-Host "4. Edite a credencial Android" -ForegroundColor White
    Write-Host "5. Cole o SHA-1 (ja esta na area de transferencia)" -ForegroundColor White
    Write-Host "6. Salve e aguarde 5-10 minutos" -ForegroundColor White
    Write-Host ""
    
    Write-Host "Package name: com.example.socialx" -ForegroundColor Cyan
    Write-Host ""
} else {
    Write-Host "Nao foi possivel extrair o SHA-1" -ForegroundColor Red
    Write-Host ""
    Write-Host "Saida completa do keytool:" -ForegroundColor Yellow
    Write-Host $keytoolOutput
}

Write-Host "Pressione qualquer tecla para continuar..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
