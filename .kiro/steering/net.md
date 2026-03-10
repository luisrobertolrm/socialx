# Regras Backend .NET - SocialX

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- AutoMapper
- FluentValidation
- xUnit

---

## Processo de Desenvolvimento

### Regra Fundamental

Sempre criar os testes antes de implementar o código.

Ordem obrigatória:

1. Criar o teste da funcionalidade
2. Criar a estrutura mínima de controller, service ou componente
3. Implementar a lógica mínima para passar no teste
4. Refatorar se necessário
5. Solicitar aprovação para continuar para o próximo caso de uso

### Desenvolvimento Incremental

- A IA não pode implementar múltiplos casos de uso automaticamente
- Cada funcionalidade deve ser autorizada explicitamente
- Cada tarefa deve terminar compilando e com testes passando

### Compilação Obrigatória

Antes de finalizar qualquer tarefa, executar:

```bash
dotnet build
dotnet test
```

A tarefa só pode ser considerada concluída após compilação e testes passando.

---

## Arquitetura do Backend .NET

O backend deve seguir a arquitetura em 4 projetos.

### Estrutura da Solução

```text
SocialX.sln

src/
  SocialX.Core/
    Entidades/
    Enums/
    Interface/
    Paginacao/
    Exceptions/

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
    Handlers/
    Program.cs
    appsettings.json

tests/
  SocialX.Api.Tests/
  SocialX.Service.Tests/
```

### Responsabilidade de cada projeto

#### `SocialX.Core`

Responsável por:

- entidades de domínio
- enums
- interfaces de abstração
- contratos e modelos reutilizáveis de paginação
- exceções de domínio

Não deve depender de `Infra`, `Service` ou `Api`.

#### `SocialX.Infra`

Responsável por:

- `DbContext`
- configurações do EF Core com `IEntityTypeConfiguration`
- implementação do repositório genérico
- implementação do `UnitOfWork`
- implementação da paginação infinita que depende de `IQueryable` e `EF Core`

#### `SocialX.Service`

Responsável por:

- DTOs
- serviços de aplicação
- AutoMapper Profiles
- FluentValidation Validators
- regras de negócio de aplicação

#### `SocialX.Api`

Responsável por:

- controllers
- configuração do container DI
- configuração do pipeline HTTP
- registro de AutoMapper, FluentValidation, DbContext e serviços
- registro de `IExceptionHandler` e `ProblemDetails`
- handlers globais de exceção

---

## Regras para Código .NET

### Tipagem obrigatória

Nunca usar `var`. Sempre usar tipo explícito.

Errado:

```csharp
var eventos = new List<Evento>();
```

Correto:

```csharp
List<Evento> eventos = new List<Evento>();
```

### Controllers

Os controllers devem ser finos.

Devem apenas:

- receber a requisição
- chamar o service
- retornar a resposta HTTP adequada

Nunca colocar regra de negócio no controller.

### AutoMapper

Os Profiles do AutoMapper devem ficar no projeto `SocialX.Service`, pois o mapeamento principal será entre DTOs e entidades.

### FluentValidation

Os Validators devem ficar no projeto `SocialX.Service`.

---

## Tratamento Global de Exceções

A API deve usar tratamento global de exceções com `IExceptionHandler` + `ProblemDetails`.

Objetivos:

- remover `try/catch` repetitivo de controllers
- padronizar respostas de erro
- separar exceções de domínio de falhas inesperadas
- retornar status HTTP corretos

### Onde colocar cada parte

#### Em `SocialX.Core/Exceptions`

Colocar as exceções de domínio:

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

#### Em `SocialX.Api/Handlers`

Colocar o handler global:

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

#### Em `SocialX.Api/Program.cs`

Registrar:

```csharp
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// No pipeline HTTP:
app.UseExceptionHandler();
```

### Regras obrigatórias para exceptions

- controllers não devem ter `try/catch` para regras normais de aplicação
- services devem lançar exceções de domínio quando necessário
- `NotFoundException` deve gerar `404`
- `BusinessRuleException` deve gerar `400`
- exceções inesperadas devem gerar `500` e ser logadas
- usar `ProblemDetails` como formato padrão de erro

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

---

## Repository Genérico

Não criar repositórios especializados por entidade por padrão.

Usar:

- `IRepositorioGenerico<T>`
- `RepositorioGenerico<T>`

Registrar no DI desta forma:

```csharp
builder.Services.AddScoped(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
```

Criar repositório especializado apenas se existir necessidade real de consultas ou operações específicas da entidade.

### Interface `IRepositorioGenerico<T>`

```csharp
public interface IRepositorioGenerico<T> where T : class
{
    T Adicionar(T entidade);
    T Editar(T entidade);
    void Deletar(T entidade);
    IQueryable<T> IQueryable();
}
```

### Implementação de referência

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

## Unit of Work

A persistência não deve ficar dentro de `Adicionar`, `Editar` ou `Deletar`.

Esses métodos apenas registram a operação no contexto. A gravação no banco deve acontecer apenas no `UnitOfWork`, com `SalvarAsync()`.

Interface:

- `IUnitOfWork` deve ficar em `SocialX.Core`

Implementação:

- `UnitOfWork` deve ficar em `SocialX.Infra`

### Interface `IUnitOfWork`

```csharp
public interface IUnitOfWork
{
    Task<bool> SalvarAsync();
}
```

### Implementação de referência

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

## Padrão de Paginação Infinita

O projeto deve incluir suporte nativo a paginação infinita baseada em cursor.

### Onde colocar cada parte

#### Em `SocialX.Core/Paginacao`

Colocar as classes e contratos independentes de EF Core:

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

#### Em `SocialX.Infra/Paginacao`

Colocar a implementação que depende de `IQueryable`, `Where`, `Take` e `ToListAsync`.

Importante: usar `Expression<Func<TEntity, long>> getId` em vez de `Func<TEntity, long> getId` para permitir tradução para SQL pelo EF Core.

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

### Regra de uso da paginação

Quando um caso de uso exigir listagem incremental, o backend deve preferir paginação por cursor em vez de paginação por página e número total.

Exemplo de uso esperado no service:

```csharp
ListaScrollInfinito<EntidadeTeste> pagina = await _repositorio
    .IQueryable()
    .AsNoTracking()
    .ScrollAsync(parametros, x => x.Id, cancellationToken);
```

---

## Exemplo de Referência: EntidadeTeste

### Campos sugeridos

- `Id` : `long`
- `Nome` : `string`
- `Valor` : `decimal`
- `DataCadastro` : `DateTime`

### Service

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

### FluentValidation

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

### Controller

Criar `EntidadeTesteController` com endpoints:

- `POST /api/entidadeteste` - Recebe `EntidadeTesteCriarDto` e retorna `201 Created`
- `GET /api/entidadeteste/{id}` - Retorna `200 OK` ou `404 NotFound`
- `POST /api/entidadeteste/scroll` - Recebe `ParametrosScrollInfinito` e retorna `200 OK` com `ListaScrollInfinito<EntidadeTesteDto>`

O controller deve ser fino, sem regra de negócio.

---

## Registro de Dependências

No `Program.cs`, registrar:

```csharp
builder.Services.AddScoped(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEntidadeTesteService, EntidadeTesteService>();
builder.Services.AddAutoMapper(typeof(EntidadeTesteProfile).Assembly);
builder.Services.AddScoped<IValidator<EntidadeTesteCriarDto>, EntidadeTesteCriarDtoValidator>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
```

No pipeline HTTP:

```csharp
app.UseExceptionHandler();
```

---

## Banco de Dados

Usar Entity Framework Core com PostgreSQL.

Criar `DbSet<EntidadeTeste>` no `CustomDbContext`.

Na configuração `EntidadeTesteConfiguration`:

- definir nome da tabela
- chave primária
- tamanho do campo `Nome`
- tipo decimal do campo `Valor`
- obrigatoriedades

---

## Testes Automatizados

Framework: xUnit

Tipos de testes:

- testes de controller
- testes de service
- testes de regras de negócio

---

## Qualidade Esperada do Código

O código gerado deve:

- compilar
- respeitar as dependências entre camadas
- evitar código desnecessário
- evitar classes vazias sem propósito
- ter nomes claros
- seguir boas práticas de .NET
- usar paginação por cursor quando houver listagens incrementais

---

## Ordem de Geração Desejada

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
