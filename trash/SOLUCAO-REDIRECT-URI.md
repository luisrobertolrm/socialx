# Solução: Redirect URI para Expo

##  Problema

O Expo estava gerando: `exp://10.0.0.100:8081`

Este URI não é aceito pelo Google porque:
- Não é HTTPS
- Não tem um domínio público válido (.com, .org, etc)

##  Solução

O `expo-auth-session` com Google automaticamente usa o proxy do Expo quando você não especifica um `redirectUri` customizado.

O URI correto que será usado:
```
https://auth.expo.io/@luisrobertolrm/social-x-native
```

##  O Que Foi Alterado

Removi a configuração manual do `redirectUri`. Agora o `Google.useAuthRequest` usa automaticamente o proxy do Expo:

```typescript
const [request, response, promptAsync] = Google.useAuthRequest({
  clientId: GOOGLE_CONFIG.webClientId,
  androidClientId: GOOGLE_CONFIG.androidClientId,
  scopes: GOOGLE_CONFIG.scopes,
  // redirectUri removido - usa proxy automático
});
```

## � Adicionar no Google Cloud Console

### URI a Adicionar:
```
https://auth.expo.io/@luisrobertolrm/social-x-native
```

### Passo a Passo:

1. **Acessar Credentials**
   - Link: https://console.cloud.google.com/apis/credentials?project=socialx-9be3f

2. **Editar Web Client ID**
   - Clique em: `849131252690-f59f7p3f5el0egjesad4asq0thi0vbie`

3. **Adicionar Redirect URI**
   - Role até **Authorized redirect URIs**
   - Clique em **+ ADD URI**
   - Cole: `https://auth.expo.io/@luisrobertolrm/social-x-native`
   - Clique em **SAVE**

4. **Adicionar Usuário de Teste**
   - Link: https://console.cloud.google.com/apis/credentials/consent?project=socialx-9be3f
   - Role até **Test users**
   - Clique em **+ ADD USERS**
   - Adicione: `luisrobertolrm@gmail.com`
   - Clique em **SAVE**

## � Testar

1. **Recarregue o app** (pressione `r` no Metro)
2. Você verá no console:
   ```
   � REDIRECT URI CORRETO:
   https://auth.expo.io/@luisrobertolrm/social-x-native
   ```
3. **Aguarde 1-2 minutos** após adicionar no Google Cloud Console
4. **Clique em " Entrar com Google"**
5.  Deve funcionar!

##  Como Funciona

### Fluxo de Autenticação:

1. App abre navegador com URL do Google
2. Usuário faz login no Google
3. Google redireciona para: `https://auth.expo.io/@luisrobertolrm/social-x-native?code=...`
4. Proxy do Expo captura o código
5. Proxy redireciona de volta para o app: `exp://10.0.0.100:8081?code=...`
6. App recebe o código e troca por tokens

### Por Que Usar o Proxy?

-  Google aceita URLs HTTPS com domínio válido
-  Funciona no Expo Go sem build
-  Não precisa configurar deep linking
-  Funciona em desenvolvimento

### Quando NÃO Usar o Proxy?

Para builds standalone (produção), você deve:
- Usar `redirectUri` customizado
- Configurar deep linking no app
- Exemplo: `com.example.socialx:/oauth2redirect/google`

##  Checklist Final

- [ ] Código atualizado (sem redirectUri manual)
- [ ] App recarregado
- [ ] URI adicionado no Google Cloud Console: `https://auth.expo.io/@luisrobertolrm/social-x-native`
- [ ] Email adicionado como usuário de teste: `luisrobertolrm@gmail.com`
- [ ] OAuth consent screen configurada
- [ ] Aguardou 1-2 minutos
- [ ] Testou o login

##  Resultado Esperado

```
� REDIRECT URI CORRETO:
https://auth.expo.io/@luisrobertolrm/social-x-native

 Iniciando login com Google...
 Processando resposta da autenticação...
 Autenticação bem-sucedida!
 Usuário: [Seu Nome]
 Email: luisrobertolrm@gmail.com
```

## � Links Úteis

- [Web Client ID](https://console.cloud.google.com/apis/credentials/oauthclient/849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com?project=socialx-9be3f)
- [OAuth Consent Screen](https://console.cloud.google.com/apis/credentials/consent?project=socialx-9be3f)
- [Expo Auth Session Docs](https://docs.expo.dev/versions/latest/sdk/auth-session/)
