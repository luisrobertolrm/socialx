# Configurar Redirect URI no Google Cloud Console

## URI de Redirecionamento Atual

Seu app Expo está rodando com:
- **Owner**: `@luisrobertolrm`
- **Slug**: `social-x-native`
- **Redirect URI**: `https://auth.expo.io/@luisrobertolrm/social-x-native`

## Passo a Passo para Configurar

### 1. Acessar Google Cloud Console

1. Acesse: https://console.cloud.google.com/
2. Selecione o projeto: **socialx-9be3f**
3. No menu lateral, vá em: **APIs & Services > Credentials**

### 2. Configurar Web Client ID

1. Encontre o **Web Client ID** (tipo 3):
   ```
   849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com
   ```

2. Clique no nome do Client ID para editar

3. Na seção **Authorized redirect URIs**, adicione:
   ```
   https://auth.expo.io/@luisrobertolrm/social-x-native
   ```

4. Clique em **SAVE**

### 3. Verificar Android Client ID

O Android Client ID já está configurado corretamente:
- **Client ID**: `849131252690-tmg67pl23vr7n8ggn1b87301f8klurmu.apps.googleusercontent.com`
- **Package**: `com.example.socialx`
- **SHA-1**: `78df6ad2a31534c5beac4cce32b6188082a7a998`

## Testar Após Configuração

### 1. Aguardar Propagação
Aguarde 1-2 minutos após salvar as mudanças no Google Cloud Console.

### 2. Reiniciar o App
```bash
cd frontend
npm start
```
Pressione `r` no terminal do Metro para recarregar o app.

### 3. Testar Login
1. Abra o app no emulador Android
2. Clique em " Entrar com Google"
3. Selecione uma conta Google
4. Autorize o app

## Redirect URIs Necessários

Para diferentes ambientes, você pode precisar adicionar:

### Desenvolvimento (Expo Go)
```
https://auth.expo.io/@luisrobertolrm/social-x-native
```

### Standalone Build (Android)
```
com.example.socialx:/oauth2redirect/google
```

### Standalone Build (iOS)
```
com.example.socialx:/oauth2redirect/google
```

### Web (se usar)
```
http://localhost:19006
https://seu-dominio.com
```

## Troubleshooting

### Erro: "redirect_uri_mismatch"
**Causa**: O redirect URI não está autorizado no Google Cloud Console.

**Solução**:
1. Verifique o redirect URI exato nos logs do console
2. Adicione esse URI exato no Google Cloud Console
3. Aguarde 1-2 minutos
4. Tente novamente

### Erro: "invalid_client"
**Causa**: Client ID incorreto ou não configurado.

**Solução**:
1. Verifique se o Web Client ID está correto em `src/config/googleAuth.ts`
2. Verifique se o Android Client ID está correto
3. Reconstrua o app se necessário

### Como Ver o Redirect URI Usado

Adicione este log no `GoogleAuthService`:
```typescript
console.log('Redirect URI:', AuthSession.makeRedirectUri());
```

## Configuração Atual do App

### app.json
```json
{
  "expo": {
    "name": "SocialX",
    "slug": "social-x-native",
    "owner": "luisrobertolrm",
    "scheme": "socialx"
  }
}
```

### Client IDs
- **Android**: `849131252690-tmg67pl23vr7n8ggn1b87301f8klurmu.apps.googleusercontent.com`
- **Web**: `849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com`

## Próximos Passos

1.  Adicionar redirect URI no Google Cloud Console
2.  Aguardar propagação (1-2 minutos)
3.  Reiniciar o app
4.  Testar login com Google
5.  Verificar logs no console

## Referências

- [Expo Auth Session - Redirect URI](https://docs.expo.dev/versions/latest/sdk/auth-session/#redirecturi)
- [Google OAuth 2.0 - Redirect URIs](https://developers.google.com/identity/protocols/oauth2/native-app#redirect-uri_loopback)
