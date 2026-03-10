# Como Rodar os Projetos no VS Code

## Pré-requisitos

1. **.NET 10 SDK** - Já instalado 
2. **Node.js** - Já instalado 
3. **PostgreSQL** - Precisa estar rodando
4. **VS Code** com extensões:
   - C# Dev Kit
   - React Native Tools (opcional)

## Configuração Inicial

### 1. Configurar o Banco de Dados PostgreSQL

Certifique-se que o PostgreSQL está rodando e crie o banco:

```sql
CREATE DATABASE socialx;
```

### 2. Atualizar a Connection String

Edite o arquivo `src/SocialX.Api/appsettings.json` com suas credenciais do PostgreSQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=socialx;Username=SEU_USUARIO;Password=SUA_SENHA"
  }
}
```

### 3. Criar as Migrations (Primeira vez apenas)

```bash
dotnet ef migrations add InitialCreate --project src/SocialX.Infra --startup-project src/SocialX.Api
dotnet ef database update --project src/SocialX.Infra --startup-project src/SocialX.Api
```

## Rodando o Backend (.NET API)

### Opção 1: Via Terminal Integrado do VS Code

1. Abra o terminal integrado (Ctrl + `)
2. Execute:

```bash
dotnet run --project src/SocialX.Api
```

A API estará disponível em:
- HTTPS: `https://localhost:7000`
- HTTP: `http://localhost:5000`
- Swagger: `https://localhost:7000/swagger`

### Opção 2: Via Debug do VS Code (F5)

1. Pressione `F5` ou vá em `Run > Start Debugging`
2. Selecione `.NET Core` se solicitado
3. A API iniciará em modo debug

### Opção 3: Via Kiro Background Process

Use o comando do Kiro para iniciar em background:
- Isso permite que a API rode enquanto você trabalha em outras coisas

## Rodando o Frontend (React Native)

### Opção 1: Via Terminal

1. Abra um novo terminal (Ctrl + Shift + `)
2. Navegue até a pasta frontend:

```bash
cd frontend
```

3. Inicie o Expo:

```bash
npm start
```

4. Escolha como visualizar:
   - Pressione `w` para abrir no navegador web
   - Pressione `a` para Android (requer emulador ou dispositivo)
   - Pressione `i` para iOS (requer macOS)
   - Escaneie o QR Code com o app Expo Go no celular

### Opção 2: Direto no navegador

```bash
cd frontend
npm run web
```

## Testando a Integração

1. Com o backend rodando em `https://localhost:7000`
2. Com o frontend rodando no Expo
3. O componente `EntidadeTesteComponent` tentará buscar dados da API

**Nota:** Se estiver testando no celular via Expo Go, você precisará:
- Atualizar a URL da API em `frontend/src/services/api.ts`
- Trocar `localhost` pelo IP da sua máquina na rede local
- Exemplo: `baseURL: 'https://192.168.1.100:7000/api'`

## Comandos Úteis

### Backend

```bash
# Compilar
dotnet build

# Rodar testes
dotnet test

# Limpar build
dotnet clean

# Restaurar pacotes
dotnet restore

# Ver logs detalhados
dotnet run --project src/SocialX.Api --verbosity detailed
```

### Frontend

```bash
cd frontend

# Instalar dependências
npm install

# Rodar lint
npm run lint

# Corrigir lint automaticamente
npm run lint:fix

# Verificar tipos TypeScript
npm run typecheck

# Rodar testes
npm test

# Limpar cache do Expo
npx expo start --clear
```

## Estrutura de Pastas para Desenvolvimento

```
SocialX/
├── src/SocialX.Api/          # Backend - Rode aqui
├── frontend/                  # Frontend - Rode aqui
├── scripts/                   # Scripts auxiliares
└── tests/                     # Testes
```

## Troubleshooting

### Backend não inicia

1. Verifique se o PostgreSQL está rodando
2. Verifique a connection string
3. Verifique se a porta 7000 está livre
4. Execute `dotnet clean` e `dotnet build` novamente

### Frontend não conecta na API

1. Verifique se o backend está rodando
2. Verifique a URL em `frontend/src/services/api.ts`
3. Se estiver no celular, use o IP da máquina, não `localhost`
4. Verifique se o firewall não está bloqueando

### Erro de certificado HTTPS

No desenvolvimento, você pode desabilitar a verificação SSL (apenas para testes):

```typescript
// frontend/src/services/api.ts
import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7000/api',
  headers: {
    'Content-Type': 'application/json',
  },
  // Apenas para desenvolvimento local
  httpsAgent: new (require('https').Agent)({
    rejectUnauthorized: false
  })
});
```

## Próximos Passos

1. Acesse o Swagger em `https://localhost:7000/swagger` para testar a API
2. Teste o endpoint `GET /api/EntidadeTeste/1`
3. No frontend, clique no botão "Buscar Entidade" para testar a integração
4. Comece a implementar os casos de uso conforme o README.md
