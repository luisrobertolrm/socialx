# SocialX - Plataforma Social de Grupos, Eventos e Posts

Mono repo contendo o backend .NET e o frontend React Native para a plataforma SocialX.

## Estrutura do Projeto

```
SocialX/
├── src/                          # Backend .NET
│   ├── SocialX.Core/            # Entidades, interfaces e contratos
│   ├── SocialX.Infra/           # DbContext, repositórios e UnitOfWork
│   ├── SocialX.Service/         # DTOs, serviços e validadores
│   └── SocialX.Api/             # Controllers e configuração da API
├── tests/                        # Testes do backend
│   ├── SocialX.Service.Tests/
│   └── SocialX.Api.Tests/
├── frontend/                     # Frontend React Native
│   ├── src/
│   │   ├── components/          # Componentes React
│   │   ├── services/            # Serviços de API
│   │   └── types/               # Tipos TypeScript
│   └── App.tsx
├── scripts/                      # Scripts auxiliares
└── SocialX.sln                  # Solução .NET
```

## Tecnologias

### Backend
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- AutoMapper 13.0.1
- FluentValidation 12.1.1
- xUnit

### Frontend
- React Native (Expo)
- TypeScript (strict mode)
- Axios
- ESLint
- Prettier

## Configuração e Execução

### Backend

1. Configurar a string de conexão no `src/SocialX.Api/appsettings.json`
2. Executar as migrations (quando criadas)
3. Compilar e executar:

```bash
dotnet build
dotnet run --project src/SocialX.Api
```

### Frontend

1. Instalar dependências:

```bash
cd frontend
npm install
```

2. Executar o projeto:

```bash
npm start
```

## Validação

### Backend

```bash
dotnet build
dotnet test
```

### Frontend

```bash
cd frontend
npm run lint
npm run typecheck
npm test
```

## Exemplo Implementado

O projeto inclui um exemplo completo de CRUD com a entidade `EntidadeTeste`:

- Backend: Controller, Service, Repository, DTOs, Validators
- Frontend: Componente de teste que consome a API
- Paginação infinita por cursor
- Tratamento global de exceções
- Validação com FluentValidation
- Mapeamento com AutoMapper

## Próximos Passos

1. Criar migrations do Entity Framework
2. Implementar os casos de uso da plataforma social
3. Adicionar testes unitários e de integração
4. Configurar CI/CD
5. Implementar autenticação e autorização
