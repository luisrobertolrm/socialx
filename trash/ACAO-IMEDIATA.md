# ACAO IMEDIATA - Corrigir DEVELOPER_ERROR

## PROBLEMA IDENTIFICADO

O SHA-1 no Google Cloud Console esta DIFERENTE do SHA-1 do seu certificado debug!

### SHA-1 Atual (seu certificado):
```
5E:8F:16:06:2E:A3:CD:2C:4A:0D:54:78:76:BA:A6:F3:8C:AB:F6:25
```

### SHA-1 no google-services.json (antigo):
```
78:DF:6A:D2:A3:15:34:C5:BE:AC:4C:CE:32:B6:18:80:82:A7:A9:98
```

## SOLUCAO

### 1. Acesse o Google Cloud Console

URL: https://console.cloud.google.com/apis/credentials?project=socialx-9be3f

### 2. Encontre a credencial Android

Procure por:
- Nome: Cliente Android 1
- ID: 849131252690-tmg67pl23vr7n8ggn1b87301f8klurmu.apps.googleusercontent.com

### 3. Edite a credencial

Clique no icone de lapis (editar)

### 4. Atualize o SHA-1

SUBSTITUA o SHA-1 antigo por:
```
5E:8F:16:06:2E:A3:CD:2C:4A:0D:54:78:76:BA:A6:F3:8C:AB:F6:25
```

Verifique tambem:
- Package name: com.example.socialx

### 5. Salve

Clique em SALVAR

### 6. Aguarde

Aguarde 5-10 minutos para as alteracoes propagarem

### 7. Teste

```powershell
cd frontend
npx expo run:android
```

## VERIFICACAO

Quando o app abrir, clique em "Login com Google" e verifique os logs:

### Sucesso:
```
Iniciando Google Sign-In...
Google Play Services disponivel
Login bem-sucedido!
Usuario: [seu nome]
Email: [seu email]
Tokens obtidos
```

### Ainda com erro:
```
Erro no Google Sign-In: Error: DEVELOPER_ERROR
Codigo do erro: 10
```

Se ainda der erro:
1. Confirme que aguardou 5-10 minutos
2. Confirme que o SHA-1 foi salvo corretamente
3. Tente fazer logout da conta Google no dispositivo e login novamente

## IMPORTANTE

O SHA-1 JA FOI COPIADO para a area de transferencia!

Basta:
1. Abrir o Google Cloud Console
2. Editar a credencial Android
3. Colar (Ctrl+V) no campo SHA-1
4. Salvar
5. Aguardar 5-10 minutos
6. Testar

## PROXIMOS PASSOS

Apos o login funcionar:
1. Integrar com backend
2. Salvar tokens
3. Implementar refresh
4. Testar em dispositivo fisico
