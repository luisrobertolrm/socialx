# Corrigir Todos os Problemas - Guia Rapido

## Problemas Identificados

1. Erro de tipagem no `googleSignInService.ts` (CORRIGIDO)
2. DEVELOPER_ERROR (codigo 10) - SHA-1 nao configurado
3. NDK sem arquivo `source.properties`

## Solucao Rapida

### 1⃣ Corrigir NDK

```powershell
.\scripts\corrigir-ndk.ps1
```

Este script vai:
- Verificar se o NDK está instalado
- Criar o arquivo `source.properties` se necessário
- Limpar builds anteriores

### 2⃣ Obter SHA-1

```powershell
.\scripts\obter-sha1.ps1
```

Este script vai:
- Extrair o SHA-1 do certificado debug
- Copiar para a área de transferência
- Mostrar instruções para configurar no Google Cloud Console

### 3⃣ Configurar SHA-1 no Google Cloud Console

1. Acesse: https://console.cloud.google.com/
2. Selecione o projeto: **socialx-9be3f**
3. Menu: **APIs e Serviços** → **Credenciais**
4. Encontre a credencial Android:
   - ID: `849131252690-tmg67pl23vr7n8ggn1b87301f8klurmu.apps.googleusercontent.com`
5. Clique em **Editar**
6. Cole o SHA-1 (já está na área de transferência)
7. Verifique o package name: `com.example.socialx`
8. Clique em **Salvar**
9.  Aguarde 5-10 minutos

### 4⃣ Reconstruir e Testar

```powershell
cd frontend

# Limpar cache
npx react-native start --reset-cache
```

Em outro terminal:

```powershell
cd frontend
npx expo run:android
```

##  Verificação Final

Quando o app abrir:

1. Clique em **Login com Google**
2. Selecione sua conta
3. Verifique os logs no console:
   -  "Google Play Services disponível"
   -  "Login bem-sucedido!"
   -  Dados do usuário exibidos

##  Logs Esperados

###  Login com Sucesso:
```
 Iniciando Google Sign-In...
 Google Play Services disponível
 Login bem-sucedido!
 Resposta completa: { ... }
 Usuário: [Nome do Usuário]
 Email: [email@exemplo.com]
 Tokens obtidos
```

###  DEVELOPER_ERROR (antes da correção):
```
 Erro no Google Sign-In: Error: DEVELOPER_ERROR
� Código do erro: 10
 DEVELOPER_ERROR: SHA-1 ou package name incorreto
� Package configurado: com.example.socialx
� SHA-1 esperado: 78:DF:6A:D2:A3:15:34:C5:BE:AC:4C:CE:32:B6:18:80:82:A7:A9:98
```

##  Troubleshooting

### Se o NDK ainda der erro:

1. Abra Android Studio
2. Tools → SDK Manager
3. SDK Tools → NDK (Side by side)
4. Marque a versão 27.1.12297006
5. Clique em Apply

### Se o DEVELOPER_ERROR persistir:

1. Verifique se aguardou 5-10 minutos após salvar no Google Cloud Console
2. Execute novamente:
   ```powershell
   .\scripts\obter-sha1.ps1
   ```
3. Confirme que o SHA-1 no console é exatamente:
   ```
   78:DF:6A:D2:A3:15:34:C5:BE:AC:4C:CE:32:B6:18:80:82:A7:A9:98
   ```
4. Confirme que o package name é:
   ```
   com.example.socialx
   ```

### Se o build falhar:

```powershell
cd frontend

# Limpar tudo
Remove-Item -Recurse -Force android\.gradle
Remove-Item -Recurse -Force android\app\build
Remove-Item -Recurse -Force android\app\.cxx
Remove-Item -Recurse -Force node_modules
Remove-Item -Recurse -Force .expo

# Reinstalar
npm install

# Rebuild
npx expo prebuild --clean
npx expo run:android
```

##  Arquivos Importantes

- `frontend/src/services/googleSignInService.ts` - Serviço de autenticação (CORRIGIDO )
- `frontend/google-services.json` - Configuração do Firebase
- `frontend/app.json` - Package name do app
- `frontend/android/app/build.gradle` - ApplicationId
- `frontend/android/app/debug.keystore` - Certificado debug

##  Checklist

- [ ] Script `corrigir-ndk.ps1` executado
- [ ] Script `obter-sha1.ps1` executado
- [ ] SHA-1 configurado no Google Cloud Console
- [ ] Aguardado 5-10 minutos
- [ ] Cache limpo
- [ ] App reconstruído
- [ ] Login testado com sucesso

##  Sucesso!

Se tudo funcionou, você verá:
-  Login com Google funcionando
-  Dados do usuário exibidos
-  Tokens obtidos corretamente

##  Próximos Passos

Após o login funcionar:
1. Integrar com o backend
2. Salvar tokens no AsyncStorage
3. Implementar refresh token
4. Adicionar logout
5. Testar em dispositivo físico
