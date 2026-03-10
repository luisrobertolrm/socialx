# Migração para Google Sign-In Nativo

##  O Que Foi Feito

Migrei o projeto de `expo-auth-session` para `@react-native-google-signin/google-signin`.

### Arquivos Criados

1. **Services**
   - `frontend/src/services/googleSignInService.ts` - Serviço nativo

2. **Components**
   - `frontend/src/components/GoogleSignInTestScreen.tsx` - Nova tela de teste

3. **Scripts**
   - `scripts/setup-google-signin-native.ps1` - Instalação automática

4. **Documentação**
   - `frontend/GOOGLE-SIGNIN-NATIVE.md` - Guia completo

### Arquivos Atualizados

- `frontend/App.tsx` - Usa o novo componente

##  Como Instalar e Testar

### Passo 1: Executar Script de Instalação

```powershell
.\scripts\setup-google-signin-native.ps1
```

Este script vai:
1. Instalar `@react-native-google-signin/google-signin`
2. Executar `expo prebuild --clean` para configurar módulos nativos
3. Preparar o projeto para build

### Passo 2: Iniciar o App

```bash
cd frontend
npm start
```

Pressione `a` para abrir no Android

### Passo 3: Testar Login

1. Clique em " Entrar com Google"
2. Selecione uma conta
3.  Deve funcionar!

##  Vantagens da Migração

###  Sem Redirect URI
- Não precisa mais configurar `https://auth.expo.io/@owner/slug`
- Sem problemas de "redirect_uri_mismatch"
- Configuração muito mais simples

###  Mais Confiável
- Usa APIs nativas do Google
- Menos erros de configuração
- Funciona offline

###  Melhor UX
- Seletor de conta nativo do Android
- Login mais rápido
- Mantém sessão automaticamente

##  Configuração do Google Cloud Console

### O Que Você Precisa

Apenas verificar/criar o **Android Client ID**:

1. **Acessar**: https://console.cloud.google.com/apis/credentials?project=socialx-9be3f

2. **Verificar se existe Android Client ID** com:
   - Package: `com.example.socialx`
   - SHA-1: `78df6ad2a31534c5beac4cce32b6188082a7a998`

3. **Se não existir, criar**:
   - Clique em **+ CREATE CREDENTIALS > OAuth client ID**
   - Tipo: **Android**
   - Package name: `com.example.socialx`
   - SHA-1: `78df6ad2a31534c5beac4cce32b6188082a7a998`

4. **Adicionar usuário de teste**:
   - Link: https://console.cloud.google.com/apis/credentials/consent?project=socialx-9be3f
   - Adicione: `luisrobertolrm@gmail.com`

###  O Que NÃO Precisa Mais

-  Authorized redirect URIs
-  Authorized JavaScript origins
-  Web Client ID (ainda é usado, mas não precisa configurar redirect)

## � Checklist

- [ ] Executar: `.\scripts\setup-google-signin-native.ps1`
- [ ] Verificar Android Client ID no Google Cloud Console
- [ ] Package: `com.example.socialx`
- [ ] SHA-1: `78df6ad2a31534c5beac4cce32b6188082a7a998`
- [ ] Usuário de teste: `luisrobertolrm@gmail.com`
- [ ] Iniciar app: `npm start`
- [ ] Testar login

##  Diferenças Técnicas

### Antes (expo-auth-session)
```typescript
// Precisava configurar redirect URI
const redirectUri = 'https://auth.expo.io/@owner/slug';

// Abria navegador web
await promptAsync();

// Dependia do proxy do Expo
```

### Agora (google-signin nativo)
```typescript
// Sem redirect URI
GoogleSignin.configure({
  webClientId: GOOGLE_CONFIG.webClientId,
});

// Usa seletor nativo
await GoogleSignin.signIn();

// Direto com Google Play Services
```

##  Importante

### Expo Go NÃO Funciona

O `@react-native-google-signin/google-signin` usa módulos nativos, então:

-  **Expo Go**: Não funciona
-  **Expo Dev Client**: Funciona (após prebuild)
-  **Build Standalone**: Funciona

### Prebuild é Necessário

Você DEVE executar `expo prebuild` para configurar os módulos nativos:

```bash
npx expo prebuild --clean
```

##  Resultado Esperado

Após instalação e configuração:

```
 Configurando Google Sign-In...
 Google Sign-In configurado

 Iniciando Google Sign-In...
 Google Play Services disponível
 Login bem-sucedido!
 Usuário: [Seu Nome]
 Email: luisrobertolrm@gmail.com
 Tokens obtidos
```

##  Documentação

Leia `frontend/GOOGLE-SIGNIN-NATIVE.md` para:
- Guia completo de instalação
- Troubleshooting detalhado
- Exemplos de uso
- Referências

## � Links Úteis

- [Android Client ID](https://console.cloud.google.com/apis/credentials?project=socialx-9be3f)
- [OAuth Consent Screen](https://console.cloud.google.com/apis/credentials/consent?project=socialx-9be3f)
- [Documentação Oficial](https://github.com/react-native-google-signin/google-signin)
