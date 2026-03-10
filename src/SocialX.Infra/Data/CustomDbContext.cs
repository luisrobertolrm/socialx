using Microsoft.EntityFrameworkCore;
using SocialX.Core.Entidades;

namespace SocialX.Infra.Data;

public class CustomDbContext : DbContext
{
    public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
    {
    }

    public DbSet<EntidadeTeste> EntidadesTeste { get; set; }
    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<LoginSocial> LoginsSociais { get; set; }
    public DbSet<Amizade> Amizades { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    public DbSet<GrupoMembro> GruposMembros { get; set; }
    public DbSet<GrupoModerador> GruposModeradores { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<ParticipacaoEvento> ParticipacoesEventos { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Midia> Midias { get; set; }
    public DbSet<Denuncia> Denuncias { get; set; }
    public DbSet<Notificacao> Notificacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomDbContext).Assembly);
    }
}
