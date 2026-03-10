# Script para obter o SHA-1 do app

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Obter SHA-1 Fingerprint" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Executando gradlew signingReport..." -ForegroundColor Yellow
Write-Host ""

cd frontend\android

if (Test-Path ".\gradlew.bat") {
    $output = .\gradlew.bat signingReport 2>&1 | Out-String
    
    # Procurar pelo SHA1 na saída
    if ($output -match "SHA1: ([A-F0-9:]+)") {
        $sha1 = $matches[1]
        
        Write-Host "========================================" -ForegroundColor Green
        Write-Host "  SHA-1 Encontrado!" -ForegroundColor Green
        Write-Host "========================================" -ForegroundColor Green
        Write-Host ""
        Write-Host "SHA-1: $sha1" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "========================================" -ForegroundColor Cyan
        Write-Host "  Proximos Passos" -ForegroundColor Cyan
        Write-Host "========================================" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "1. Copie o SHA-1 acima" -ForegroundColor White
        Write-Host ""
        Write-Host "2. Acesse o Google Cloud Console:" -ForegroundColor White
        Write-Host "   https://console.cloud.google.com/apis/credentials?project=socialx-9be3f" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "3. Crie ou edite o Android Client ID:" -ForegroundColor White
        Write-Host "   - Package name: com.example.socialx" -ForegroundColor White
        Write-Host "   - SHA-1: $sha1" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "4. Aguarde 2-3 minutos" -ForegroundColor White
        Write-Host ""
        Write-Host "5. Recompile o app:" -ForegroundColor White
        Write-Host "   cd frontend" -ForegroundColor White
        Write-Host "   npx expo run:android" -ForegroundColor White
        Write-Host ""
        
        # Copiar para clipboard se possível
        try {
            Set-Clipboard -Value $sha1
            Write-Host "SHA-1 copiado para a area de transferencia!" -ForegroundColor Green
        } catch {
            Write-Host "Nao foi possivel copiar automaticamente" -ForegroundColor Yellow
        }
        
    } else {
        Write-Host "SHA-1 nao encontrado na saida" -ForegroundColor Red
        Write-Host ""
        Write-Host "Saida completa:" -ForegroundColor Yellow
        Write-Host $output
    }
    
} else {
    Write-Host "Erro: gradlew.bat nao encontrado" -ForegroundColor Red
    Write-Host "Execute primeiro: npx expo prebuild" -ForegroundColor Yellow
}

cd ..\..

Write-Host ""
