# Resolver Erro OAuth 400: invalid_request

## � Erro Atual

```
Error 400: invalid_request
Access blocked: Authorization Error
This app doesn't comply with Google's OAuth 2.0 policy
```

##  Causas Possíveis

### 1. Redirect URI Não Autorizado
O redirect URI usado pelo app não está na lista de URIs autorizados no Google Cloud Console.

### 2. App em Modo de Teste
O app pode estar em modo de teste e seu email não está na lista de usuários de teste.

### 3. Tela de Consentimento Não Configurada
A OAuth consent screen pode não estar configurada corretamente.

##  Soluções

### Solução 1: Verificar Redirect URI nos Logs

1. Abra o console do app e procure por:
   ```
   � Redirect URI gerado: [URI]
   ```

2. Copie esse URI exato

3. Adicione no Google Cloud Console:
   - Acesse: https://console.cloud.google.com/
   - Projeto: **socialx-9be3f**
   - **APIs & Services > Credentials**
   - Edite o **Web Client ID**
   - Adicione o URI em **Authorized redirect URIs**

### Solução 2: Adicionar Usuário de Teste

1. Acesse: https://console.cloud.google.com/
2. Projeto: **socialx-9be3f**
3. **APIs & Services > OAuth consent screen**
4. Role até **Test users**
5. Clique em **+ ADD USERS**
6. Adicione: `luisrobertolrm@gmail.com`
7. Clique em **SAVE**

### Solução 3: Configurar OAuth Consent Screen

1. Acesse: https://console.cloud.google.com/
2. Projeto: **socialx-9be3f**
3. **APIs & Services > OAuth consent screen**
4. Preencha os campos obrigatórios:
   - **App name**: SocialX
   - **User support email**: seu email
   - **Developer contact information**: seu email
5. Em **Scopes**, adicione:
   - `userinfo.email`
   - `userinfo.profile`
6. Clique em **SAVE AND CONTINUE**

### Solução 4: Publicar o App (Opcional)

Se quiser que qualquer pessoa possa fazer login:

1. **OAuth consent screen**
2. Clique em **PUBLISH APP**
3. Confirme a publicação

 **Nota**: Para apps em produção, você precisará passar pela verificação do Google.

##  Verificar Configuração Atual

### Passo 1: Ver o Redirect URI Usado

Recarregue o app e veja o console:
```
� Redirect URI gerado: [URI]
```

Possíveis URIs:
- `https://auth.expo.io/@luisrobertolrm/social-x-native`
- `socialx://redirect`
- `exp://[IP]:8081/--/redirect`

### Passo 2: Adicionar TODOS os Redirect URIs

No Google Cloud Console, adicione:

```
https://auth.expo.io/@luisrobertolrm/social-x-native
socialx://redirect
exp://localhost:8081/--/redirect
```

### Passo 3: Verificar Client IDs

Certifique-se de que está usando os Client IDs corretos:

**Web Client ID** (para Expo):
```
849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com
```

**Android Client ID**:
```
849131252690-tmg67pl23vr7n8ggn1b87301f8klurmu.apps.googleusercontent.com
```

## � Testar Novamente

Após fazer as mudanças:

1. Aguarde 1-2 minutos
2. Recarregue o app (pressione `r` no Metro)
3. Tente fazer login novamente
4. Verifique os logs no console

## � Checklist Completo

- [ ] OAuth consent screen configurada
- [ ] Email adicionado como usuário de teste
- [ ] Redirect URIs adicionados no Web Client ID
- [ ] Client IDs corretos no código
- [ ] App recarregado após mudanças
- [ ] Aguardou 1-2 minutos para propagação

##  Se Ainda Não Funcionar

### Opção 1: Usar Expo Go com Proxy

O Expo Go pode ter limitações. Considere fazer um build standalone:

```bash
eas build --profile development --platform android
```

### Opção 2: Verificar Logs Detalhados

Adicione mais logs no componente:

```typescript
console.log('Request:', request);
console.log('Response completa:', JSON.stringify(response, null, 2));
```

### Opção 3: Testar com Google Sign-In Nativo

Use `@react-native-google-signin/google-signin` em vez de `expo-auth-session`:

```bash
npm install @react-native-google-signin/google-signin
```

##  Próximos Passos

1. Verifique o redirect URI nos logs
2. Adicione seu email como usuário de teste
3. Configure a OAuth consent screen
4. Adicione todos os redirect URIs possíveis
5. Teste novamente

## � Links Úteis

- [Google Cloud Console](https://console.cloud.google.com/)
- [OAuth Consent Screen](https://console.cloud.google.com/apis/credentials/consent)
- [Credentials](https://console.cloud.google.com/apis/credentials)
- [Expo Auth Session Docs](https://docs.expo.dev/versions/latest/sdk/auth-session/)
