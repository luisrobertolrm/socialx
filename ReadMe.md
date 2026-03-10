# Instrução inicial obrigatória

Criar um **mono repo** contendo **dois projetos principais**:

- **Frontend React**
- **Backend .NET**

Também deve existir uma pasta **`scripts`** no repositório para centralizar scripts auxiliares, incluindo a geração das classes de Entity.

No projeto **React**, já criar um **componente de teste** consumindo a implementação exposta pelo **`EntidadeTesteController`** do backend para validar a comunicação entre front-end e API desde o início.

# Projeto: Plataforma Social de Grupos, Eventos e Posts

## Visão Geral

Este projeto consiste em uma plataforma social focada em organização de **grupos**, **eventos** e **micro-posts**, semelhante a uma rede social simplificada, voltada para atividades presenciais ou online, como jogos esportivos, música e encontros sociais.

Arquitetura principal:

- **Backend:** .NET 10 (ASP.NET Core Web API)
- **Frontend:** React Native com TypeScript
- **Banco de dados:** PostgreSQL
- **Armazenamento de mídia:** Storage externo, como S3, Supabase Storage ou similar

O desenvolvimento será conduzido com suporte de IA, seguindo regras rígidas de arquitetura, tipagem, testes, compilação e implementação incremental.

---

# Tecnologias do Projeto

## Backend

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- AutoMapper
- FluentValidation
- xUnit


## Componente inicial de teste no React

No projeto React, criar já na implementação inicial um componente de teste para chamar a API do **`EntidadeTesteController`**.

Objetivo desse componente:

- validar a integração entre React e .NET
- testar configuração de base URL, client HTTP e serialização
- servir como referência inicial para os próximos casos de uso

Esse componente pode, por exemplo, executar um **GET** em `api/EntidadeTeste/{id}` e exibir em tela os dados retornados do backend.

## Frontend

- React Native
- TypeScript
- React Navigation
- Jest
- React Native Testing Library

## Qualidade de Código

- ESLint
- Prettier
- TypeScript strict mode
- Husky
- EditorConfig

---

# Processo de Desenvolvimento Orientado por IA

A IA deve seguir obrigatoriamente o fluxo abaixo.

## Regra Fundamental

Sempre criar os testes antes de implementar o código.

Ordem obrigatória:

1. Criar o teste da funcionalidade
2. Criar a estrutura mínima de controller, service ou componente
3. Implementar a lógica mínima para passar no teste
4. Refatorar se necessário
5. Solicitar aprovação para continuar para o próximo caso de uso

## Desenvolvimento Incremental

O desenvolvimento será incremental e controlado.

Regras:

- a IA não pode implementar múltiplos casos de uso automaticamente
- cada funcionalidade deve ser autorizada explicitamente
- cada tarefa deve terminar compilando e com testes passando

Fluxo:

1. Eu solicito um Caso de Uso específico
2. A IA gera os testes
3. A IA gera a estrutura mínima
4. Após autorização explícita, a IA implementa
5. A IA compila e testa
6. A IA encerra a tarefa

Exemplo de pedido válido:

```text
Gerar Caso de Uso: Criar Evento
```

---

# Compilação Obrigatória

Antes de finalizar qualquer tarefa:

## Backend

Executar:

```bash
dotnet build
dotnet test
```

## Frontend

Executar:

```bash
npm run lint
npm run typecheck
npm test
```

A tarefa só pode ser considerada concluída após compilação e testes passando.

---

# Arquitetura do Backend .NET

O backend deve seguir a arquitetura em 4 projetos.

## Estrutura da Solução

```text
SocialX.sln

src/
  SocialX.Core/
    Entidades/
    Enums/
    Interface/
    Paginacao/

  SocialX.Infra/
    Data/
    Configuracoes/
    Repositorio/
    UnitOfWork/
    Paginacao/

  SocialX.Service/
    DTOs/
    Interfaces/
    Mapeamentos/
    Servicos/
    Validadores/

  SocialX.Api/
    Controllers/
    Program.cs
    appsettings.json

tests/
  SocialX.Api.Tests/
  SocialX.Service.Tests/
```

## Responsabilidade de cada projeto

### `SocialX.Core`

Responsável por:

- entidades de domínio
- enums
- interfaces de abstração
- contratos e modelos reutilizáveis de paginação

Não deve depender de `Infra`, `Service` ou `Api`.

### `SocialX.Infra`

Responsável por:

- `DbContext`
- configurações do EF Core com `IEntityTypeConfiguration`
- implementação do repositório genérico
- implementação do `UnitOfWork`
- implementação da paginação infinita que depende de `IQueryable` e `EF Core`

### `SocialX.Service`

Responsável por:

- DTOs
- serviços de aplicação
- AutoMapper Profiles
- FluentValidation Validators
- regras de negócio de aplicação

### `SocialX.Api`

Responsável por:

- controllers
- configuração do container DI
- configuração do pipeline HTTP
- registro de AutoMapper, FluentValidation, DbContext e serviços
- registro de `IExceptionHandler` e `ProblemDetails`

---

# Regras para Código .NET

## Tipagem obrigatória

Nunca usar:

```csharp
var
```

Sempre usar tipo explícito.

Errado:

```csharp
var eventos = new List<Evento>();
```

Correto:

```csharp
List<Evento> eventos = new List<Evento>();
```

## Controllers

Os controllers devem ser finos.

Devem apenas:

- receber a requisição
- chamar o service
- retornar a resposta HTTP adequada

Nunca colocar regra de negócio no controller.

## AutoMapper

Os Profiles do AutoMapper devem ficar no projeto `SocialX.Service`, pois o mapeamento principal será entre DTOs e entidades.

## FluentValidation

Os Validators devem ficar no projeto `SocialX.Service`.

## Tratamento global de exceções

A API deve usar tratamento global de exceções com `IExceptionHandler` + `ProblemDetails`.

Objetivos:

- remover `try/catch` repetitivo de controllers
- padronizar respostas de erro
- separar exceções de domínio de falhas inesperadas
- retornar status HTTP corretos

### Onde colocar cada parte

#### Em `SocialX.Core/Exceptions`
Colocar as exceções de domínio, por exemplo:

- `NotFoundException`
- `BusinessRuleException`

#### Em `SocialX.Api/Handlers`
Colocar o handler global:

- `GlobalExceptionHandler`

#### Em `SocialX.Api/Program.cs`
Registrar:

- `AddExceptionHandler<GlobalExceptionHandler>()`
- `AddProblemDetails()`
- `UseExceptionHandler()`

### Exceções de domínio de referência

```csharp
namespace SocialX.Core.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
```

```csharp
namespace SocialX.Core.Exceptions;

public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message)
    {
    }
}
```

### Implementação de referência do `GlobalExceptionHandler`

```csharp
using SocialX.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SocialX.Api.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails = new ProblemDetails();

        switch (exception)
        {
            case NotFoundException:
                problemDetails.Title = "Recurso não encontrado";
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Detail = exception.Message;
                break;

            case BusinessRuleException:
                problemDetails.Title = "Erro de regra de negócio";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Detail = exception.Message;
                break;

            default:
                logger.LogError(exception, "Erro inesperado");

                problemDetails.Title = "Erro interno do servidor";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Detail = "Ocorreu um erro inesperado.";
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status!.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
```

### Regras obrigatórias para exceptions

- controllers não devem ter `try/catch` para regras normais de aplicação
- services devem lançar exceções de domínio quando necessário
- `NotFoundException` deve gerar `404`
- `BusinessRuleException` deve gerar `400`
- exceções inesperadas devem gerar `500` e ser logadas
- usar `ProblemDetails` como formato padrão de erro

### Exemplo de service lançando exceção

```csharp
using SocialX.Core.Exceptions;
using SocialX.Service.DTOs;
using SocialX.Service.Interfaces;

namespace SocialX.Service.Servicos;

public class EntidadeTesteService : IEntidadeTesteService
{
    public Task<EntidadeTesteDto> ObterPorIdAsync(long id)
    {
        if (id != 1)
        {
            throw new NotFoundException("EntidadeTeste não encontrada.");
        }

        EntidadeTesteDto dto = new EntidadeTesteDto
        {
            Id = 1,
            Nome = "Entidade Teste"
        };

        return Task.FromResult(dto);
    }
}
```

### Exemplo de controller sem `try/catch`

```csharp
using Microsoft.AspNetCore.Mvc;
using SocialX.Service.DTOs;
using SocialX.Service.Interfaces;

namespace SocialX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntidadeTesteController : ControllerBase
{
    private readonly IEntidadeTesteService entidadeTesteService;

    public EntidadeTesteController(IEntidadeTesteService entidadeTesteService)
    {
        this.entidadeTesteService = entidadeTesteService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EntidadeTesteDto>> Get(long id)
    {
        EntidadeTesteDto entidadeTeste = await entidadeTesteService.ObterPorIdAsync(id);

        return Ok(entidadeTeste);
    }
}
```

### Exemplo de resposta de erro

```json
{
  "title": "Recurso não encontrado",
  "status": 404,
  "detail": "EntidadeTeste não encontrada."
}
```

## Repository genérico

Não criar repositórios especializados por entidade por padrão.

Usar:

- `IRepositorioGenerico<T>`
- `RepositorioGenerico<T>`

Registrar no DI desta forma:

```csharp
builder.Services.AddScoped(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
```

Criar repositório especializado apenas se existir necessidade real de consultas ou operações específicas da entidade.

## Unit of Work

A persistência não deve ficar dentro de `Adicionar`, `Editar` ou `Deletar`.

Esses métodos apenas registram a operação no contexto. A gravação no banco deve acontecer apenas no `UnitOfWork`, com `SalvarAsync()`.

Interface:

- `IUnitOfWork` deve ficar em `SocialX.Core`

Implementação:

- `UnitOfWork` deve ficar em `SocialX.Infra`

---

# Contratos do Backend

## Interface `IRepositorioGenerico<T>`

```csharp
public interface IRepositorioGenerico<T> where T : class
{
    T Adicionar(T entidade);
    T Editar(T entidade);
    void Deletar(T entidade);
    IQueryable<T> IQueryable();
}
```

## Interface `IUnitOfWork`

```csharp
public interface IUnitOfWork
{
    Task<bool> SalvarAsync();
}
```

---

# Implementação de referência do Repositório Genérico

```csharp
using SocialX.Core.Interface;
using SocialX.Infra.Data;
using System.Linq;

namespace SocialX.Infra.Repositorio
{
    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
    {
        protected readonly CustomDbContext contexto;

        public RepositorioGenerico(CustomDbContext customDbContext)
        {
            contexto = customDbContext;
        }

        public virtual T Adicionar(T entidade)
        {
            contexto.Set<T>().Add(entidade);
            return entidade;
        }

        public virtual T Editar(T entidade)
        {
            contexto.Set<T>().Update(entidade);
            return entidade;
        }

        public virtual void Deletar(T entidade)
        {
            contexto.Set<T>().Remove(entidade);
        }

        public virtual IQueryable<T> IQueryable()
        {
            return contexto.Set<T>();
        }
    }
}
```

Regras obrigatórias:

- não criar construtor vazio
- não chamar `SaveChanges` dentro do repositório
- não criar repositório especializado sem necessidade real
- usar `contexto.Set<T>()`
- manter a implementação simples e previsível

---

# Implementação de referência do UnitOfWork

```csharp
using SocialX.Core.Interface;
using SocialX.Infra.Data;

namespace SocialX.Infra.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CustomDbContext _context;

        public UnitOfWork(CustomDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SalvarAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
```

---

# Padrão de Paginação Infinita

O projeto deve incluir suporte nativo a paginação infinita baseada em cursor.

## Onde colocar cada parte

### Em `SocialX.Core/Paginacao`

Colocar as classes e contratos independentes de EF Core:

- `TipoOrdem`
- `ParametrosScrollInfinito`
- `ParametrosScrollInfinito<T>`
- `ListaScrollInfinito<T>`
- `CursorHelper`
- `CursorPadrao`

### Em `SocialX.Infra/Paginacao`

Colocar a implementação que depende de `IQueryable`, `Where`, `Take` e `ToListAsync`.

Importante: o método recebido pelo usuário precisa de ajuste para funcionar corretamente com Entity Framework Core.

O trecho abaixo:

```csharp
Func<TEntity, long> getId
```

deve ser alterado para:

```csharp
Expression<Func<TEntity, long>> getId
```

Motivo: `Func<TEntity, long>` não é traduzido para SQL pelo EF Core. Para paginação em banco, a expressão precisa ser traduzível.

## Implementação recomendada em `SocialX.Core/Paginacao`

```csharp
namespace SocialX.Core.Paginacao
{
    public enum TipoOrdem : short
    {
        Asc = 1,
        Desc = 2
    }

    public class ParametrosScrollInfinito
    {
        public int Tamanho { get; set; } = 20;
        public string? OrderBy { get; set; }
        public TipoOrdem Ordem { get; set; } = TipoOrdem.Desc;
        public string? Cursor { get; set; }
    }

    public class ParametrosScrollInfinito<T> : ParametrosScrollInfinito
    {
        public T? Parametros { get; set; }
    }

    [Serializable]
    public class ListaScrollInfinito<T>
    {
        public List<T> Lista { get; set; } = new();
        public string? ProximoCursor { get; set; }
        public bool TemMais { get; set; }
    }

    public class CursorPadrao
    {
        public long Id { get; set; }
    }

    public static class CursorHelper
    {
        public static string Serializar<T>(T obj)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(obj);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return Convert.ToBase64String(bytes);
        }

        public static T? Desserializar<T>(string? cursor)
        {
            if (string.IsNullOrWhiteSpace(cursor))
            {
                return default;
            }

            byte[] bytes = Convert.FromBase64String(cursor);
            string json = System.Text.Encoding.UTF8.GetString(bytes);
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
    }
}
```

## Implementação recomendada em `SocialX.Infra/Paginacao`

```csharp
using SocialX.Core.Paginacao;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SocialX.Infra.Paginacao
{
    public static class ScrollInfinitoExtensions
    {
        public static async Task<ListaScrollInfinito<TEntity>> ScrollAsync<TEntity>(
            this IQueryable<TEntity> query,
            ParametrosScrollInfinito parametros,
            Expression<Func<TEntity, long>> getId,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            CursorPadrao? cursor = CursorHelper.Desserializar<CursorPadrao>(parametros.Cursor);

            if (cursor is not null)
            {
                if (parametros.Ordem == TipoOrdem.Asc)
                {
                    query = query.Where(
                        System.Linq.Expressions.Expression.Lambda<Func<TEntity, bool>>(
                            System.Linq.Expressions.Expression.GreaterThan(
                                getId.Body,
                                System.Linq.Expressions.Expression.Constant(cursor.Id)
                            ),
                            getId.Parameters
                        )
                    );
                }
                else
                {
                    query = query.Where(
                        System.Linq.Expressions.Expression.Lambda<Func<TEntity, bool>>(
                            System.Linq.Expressions.Expression.LessThan(
                                getId.Body,
                                System.Linq.Expressions.Expression.Constant(cursor.Id)
                            ),
                            getId.Parameters
                        )
                    );
                }
            }

            int tamanho = parametros.Tamanho <= 0 ? 20 : parametros.Tamanho;

            query = parametros.Ordem == TipoOrdem.Asc
                ? query.OrderBy(getId)
                : query.OrderByDescending(getId);

            List<TEntity> itens = await query
                .Take(tamanho + 1)
                .ToListAsync(cancellationToken);

            bool temMais = itens.Count > tamanho;

            if (temMais)
            {
                itens = itens.Take(tamanho).ToList();
            }

            string? proximoCursor = null;

            if (temMais && itens.Count > 0)
            {
                long ultimoId = getId.Compile().Invoke(itens[^1]);

                CursorPadrao cursorProximo = new CursorPadrao
                {
                    Id = ultimoId
                };

                proximoCursor = CursorHelper.Serializar(cursorProximo);
            }

            return new ListaScrollInfinito<TEntity>
            {
                Lista = itens,
                TemMais = temMais,
                ProximoCursor = proximoCursor
            };
        }
    }
}
```

## Regra de uso da paginação

Quando um caso de uso exigir listagem incremental, o backend deve preferir paginação por cursor em vez de paginação por página e número total.

Exemplo de uso esperado no service ou query handler:

```csharp
ListaScrollInfinito<EntidadeTeste> pagina = await _repositorio
    .IQueryable()
    .AsNoTracking()
    .ScrollAsync(parametros, x => x.Id, cancellationToken);
```

Se houver DTO de saída, mapear a lista retornada para DTO.

---

# Exemplo obrigatório de fluxo completo: EntidadeTeste

Criar a entidade de exemplo `EntidadeTeste` com o fluxo completo da API até a persistência.

## Campos sugeridos

- `Id` : `long`
- `Nome` : `string`
- `Valor` : `decimal`
- `DataCadastro` : `DateTime`

Pode usar construtor e método de atualização para evitar setters públicos desnecessários.

## Arquivos mínimos

### Projeto `SocialX.Core`

- `Entidades/EntidadeTeste.cs`
- `Interface/IRepositorioGenerico.cs`
- `Interface/IUnitOfWork.cs`
- `Paginacao/TipoOrdem.cs`
- `Paginacao/ParametrosScrollInfinito.cs`
- `Paginacao/ListaScrollInfinito.cs`
- `Paginacao/CursorHelper.cs`
- `Paginacao/CursorPadrao.cs`

### Projeto `SocialX.Infra`

- `Data/CustomDbContext.cs`
- `Configuracoes/EntidadeTesteConfiguration.cs`
- `Repositorio/RepositorioGenerico.cs`
- `UnitOfWork/UnitOfWork.cs`
- `Paginacao/ScrollInfinitoExtensions.cs`

### Projeto `SocialX.Service`

- `DTOs/EntidadeTesteCriarDto.cs`
- `DTOs/EntidadeTesteDto.cs`
- `Interfaces/IEntidadeTesteService.cs`
- `Servicos/EntidadeTesteService.cs`
- `Validadores/EntidadeTesteCriarDtoValidator.cs`
- `Mapeamentos/EntidadeTesteProfile.cs`

### Projeto `SocialX.Api`

- `Controllers/EntidadeTesteController.cs`
- `Handlers/GlobalExceptionHandler.cs`
- `Program.cs`
- `appsettings.json`

---

# Regras para o Service da EntidadeTeste

Criar um serviço chamado `EntidadeTesteService`.

Ele deve receber:

- `IRepositorioGenerico<EntidadeTeste>`
- `IUnitOfWork`
- `IMapper`
- `IValidator<EntidadeTesteCriarDto>`

Deve implementar pelo menos:

- criação
- consulta por id
- listagem com scroll infinito

## Implementação de referência do service

```csharp
using AutoMapper;
using SocialX.Core.Entidades;
using SocialX.Core.Interface;
using SocialX.Core.Paginacao;
using SocialX.Infra.Paginacao;
using SocialX.Service.DTOs;
using SocialX.Service.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace SocialX.Service.Servicos
{
    public class EntidadeTesteService : IEntidadeTesteService
    {
        private readonly IRepositorioGenerico<EntidadeTeste> _repositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<EntidadeTesteCriarDto> _validator;

        public EntidadeTesteService(
            IRepositorioGenerico<EntidadeTeste> repositorio,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<EntidadeTesteCriarDto> validator)
        {
            _repositorio = repositorio;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<EntidadeTesteDto> CriarAsync(EntidadeTesteCriarDto dto)
        {
            await _validator.ValidateAndThrowAsync(dto);

            EntidadeTeste entidade = _mapper.Map<EntidadeTeste>(dto);

            _repositorio.Adicionar(entidade);
            await _unitOfWork.SalvarAsync();

            EntidadeTesteDto retorno = _mapper.Map<EntidadeTesteDto>(entidade);
            return retorno;
        }

        public async Task<EntidadeTesteDto?> ObterPorIdAsync(long id)
        {
            EntidadeTeste? entidade = await _repositorio
                .IQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entidade == null)
            {
                return null;
            }

            EntidadeTesteDto retorno = _mapper.Map<EntidadeTesteDto>(entidade);
            return retorno;
        }

        public async Task<ListaScrollInfinito<EntidadeTesteDto>> ScrollAsync(
            ParametrosScrollInfinito parametros,
            CancellationToken cancellationToken)
        {
            ListaScrollInfinito<EntidadeTeste> paginaEntidade = await _repositorio
                .IQueryable()
                .AsNoTracking()
                .ScrollAsync(parametros, x => x.Id, cancellationToken);

            List<EntidadeTesteDto> listaDto = _mapper.Map<List<EntidadeTesteDto>>(paginaEntidade.Lista);

            ListaScrollInfinito<EntidadeTesteDto> retorno = new ListaScrollInfinito<EntidadeTesteDto>
            {
                Lista = listaDto,
                TemMais = paginaEntidade.TemMais,
                ProximoCursor = paginaEntidade.ProximoCursor
            };

            return retorno;
        }
    }
}
```

Regras obrigatórias:

- o service deve conter o fluxo de aplicação
- o controller não deve validar regra de negócio
- o service deve chamar o validator explicitamente
- o service deve chamar o `UnitOfWork` para persistir
- o service deve usar AutoMapper para entrada e saída
- a listagem deve usar scroll infinito por cursor

---

# Regras para o FluentValidation

Criar um validator para `EntidadeTesteCriarDto`.

Validações mínimas:

- `Nome` obrigatório
- `Nome` com no máximo 150 caracteres
- `Valor` maior que zero

## Implementação de referência do validator

```csharp
using SocialX.Service.DTOs;
using FluentValidation;

namespace SocialX.Service.Validadores
{
    public class EntidadeTesteCriarDtoValidator : AbstractValidator<EntidadeTesteCriarDto>
    {
        public EntidadeTesteCriarDtoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(150).WithMessage("Nome deve ter no máximo 150 caracteres.");

            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("Valor deve ser maior que zero.");
        }
    }
}
```

---

# Regras para o AutoMapper

Criar um profile chamado `EntidadeTesteProfile`.

Deve mapear:

- `EntidadeTesteCriarDto -> EntidadeTeste`
- `EntidadeTeste -> EntidadeTesteDto`

Se a entidade usar construtor, usar `ConstructUsing`.

---

# Regras para o Controller

Criar `EntidadeTesteController` com endpoints:

### `POST /api/entidadeteste`

Recebe `EntidadeTesteCriarDto` e retorna `201 Created`.

### `GET /api/entidadeteste/{id}`

Consulta por id e retorna:

- `200 OK` se encontrar
- `404 NotFound` se não encontrar

### `POST /api/entidadeteste/scroll`

Recebe `ParametrosScrollInfinito` e retorna `200 OK` com `ListaScrollInfinito<EntidadeTesteDto>`.

O controller deve ser fino, sem regra de negócio.

---

# Registro de Dependências

No `Program.cs`, registrar:

- `DbContext`
- `IRepositorioGenerico<>`
- `IUnitOfWork`
- services de aplicação
- AutoMapper
- FluentValidation
- controllers
- tratamento global de exceções com `IExceptionHandler` e `ProblemDetails`
- swagger

Exemplo esperado de DI:

```csharp
builder.Services.AddScoped(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEntidadeTesteService, EntidadeTesteService>();
builder.Services.AddAutoMapper(typeof(EntidadeTesteProfile).Assembly);
builder.Services.AddScoped<IValidator<EntidadeTesteCriarDto>, EntidadeTesteCriarDtoValidator>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
```

No pipeline HTTP, usar também:

```csharp
app.UseExceptionHandler();
```

---

# Regras para React Native

## TypeScript Estrito

Nunca usar:

```ts
any
```

Sempre usar interfaces explícitas.

Exemplo:

```ts
interface EventoDto {
    id: number;
    nome: string;
    descricao: string;
    dataHoraInicio: string;
}
```

## Tipagem de API

Sempre tipar:

- retorno do servidor
- parâmetros enviados
- estados do componente

Exemplo:

```ts
interface CriarEventoRequest {
    nome: string;
    descricao: string;
}
```

## Componentes

Cada tela deve ter:

- componente
- interface de props
- testes

Estrutura sugerida:

```text
screens/
components/
services/
types/
tests/
```

---

# Testes Automatizados

## Backend

Framework sugerido:

- xUnit

Tipos de testes:

- testes de controller
- testes de service
- testes de regras de negócio

## Frontend

Frameworks:

- Jest
- React Native Testing Library

Testar:

- renderização
- interação
- chamadas de API simuladas

---

# Ferramentas de Qualidade

## Frontend

Executar sempre:

```bash
npm run lint
```

Configurar regras:

- `no-explicit-any`
- strict typing
- import ordering

## Prettier

Padronização automática de código.

## TypeScript Strict Mode

No `tsconfig.json`:

```json
{
  "compilerOptions": {
    "strict": true
  }
}
```

## Ferramentas MCP sugeridas

- ESLint MCP
- TypeScript Language Server
- Jest MCP
- Prettier MCP

Essas ferramentas ajudam a IA a validar código, detectar erros e garantir consistência.

---

# Banco de Dados

Usar Entity Framework Core com PostgreSQL.

Criar `DbSet<EntidadeTeste>` no `CustomDbContext`.

Na configuração `EntidadeTesteConfiguration`:

- definir nome da tabela
- chave primária
- tamanho do campo `Nome`
- tipo decimal do campo `Valor`
- obrigatoriedades

---

# Qualidade esperada do código

O código gerado deve:

- compilar
- respeitar as dependências entre camadas
- evitar código desnecessário
- evitar classes vazias sem propósito
- ter nomes claros
- seguir boas práticas de .NET
- usar paginação por cursor quando houver listagens incrementais

---

# Ordem de geração desejada

Gerar na seguinte ordem:

1. solução e projetos
2. referências entre projetos
3. Core
4. Infra
5. Service
6. Api
7. DI no `Program.cs`
8. exemplo completo da `EntidadeTeste`
9. paginação infinita
10. validação final de compilação

---

# Instruções finais para a IA

- gerar todos os arquivos necessários para o exemplo funcionar ponta a ponta
- manter a arquitetura simples e coerente
- não criar repositórios especializados desnecessários
- usar `IRepositorioGenerico<T>` como padrão inicial
- deixar o `SaveChanges` centralizado no `UnitOfWork`
- colocar AutoMapper e FluentValidation no projeto `Service`
- criar um exemplo completo com `EntidadeTeste`
- incluir paginação infinita por cursor
- entregar o código de forma organizada por projeto e arquivo
- garantir que a solução compile sem ajustes manuais relevantes
