# Google Sign-In Nativo para React Native

##  Por Que Migrar?

Migramos de `expo-auth-session` para `@react-native-google-signin/google-signin` porque:

 **Mais Confiável**: Usa APIs nativas do Google
 **Sem Redirect URI**: Não precisa configurar redirect URIs complexos
 **Funciona Offline**: Pode fazer login silencioso
 **Mais Rápido**: Integração direta com Google Play Services
 **Melhor UX**: Usa o seletor de conta nativo do Android

##  Instalação

### Opção 1: Script Automático (Recomendado)
```powershell
.\scripts\setup-google-signin-native.ps1
```

### Opção 2: Manual
```bash
cd frontend

# Instalar dependência
npm install @react-native-google-signin/google-signin

# Executar prebuild para configurar módulos nativos
npx expo prebuild --clean
```

##  Configuração do Google Cloud Console

### O Que Você Precisa

Apenas o **Android Client ID** (tipo 1)! Não precisa mais de redirect URIs.

### Passo a Passo

1. **Acessar Google Cloud Console**
   - Link: https://console.cloud.google.com/apis/credentials?project=socialx-9be3f

2. **Verificar Android Client ID**
   - Deve existir um Client ID com:
     - **Package name**: `com.example.socialx`
     - **SHA-1**: `78df6ad2a31534c5beac4cce32b6188082a7a998`

3. **Se Não Existir, Criar Novo**
   - Clique em **+ CREATE CREDENTIALS > OAuth client ID**
   - Tipo: **Android**
   - Package name: `com.example.socialx`
   - SHA-1: `78df6ad2a31534c5beac4cce32b6188082a7a998`
   - Clique em **CREATE**

4. **Adicionar Usuário de Teste**
   - Link: https://console.cloud.google.com/apis/credentials/consent?project=socialx-9be3f
   - Role até **Test users**
   - Adicione: `luisrobertolrm@gmail.com`

##  Obter SHA-1 Fingerprint

### Para Desenvolvimento (Debug)

O SHA-1 já está configurado: `78df6ad2a31534c5beac4cce32b6188082a7a998`

Este é o SHA-1 padrão do Expo para desenvolvimento.

### Para Produção (Release)

Quando fizer build de produção, você precisará gerar um novo SHA-1:

```bash
cd android
./gradlew signingReport
```

Ou com Expo:
```bash
eas credentials
```

##  Arquivos Criados

### Services
- `src/services/googleSignInService.ts` - Serviço de autenticação nativo

### Components
- `src/components/GoogleSignInTestScreen.tsx` - Tela de teste

### Scripts
- `scripts/setup-google-signin-native.ps1` - Script de instalação

##  Como Usar

### 1. Executar Prebuild
```bash
cd frontend
npx expo prebuild --clean
```

### 2. Iniciar o App
```bash
npm start
```

### 3. Abrir no Emulador Android
Pressione `a` no terminal do Metro

### 4. Testar Login
- Clique em " Entrar com Google"
- Selecione uma conta
-  Deve funcionar!

##  Funcionalidades

### Tela de Login
- Botão "Entrar com Google"
- Informações sobre o método nativo
- Loading durante autenticação

### Após Login
- Avatar do usuário
- Nome completo
- Email
- ID do Google
- Botão "Sair"

### Recursos Avançados
- Login silencioso (mantém sessão)
- Verificação automática de login ao abrir app
- Tratamento de erros detalhado

##  Logs de Debug

O serviço gera logs detalhados:

```
 Configurando Google Sign-In...
 Google Sign-In configurado
� Web Client ID: [ID]

 Iniciando Google Sign-In...
 Google Play Services disponível
 Login bem-sucedido!
 Usuário: [Nome]
 Email: [Email]
 Tokens obtidos
```

##  Diferenças do expo-auth-session

| Recurso | expo-auth-session | google-signin (nativo) |
|---------|-------------------|------------------------|
| Redirect URI |  Necessário |  Não necessário |
| Prebuild |  Não necessário |  Necessário |
| Expo Go |  Funciona |  Não funciona |
| Build Standalone |  Funciona |  Funciona |
| Login Silencioso |  Não |  Sim |
| Offline |  Não |  Sim |

##  Troubleshooting

### Erro: "DEVELOPER_ERROR"
**Causa**: SHA-1 ou package name incorreto.

**Solução**:
1. Verifique se o package name é `com.example.socialx`
2. Verifique se o SHA-1 está correto no Google Cloud Console
3. Aguarde 2-3 minutos após criar/atualizar o Client ID

### Erro: "SIGN_IN_REQUIRED"
**Causa**: Usuário não está logado.

**Solução**: Normal, apenas faça login novamente.

### Erro: "PLAY_SERVICES_NOT_AVAILABLE"
**Causa**: Google Play Services não instalado no emulador.

**Solução**:
1. Use um emulador com Google Play (não AOSP)
2. Ou use um dispositivo físico

### Erro: Module not found
**Causa**: Prebuild não foi executado.

**Solução**:
```bash
cd frontend
npx expo prebuild --clean
```

## � Checklist de Configuração

- [ ] Dependência instalada: `@react-native-google-signin/google-signin`
- [ ] Prebuild executado: `npx expo prebuild --clean`
- [ ] Android Client ID criado no Google Cloud Console
- [ ] Package name: `com.example.socialx`
- [ ] SHA-1: `78df6ad2a31534c5beac4cce32b6188082a7a998`
- [ ] Usuário de teste adicionado: `luisrobertolrm@gmail.com`
- [ ] App testado no emulador Android

##  Vantagens

### Simplicidade
- Sem configuração de redirect URIs
- Sem problemas com proxy do Expo
- Configuração mais simples

### Performance
- Login mais rápido
- Usa APIs nativas
- Melhor integração com o sistema

### Confiabilidade
- Menos erros de configuração
- Funciona offline
- Mantém sessão automaticamente

## � Referências

- [Documentação Oficial](https://github.com/react-native-google-signin/google-signin)
- [Google Cloud Console](https://console.cloud.google.com/)
- [Expo Prebuild](https://docs.expo.dev/workflow/prebuild/)
