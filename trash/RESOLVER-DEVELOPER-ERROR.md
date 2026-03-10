# Resolver DEVELOPER_ERROR (Código 10)

##  Erro

```
DEVELOPER_ERROR: Follow troubleshooting instructions
Erro: 10
```

##  Causa

O SHA-1 fingerprint ou package name configurado no Google Cloud Console não corresponde ao app compilado.

##  Solução Completa

### Passo 1: Obter o SHA-1 Correto do Seu App

Execute este comando na pasta do projeto:

```powershell
cd frontend\android
.\gradlew signingReport
```

Procure por esta seção na saída:

```
Variant: debug
Config: debug
Store: C:\Users\luisr\.android\debug.keystore
Alias: AndroidDebugKey
MD5: XX:XX:XX...
SHA1: 78:DF:6A:D2:A3:15:34:C5:BE:AC:4C:CE:32:B6:18:80:82:A7:A9:98
SHA-256: XX:XX:XX...
```

**Copie o SHA1** (exemplo acima).

### Passo 2: Verificar o Package Name

O package name está em `frontend/app.json`:

```json
"android": {
  "package": "com.example.socialx"
}
```

### Passo 3: Configurar no Google Cloud Console

1. **Acessar Credentials**
   - Link: https://console.cloud.google.com/apis/credentials?project=socialx-9be3f

2. **Encontrar ou Criar Android Client ID**
   
   **Se JÁ EXISTE um Android Client ID:**
   - Clique nele para editar
   - Verifique se o Package name é: `com.example.socialx`
   - Verifique se o SHA-1 corresponde ao obtido no Passo 1
   - Se estiver diferente, ATUALIZE
   - Clique em **SAVE**

   **Se NÃO EXISTE:**
   - Clique em **+ CREATE CREDENTIALS**
   - Escolha: **OAuth client ID**
   - Application type: **Android**
   - Name: `SocialX Android`
   - Package name: `com.example.socialx`
   - SHA-1 certificate fingerprint: [Cole o SHA-1 do Passo 1]
   - Clique em **CREATE**

3. **Aguardar Propagação**
   - Aguarde 2-3 minutos

### Passo 4: Atualizar o Web Client ID no Código

Copie o **Web Client ID** (tipo 3) do Google Cloud Console e verifique se está correto em `frontend/src/config/googleAuth.ts`:

```typescript
export const GOOGLE_CONFIG = {
  androidClientId: '[Android Client ID]',
  webClientId: '849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com',
  // ...
};
```

### Passo 5: Recompilar o App

```powershell
cd frontend

# Limpar build anterior
cd android
.\gradlew clean
cd ..

# Recompilar
npx expo run:android
```

### Passo 6: Testar Novamente

Após o app abrir, tente fazer login novamente.

##  Script Automático para Obter SHA-1

Criei um script para facilitar:

```powershell
.\scripts\get-sha1.ps1
```

## � Checklist

- [ ] Executou: `cd frontend\android && .\gradlew signingReport`
- [ ] Copiou o SHA-1 da seção "Variant: debug"
- [ ] Verificou package name: `com.example.socialx`
- [ ] Acessou Google Cloud Console
- [ ] Criou/atualizou Android Client ID com SHA-1 correto
- [ ] Aguardou 2-3 minutos
- [ ] Limpou build: `.\gradlew clean`
- [ ] Recompilou: `npx expo run:android`
- [ ] Testou login novamente

##  Troubleshooting

### SHA-1 Não Aparece no signingReport

**Solução**: Verifique se o arquivo `debug.keystore` existe:

```powershell
Test-Path "$env:USERPROFILE\.android\debug.keystore"
```

Se não existir, o Android Studio cria automaticamente na primeira compilação.

### Erro Persiste Após Configurar

**Causas possíveis**:
1. Aguarde mais 2-3 minutos (propagação)
2. Verifique se usou o SHA-1 correto (debug, não release)
3. Verifique se o package name está exatamente igual
4. Limpe o cache: `.\gradlew clean`

### Múltiplos SHA-1

Você pode adicionar múltiplos SHA-1 no mesmo Android Client ID:
- SHA-1 de debug (desenvolvimento)
- SHA-1 de release (produção)

##  Configuração Final Esperada

No Google Cloud Console, seu Android Client ID deve ter:

```
Name: SocialX Android
Package name: com.example.socialx
SHA-1 certificate fingerprints:
  - 78:DF:6A:D2:A3:15:34:C5:BE:AC:4C:CE:32:B6:18:80:82:A7:A9:98 (ou o seu)
```

## � Links Úteis

- [Google Cloud Console - Credentials](https://console.cloud.google.com/apis/credentials?project=socialx-9be3f)
- [Troubleshooting Google Sign-In](https://github.com/react-native-google-signin/google-signin/blob/master/docs/android-guide.md#troubleshooting)
