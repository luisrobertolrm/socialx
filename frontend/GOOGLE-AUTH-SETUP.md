# Configuração de Autenticação Google

## Pacotes Necessários

Execute o comando abaixo para instalar as dependências:

```bash
cd frontend
npm install expo-auth-session expo-web-browser
```

## Arquivos Criados

### 1. Configuração
- `google-services.json` - Configuração do Firebase/Google Cloud
- `src/config/googleAuth.ts` - Constantes de configuração

### 2. Types
- `src/types/AuthTypes.ts` - Interfaces TypeScript para autenticação

### 3. Services
- `src/services/googleAuthService.ts` - Lógica de autenticação

### 4. Components
- `src/components/GoogleAuthTestScreen.tsx` - Tela de teste completa

## Configuração do Google Cloud Console

### Client IDs Configurados

1. **Android Client ID** (tipo 1):
   ```
   849131252690-tmg67pl23vr7n8ggn1b87301f8klurmu.apps.googleusercontent.com
   ```
   - Package: `com.example.socialx`
   - SHA-1: `78df6ad2a31534c5beac4cce32b6188082a7a998`

2. **Web Client ID** (tipo 3):
   ```
   849131252690-f59f7p3f5el0egjesad4asq0thi0vbie.apps.googleusercontent.com
   ```
   - Usado para iOS e Expo Go

###  IMPORTANTE: Configurar Redirect URI

**VOCÊ PRECISA ADICIONAR O REDIRECT URI NO GOOGLE CLOUD CONSOLE!**

1. Acesse: https://console.cloud.google.com/
2. Selecione o projeto: `socialx-9be3f`
3. Vá em: **APIs & Services > Credentials**
4. Clique no **Web Client ID** para editar
5. Em **Authorized redirect URIs**, adicione:
   ```
   https://auth.expo.io/@luisrobertolrm/social-x-native
   ```
6. Clique em **SAVE**
7. Aguarde 1-2 minutos para propagação

� **Leia**: `GOOGLE-REDIRECT-URI.md` para instruções detalhadas

## Como Testar

### 1. Instalar Dependências
```bash
cd frontend
npm install expo-auth-session expo-web-browser
```

### 2. Iniciar o App
```bash
npm start
```

### 3. Testar no Emulador Android
- Pressione `a` para abrir no Android
- A tela de autenticação Google aparecerá no topo
- Clique em " Entrar com Google"
- Selecione uma conta Google
- Autorize o app

### 4. Verificar Logs
Os logs detalhados aparecem no console:
```
 Iniciando login com Google...
 Processando resposta da autenticação...
 Autenticação bem-sucedida!
 Usuário: [Nome]
 Email: [Email]
```

## Estrutura da Tela de Teste

### Estado Inicial (Não Autenticado)
- Título: " Teste de Autenticação Google"
- Botão: " Entrar com Google"
- Informações de configuração (Project ID, Package, Status)

### Durante Autenticação
- Loading spinner
- Texto: "Autenticando..."

### Após Autenticação Bem-Sucedida
- Avatar do usuário (foto do Google)
- Nome completo
- Email
- Detalhes: ID, Nome, Sobrenome
- Botão: " Sair"

### Em Caso de Erro
- Mensagem de erro em vermelho
- Detalhes do erro no console

## Troubleshooting

### Erro: "Request não está pronto"
- Aguarde alguns segundos após abrir o app
- O hook `useAuthRequest` precisa inicializar

### Erro: "Invalid client"
- Verifique se o package name está correto: `com.example.socialx`
- Verifique se o SHA-1 está registrado no Console
- Reconstrua o app: `expo prebuild --clean`

### Erro: "Network Error"
- Verifique sua conexão com a internet
- Tente novamente após alguns segundos

### SHA-1 Não Corresponde
Para gerar um novo SHA-1 para desenvolvimento:
```bash
cd android
./gradlew signingReport
```

Para Expo:
```bash
expo credentials:manager
```

## Próximos Passos

### 1. Integrar com Backend
Envie o `idToken` para o backend validar:
```typescript
const response = await api.post('/auth/google', {
  idToken: result.idToken,
});
```

### 2. Persistir Sessão
Use AsyncStorage para manter o usuário logado:
```typescript
import AsyncStorage from '@react-native-async-storage/async-storage';

await AsyncStorage.setItem('user', JSON.stringify(user));
```

### 3. Criar Context de Autenticação
Crie um `AuthContext` para compartilhar o estado de autenticação:
```typescript
export const AuthContext = React.createContext<AuthState>({...});
```

## Referências

- [Expo Auth Session](https://docs.expo.dev/versions/latest/sdk/auth-session/)
- [Google Sign-In](https://developers.google.com/identity/sign-in/android/start)
- [Firebase Console](https://console.firebase.google.com/)
