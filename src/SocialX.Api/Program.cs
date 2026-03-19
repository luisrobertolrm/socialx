using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using SocialX.Api.Handlers;
using SocialX.Core.Interface;
using SocialX.Infra.Data;
using SocialX.Infra.Repositorio;
using SocialX.Infra.UnitOfWork;
using SocialX.Service.DTOs;
using SocialX.Service.Interfaces;
using SocialX.Service.Mapeamentos;
using SocialX.Service.Servicos;
using SocialX.Service.Validadores;
using FluentValidation;
using AutoMapper;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<CustomDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository and UnitOfWork
builder.Services.AddScoped(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IEntidadeTesteService, EntidadeTesteService>();

// AutoMapper
MapperConfiguration config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<EntidadeTesteProfile>();
}, NullLoggerFactory.Instance);
IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

// FluentValidation
builder.Services.AddScoped<IValidator<EntidadeTesteCriarDto>, EntidadeTesteCriarDtoValidator>();

// CORS - Permitir requisições do frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

// CORS - Deve vir antes de UseAuthorization
app.UseCors("AllowFrontend");

// Desabilitar redirecionamento HTTPS em desenvolvimento
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
