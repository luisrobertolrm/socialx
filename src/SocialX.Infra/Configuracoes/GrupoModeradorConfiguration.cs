using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Entidades;

namespace SocialX.Infra.Configuracoes;

public class GrupoModeradorConfiguration : IEntityTypeConfiguration<GrupoModerador>
{
    public void Configure(EntityTypeBuilder<GrupoModerador> builder)
    {
        builder.ToTable("grupo_moderador");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.GrupoId)
            .HasColumnName("grupo_id")
            .IsRequired();

        builder.Property(e => e.PessoaId)
            .HasColumnName("pessoa_id")
            .IsRequired();

        builder.Property(e => e.Papel)
            .HasColumnName("papel")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.DataInicio)
            .HasColumnName("data_inicio")
            .IsRequired()
            .HasDefaultValueSql("current_timestamp");

        builder.HasOne(e => e.Grupo)
            .WithMany()
            .HasForeignKey(e => e.GrupoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Pessoa)
            .WithMany()
            .HasForeignKey(e => e.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.GrupoId, e.PessoaId })
            .IsUnique()
            .HasDatabaseName("uq_grupo_moderador");

        builder.HasIndex(e => e.PessoaId)
            .HasDatabaseName("ix_grupo_moderador_pessoa");
    }
}
