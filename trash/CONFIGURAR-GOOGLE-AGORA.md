#  AÇÃO NECESSÁRIA: Configurar Redirect URI

##  O Que Você Precisa Fazer AGORA

Seu app Expo está rodando como `@luisrobertolrm/social-x-native`, então você precisa adicionar o redirect URI no Google Cloud Console.

## � Passo a Passo (5 minutos)

### 1⃣ Acessar Google Cloud Console
� https://console.cloud.google.com/

### 2⃣ Selecionar o Projeto
- Clique no seletor de projetos (topo da página)
- Selecione: **socialx-9be3f**

### 3⃣ Ir para Credentials
- Menu lateral: **APIs & Services**
- Clique em: **Credentials**

### 4⃣ Editar Web Client ID
- Procure por: `849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com`
- Clique no nome para editar

### 5⃣ Adicionar Redirect URI
Na seção **Authorized redirect URIs**, clique em **+ ADD URI** e adicione:

```
https://auth.expo.io/@luisrobertolrm/social-x-native
```

### 6⃣ Salvar
- Clique em **SAVE** no final da página
- Aguarde 1-2 minutos para as mudanças propagarem

##  Testar

Após salvar no Google Cloud Console:

```bash
cd frontend
npm start
```

No app:
1. Clique em " Entrar com Google"
2. Selecione uma conta
3. Autorize o app
4.  Deve funcionar!

##  Como Saber se Funcionou

###  Sucesso
```
 Iniciando login com Google...
 Processando resposta da autenticação...
 Autenticação bem-sucedida!
 Usuário: [Seu Nome]
```

###  Erro (redirect_uri_mismatch)
```
 ERRO: redirect_uri_mismatch
```
**Solução**: Verifique se adicionou o URI correto no Google Cloud Console.

##  Informações do Seu App

- **Owner**: `@luisrobertolrm`
- **Slug**: `social-x-native`
- **Package**: `com.example.socialx`
- **Redirect URI**: `https://auth.expo.io/@luisrobertolrm/social-x-native`

##  Precisa de Ajuda?

Leia os documentos detalhados:
- `frontend/GOOGLE-REDIRECT-URI.md` - Instruções completas
- `frontend/GOOGLE-AUTH-SETUP.md` - Setup completo
- `GOOGLE-AUTH-RESUMO.md` - Visão geral

##  Depois de Configurar

Você terá:
-  Login com Google funcionando
-  Avatar do usuário
-  Nome e email
-  ID do Google
-  Botão de logout

Tudo pronto para integrar com o backend!
