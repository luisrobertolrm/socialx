# Como Pegar o Redirect URI

##  Método 1: Ver na Tela do App (MAIS FÁCIL)

Atualizei o componente para mostrar o redirect URI diretamente na tela!

1. Recarregue o app (pressione `r` no Metro)
2. Role a tela até a seção amarela com o título "� REDIRECT URI:"
3. O URI estará visível e você pode selecioná-lo
4. Copie o URI completo

##  Método 2: Ver no Console

1. Abra o console do Metro (terminal onde rodou `npm start`)
2. Procure por:
   ```
   ========================================
   � REDIRECT URI USADO PELO APP:
   [URI AQUI]
   ========================================
   ```
3. Copie o URI exato

## � URIs Possíveis

Dependendo do ambiente, o URI pode ser:

### Expo Go (Desenvolvimento)
```
https://auth.expo.io/@luisrobertolrm/social-x-native
```

### Custom Scheme
```
socialx://redirect
```

### Expo Development Client
```
exp://[IP]:8081/--/redirect
```

##  Adicionar no Google Cloud Console

Depois de pegar o URI:

1. Acesse: https://console.cloud.google.com/apis/credentials?project=socialx-9be3f
2. Clique no **Web Client ID**: `849131252690-f59f7p3f5el0egjesad4asq0thi0vbie`
3. Role até **Authorized redirect URIs**
4. Clique em **+ ADD URI**
5. Cole o URI exato que você copiou
6. Clique em **SAVE**
7. Aguarde 1-2 minutos

##  Testar Novamente

1. Aguarde 1-2 minutos após salvar
2. Recarregue o app (pressione `r`)
3. Clique em " Entrar com Google"
4. Deve funcionar!

##  Dica

Adicione TODOS esses URIs para cobrir diferentes cenários:

```
https://auth.expo.io/@luisrobertolrm/social-x-native
socialx://redirect
exp://localhost:8081/--/redirect
```

Assim funciona em qualquer ambiente!
