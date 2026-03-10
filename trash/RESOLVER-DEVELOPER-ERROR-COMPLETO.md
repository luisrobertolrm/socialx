# Resolver DEVELOPER_ERROR no Google Sign-In

## Problema
Erro: `DEVELOPER_ERROR` (codigo 10) ao tentar fazer login com Google

## Causa
O SHA-1 do certificado debug nao esta registrado corretamente no Google Cloud Console

## Solucao Completa

### 1⃣ Verificar SHA-1 Atual

Execute no terminal:

```powershell
cd frontend/android
./gradlew signingReport
```

Procure por:
```
Variant: debug
Config: debug
Store: E:\Projetos Kiro\SocialX\frontend\android\app\debug.keystore
Alias: androiddebugkey
SHA1: 78:DF:6A:D2:A3:15:34:C5:BE:AC:4C:CE:32:B6:18:80:82:A7:A9:98
```

### 2⃣ Acessar Google Cloud Console

1. Acesse: https://console.cloud.google.com/
2. Selecione o projeto: **socialx-9be3f**
3. Menu lateral → **APIs e Serviços** → **Credenciais**

### 3⃣ Verificar/Atualizar Credencial Android

Procure pela credencial:
- **Tipo**: ID do cliente OAuth 2.0 Android
- **Nome**: Cliente Android 1
- **ID**: `849131252690-tmg67pl23vr7n8ggn1b87301f8klurmu.apps.googleusercontent.com`

Clique para editar e verifique:

####  Configurações Corretas:
```
Nome do pacote: com.example.socialx
Impressão digital do certificado SHA-1: 78:DF:6A:D2:A3:15:34:C5:BE:AC:4C:CE:32:B6:18:80:82:A7:A9:98
```

### 4⃣ Se o SHA-1 Estiver Diferente

Se o SHA-1 no Google Cloud Console for diferente do seu SHA-1 local:

**Opção A: Atualizar no Google Cloud Console**
1. Clique em **Editar** na credencial Android
2. Atualize o campo **Impressão digital do certificado SHA-1**
3. Cole o SHA-1 correto: `78:DF:6A:D2:A3:15:34:C5:BE:AC:4C:CE:32:B6:18:80:82:A7:A9:98`
4. Clique em **Salvar**

**Opção B: Adicionar SHA-1 Adicional**
1. Na mesma tela de edição
2. Clique em **+ Adicionar impressão digital**
3. Cole o novo SHA-1
4. Clique em **Salvar**

### 5⃣ Aguardar Propagação

 Aguarde 5-10 minutos para as alterações propagarem

### 6⃣ Limpar e Reconstruir

```powershell
cd frontend

# Limpar cache do Metro
npx react-native start --reset-cache

# Em outro terminal
cd frontend
npx expo run:android
```

### 7⃣ Testar Login

1. Abra o app no emulador/dispositivo
2. Clique em **Login com Google**
3. Selecione sua conta Google
4. Verifique se o login funciona

##  Verificação de Configuração

### Arquivo: `frontend/app.json`
```json
{
  "android": {
    "package": "com.example.socialx"
  }
}
```

### Arquivo: `frontend/android/app/build.gradle`
```gradle
android {
    namespace 'com.example.socialx'
    defaultConfig {
        applicationId 'com.example.socialx'
    }
}
```

### Arquivo: `frontend/google-services.json`
```json
{
  "client": [{
    "client_info": {
      "android_client_info": {
        "package_name": "com.example.socialx"
      }
    },
    "oauth_client": [{
      "client_id": "849131252690-tmg67pl23vr7n8ggn1b87301f8klurmu.apps.googleusercontent.com",
      "client_type": 1,
      "android_info": {
        "package_name": "com.example.socialx",
        "certificate_hash": "78df6ad2a31534c5beac4cce32b6188082a7a998"
      }
    }]
  }]
}
```

## � Checklist Final

- [ ] SHA-1 obtido com `./gradlew signingReport`
- [ ] SHA-1 registrado no Google Cloud Console
- [ ] Package name correto: `com.example.socialx`
- [ ] Aguardado 5-10 minutos após alteração
- [ ] Cache limpo e app reconstruído
- [ ] Login testado

##  Se Ainda Não Funcionar

1. **Verificar logs detalhados**:
   ```powershell
   npx react-native log-android
   ```

2. **Verificar se o certificado está correto**:
   ```powershell
   keytool -list -v -keystore frontend/android/app/debug.keystore -alias androiddebugkey -storepass android -keypass android
   ```

3. **Criar nova credencial OAuth**:
   - No Google Cloud Console
   - Criar novo ID do cliente OAuth 2.0
   - Tipo: Android
   - Package: `com.example.socialx`
   - SHA-1: `78:DF:6A:D2:A3:15:34:C5:BE:AC:4C:CE:32:B6:18:80:82:A7:A9:98`

##  Referências

- [Google Sign-In Android Setup](https://developers.google.com/identity/sign-in/android/start-integrating)
- [React Native Google Sign-In](https://github.com/react-native-google-signin/google-signin)
- [Troubleshooting DEVELOPER_ERROR](https://developers.google.com/identity/sign-in/android/troubleshooting)
