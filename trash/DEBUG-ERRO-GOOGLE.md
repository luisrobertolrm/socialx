# Debug do Erro de Autenticação Google

##  Como Ver o Erro Exato

Atualizei o código para mostrar logs detalhados. Agora quando você tentar fazer login, verá no console:

```
 Resposta completa recebida: {...}
 Processando resposta da autenticação...
� Tipo de resposta: [success/error/cancel]
 Erro detectado: {...}
 Params: {...}
```

## � Erros Comuns e Soluções

### 1. redirect_uri_mismatch
**Mensagem**: "The redirect URI in the request does not match..."

**Causa**: O redirect URI não está autorizado.

**Solução**:
- Verifique se adicionou EXATAMENTE: `https://auth.expo.io/@luisrobertolrm/social-x-native`
- Sem espaços, sem barra no final
- Aguarde 2-3 minutos após salvar

### 2. access_denied
**Mensagem**: "Access blocked: Authorization Error"

**Causa**: Email não está na lista de usuários de teste OU app não está publicado.

**Solução**:
- Verifique se `luisrobertolrm@gmail.com` está em Test users
- OU publique o app (OAuth consent screen > PUBLISH APP)

### 3. invalid_client
**Mensagem**: "The OAuth client was not found"

**Causa**: Client ID incorreto ou projeto errado.

**Solução**:
- Verifique se está usando o Web Client ID correto
- Verifique se está no projeto correto (socialx-9be3f)

### 4. origin_mismatch
**Mensagem**: "Origin mismatch"

**Causa**: JavaScript origin não autorizado.

**Solução**:
- Verifique se tem `https://auth.expo.io` em Authorized JavaScript origins

### 5. idpiframe_initialization_failed
**Mensagem**: "Cookies are not enabled in current environment"

**Causa**: Problema com cookies no navegador do emulador.

**Solução**:
- Limpe o cache do navegador no emulador
- Reinicie o emulador

## � Passos para Debug

### 1. Recarregar o App
```bash
# No terminal do Metro, pressione:
r
```

### 2. Tentar Login
- Clique em " Entrar com Google"
- Observe o console

### 3. Copiar o Erro Completo
Procure por:
```
 Erro detectado: {...}
 Params: {...}
```

### 4. Verificar Configuração

#### Checklist:
- [ ] Authorized JavaScript origins: `https://auth.expo.io`
- [ ] Authorized redirect URIs: `https://auth.expo.io/@luisrobertolrm/social-x-native`
- [ ] Test users: `luisrobertolrm@gmail.com`
- [ ] OAuth consent screen configurada
- [ ] Aguardou 2-3 minutos após salvar

##  Verificar Configuração Atual

### Web Client ID
Link: https://console.cloud.google.com/apis/credentials/oauthclient/849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com?project=socialx-9be3f

Deve ter:
```
Authorized JavaScript origins:
  https://auth.expo.io

Authorized redirect URIs:
  https://auth.expo.io/@luisrobertolrm/social-x-native
```

### OAuth Consent Screen
Link: https://console.cloud.google.com/apis/credentials/consent?project=socialx-9be3f

Deve ter:
```
Publishing status: Testing
Test users: luisrobertolrm@gmail.com
```

##  Teste Alternativo

Se continuar com erro, tente publicar o app:

1. Acesse: https://console.cloud.google.com/apis/credentials/consent?project=socialx-9be3f
2. Clique em **PUBLISH APP**
3. Confirme a publicação
4. Aguarde 2-3 minutos
5. Teste novamente

 **Nota**: Apps publicados podem ser usados por qualquer pessoa, não só usuários de teste.

##  Próximos Passos

1. Recarregue o app
2. Tente fazer login
3. Copie o erro completo do console
4. Me envie o erro para análise

## � Links Úteis

- [Web Client ID](https://console.cloud.google.com/apis/credentials/oauthclient/849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com?project=socialx-9be3f)
- [OAuth Consent Screen](https://console.cloud.google.com/apis/credentials/consent?project=socialx-9be3f)
- [Google OAuth Errors](https://developers.google.com/identity/protocols/oauth2/web-server#error-codes)
