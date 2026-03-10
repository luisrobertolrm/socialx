# Redirect URI Correto para Expo

##  URI Correto para Expo Go

Com `useProxy: true`, o Expo gera automaticamente um URI válido:

```
https://auth.expo.io/@luisrobertolrm/social-x-native
```

Este é um domínio válido (.io) que o Google aceita!

## � Como Adicionar no Google Cloud Console

### Passo 1: Acessar Credentials
1. Acesse: https://console.cloud.google.com/apis/credentials?project=socialx-9be3f
2. Procure pelo **Web Client ID**:
   ```
   849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com
   ```
3. Clique no nome para editar

### Passo 2: Adicionar o Redirect URI
1. Role até **Authorized redirect URIs**
2. Clique em **+ ADD URI**
3. Cole exatamente:
   ```
   https://auth.expo.io/@luisrobertolrm/social-x-native
   ```
4. Clique em **SAVE**

### Passo 3: Adicionar Usuário de Teste
1. Vá para: https://console.cloud.google.com/apis/credentials/consent?project=socialx-9be3f
2. Role até **Test users**
3. Clique em **+ ADD USERS**
4. Adicione: `luisrobertolrm@gmail.com`
5. Clique em **SAVE**

##  Verificar o URI no App

Após recarregar o app, você verá:

### No Console:
```
========================================
� REDIRECT URI USADO PELO APP:
https://auth.expo.io/@luisrobertolrm/social-x-native
========================================
```

### Na Tela do App:
Uma caixa amarela mostrando o URI completo.

##  Importante

### Para Expo Go (Desenvolvimento)
- Use `useProxy: true`
- URI: `https://auth.expo.io/@owner/slug`
-  Aceito pelo Google

### Para Standalone Build (Produção)
- Use `useProxy: false`
- URI: `com.example.socialx:/oauth2redirect/google`
- Precisa configurar deep linking

## � Testar

Após adicionar o URI no Google Cloud Console:

1. **Aguarde 1-2 minutos** para propagação
2. Recarregue o app (pressione `r` no Metro)
3. Clique em " Entrar com Google"
4. Selecione sua conta
5.  Deve funcionar!

##  Checklist Completo

- [ ] Redirect URI adicionado: `https://auth.expo.io/@luisrobertolrm/social-x-native`
- [ ] Email adicionado como usuário de teste: `luisrobertolrm@gmail.com`
- [ ] OAuth consent screen configurada
- [ ] Aguardou 1-2 minutos após salvar
- [ ] App recarregado

##  Resultado Esperado

Após configurar corretamente:

```
 Iniciando login com Google...
 Processando resposta da autenticação...
 Autenticação bem-sucedida!
 Usuário: [Seu Nome]
 Email: luisrobertolrm@gmail.com
```

##  Se Ainda Não Funcionar

### Erro: "redirect_uri_mismatch"
- Verifique se o URI está EXATAMENTE igual (sem espaços, sem barra no final)
- Aguarde mais 1-2 minutos

### Erro: "Access blocked"
- Adicione seu email como usuário de teste
- Configure a OAuth consent screen

### Erro: "invalid_client"
- Verifique se está usando o Web Client ID correto
- Verifique se o projeto está correto (socialx-9be3f)

## � Links Diretos

- [Credentials](https://console.cloud.google.com/apis/credentials?project=socialx-9be3f)
- [OAuth Consent Screen](https://console.cloud.google.com/apis/credentials/consent?project=socialx-9be3f)
- [Web Client ID](https://console.cloud.google.com/apis/credentials/oauthclient/849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com?project=socialx-9be3f)
