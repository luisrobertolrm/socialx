# Script para corrigir build do Google Sign-In

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Fix Google Sign-In Build" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

cd frontend

Write-Host "1. Limpando cache do Metro..." -ForegroundColor Yellow
Remove-Item -Recurse -Force node_modules\.cache -ErrorAction SilentlyContinue

Write-Host "2. Limpando build do Android..." -ForegroundColor Yellow
Remove-Item -Recurse -Force android\.gradle -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force android\build -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force android\app\build -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force android\app\.cxx -ErrorAction SilentlyContinue

Write-Host "3. Limpando Gradle..." -ForegroundColor Yellow
cd android
.\gradlew clean
cd ..

Write-Host "4. Reinstalando dependencias..." -ForegroundColor Yellow
Remove-Item -Recurse -Force node_modules\@react-native-google-signin -ErrorAction SilentlyContinue
npm install @react-native-google-signin/google-signin

Write-Host "5. Executando prebuild novamente..." -ForegroundColor Yellow
npx expo prebuild --clean

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Limpeza Concluida!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Agora execute:" -ForegroundColor Yellow
Write-Host "cd frontend" -ForegroundColor White
Write-Host "npx expo run:android" -ForegroundColor White
Write-Host ""

cd ..
