# Configuração Completa do Google OAuth

##  Configurações Necessárias

Para o Google OAuth funcionar com Expo, você precisa configurar DUAS coisas no Web Client ID:

### 1. Authorized JavaScript origins (Origens JavaScript autorizadas)
### 2. Authorized redirect URIs (URIs de redirecionamento autorizados)

##  Configuração Correta

### Authorized JavaScript origins
```
https://auth.expo.io
```

 **Está correto!** Mantenha assim.

### Authorized redirect URIs
```
https://auth.expo.io/@luisrobertolrm/social-x-native
```

 **Adicione este URI!**

## � Passo a Passo Completo

### 1. Acessar Web Client ID
Link direto: https://console.cloud.google.com/apis/credentials/oauthclient/849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com?project=socialx-9be3f

### 2. Verificar "Authorized JavaScript origins"
- Deve ter: `https://auth.expo.io`
-  Se já está, não precisa alterar

### 3. Adicionar "Authorized redirect URIs"
- Clique em **+ ADD URI**
- Cole: `https://auth.expo.io/@luisrobertolrm/social-x-native`
- Clique em **SAVE**

### 4. Adicionar Usuário de Teste
Link: https://console.cloud.google.com/apis/credentials/consent?project=socialx-9be3f

- Role até **Test users**
- Clique em **+ ADD USERS**
- Adicione: `luisrobertolrm@gmail.com`
- Clique em **SAVE**

### 5. Configurar OAuth Consent Screen (se necessário)
Link: https://console.cloud.google.com/apis/credentials/consent?project=socialx-9be3f

Preencha:
- **App name**: SocialX
- **User support email**: luisrobertolrm@gmail.com
- **Developer contact information**: luisrobertolrm@gmail.com

Em **Scopes**, adicione:
- `userinfo.email`
- `userinfo.profile`

##  Por Que Cada Configuração é Necessária?

### Authorized JavaScript origins
```
https://auth.expo.io
```

**Função**: Permite que o JavaScript do proxy do Expo faça requisições ao Google OAuth.

**Quando é usado**: Durante o processo de autenticação, o proxy do Expo precisa fazer chamadas JavaScript para o Google.

### Authorized redirect URIs
```
https://auth.expo.io/@luisrobertolrm/social-x-native
```

**Função**: Define para onde o Google deve redirecionar após o login.

**Quando é usado**: Após o usuário fazer login, o Google redireciona para este URI com o código de autorização.

##  Configuração Final

Seu Web Client ID deve ter:

```
Client ID: 849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com

Authorized JavaScript origins:
   https://auth.expo.io

Authorized redirect URIs:
   https://auth.expo.io/@luisrobertolrm/social-x-native
```

## � Testar

Após configurar:

1. **Aguarde 1-2 minutos** para propagação
2. **Recarregue o app** (pressione `r` no Metro)
3. **Clique em " Entrar com Google"**
4. **Selecione sua conta**
5.  **Deve funcionar!**

##  Resultado Esperado

### No Console:
```
� REDIRECT URI CORRETO:
https://auth.expo.io/@luisrobertolrm/social-x-native

 Iniciando login com Google...
 Processando resposta da autenticação...
 Autenticação bem-sucedida!
 Usuário: [Seu Nome]
 Email: luisrobertolrm@gmail.com
```

### Na Tela:
- Avatar do Google
- Nome completo
- Email
- ID do usuário
- Botão "Sair"

##  Troubleshooting

### Erro: "redirect_uri_mismatch"
**Causa**: O redirect URI não está configurado ou está incorreto.

**Solução**:
- Verifique se adicionou: `https://auth.expo.io/@luisrobertolrm/social-x-native`
- Verifique se não tem espaços ou barra no final
- Aguarde 1-2 minutos após salvar

### Erro: "Access blocked: Authorization Error"
**Causa**: Seu email não está na lista de usuários de teste.

**Solução**:
- Adicione `luisrobertolrm@gmail.com` como usuário de teste
- Aguarde 1-2 minutos

### Erro: "origin_mismatch"
**Causa**: A origem JavaScript não está autorizada.

**Solução**:
- Verifique se tem `https://auth.expo.io` em "Authorized JavaScript origins"
- Aguarde 1-2 minutos após salvar

## � Checklist Completo

- [ ] Authorized JavaScript origins: `https://auth.expo.io` 
- [ ] Authorized redirect URIs: `https://auth.expo.io/@luisrobertolrm/social-x-native`
- [ ] Usuário de teste adicionado: `luisrobertolrm@gmail.com`
- [ ] OAuth consent screen configurada
- [ ] Aguardou 1-2 minutos após salvar
- [ ] App recarregado
- [ ] Testou o login

## � Links Úteis

- [Web Client ID (Editar)](https://console.cloud.google.com/apis/credentials/oauthclient/849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com?project=socialx-9be3f)
- [OAuth Consent Screen](https://console.cloud.google.com/apis/credentials/consent?project=socialx-9be3f)
- [Credentials](https://console.cloud.google.com/apis/credentials?project=socialx-9be3f)
- [Expo Auth Session Docs](https://docs.expo.dev/versions/latest/sdk/auth-session/)

##  Dica

Se ainda não funcionar após seguir todos os passos:
1. Aguarde mais 2-3 minutos (propagação pode demorar)
2. Limpe o cache do navegador no emulador
3. Reinicie o app completamente
4. Verifique os logs no console para mensagens de erro específicas
