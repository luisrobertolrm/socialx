using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Entidades;
using SocialX.Core.Enums;

namespace SocialX.Infra.Configuracoes;

public class GrupoConfiguration : IEntityTypeConfiguration<Grupo>
{
    public void Configure(EntityTypeBuilder<Grupo> builder)
    {
        builder.ToTable("grupo");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Nome)
            .HasColumnName("nome")
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(e => e.Descricao)
            .HasColumnName("descricao")
            .HasColumnType("text");

        builder.Property(e => e.FotoCapa)
            .HasColumnName("foto_capa")
            .HasMaxLength(500);

        builder.Property(e => e.FotoPerfil)
            .HasColumnName("foto_perfil")
            .HasMaxLength(500);

        builder.Property(e => e.TipoPrivacidade)
            .HasColumnName("tipo_privacidade")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.Regras)
            .HasColumnName("regras")
            .HasColumnType("text");

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(StatusGrupoEnum.Ativo);

        builder.Property(e => e.CriadoPorPessoaId)
            .HasColumnName("criado_por_pessoa_id")
            .IsRequired();

        builder.Property(e => e.DataCriacao)
            .HasColumnName("data_criacao")
            .IsRequired()
            .HasDefaultValueSql("current_timestamp");

        builder.HasOne(e => e.CriadoPor)
            .WithMany()
            .HasForeignKey(e => e.CriadoPorPessoaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.CriadoPorPessoaId)
            .HasDatabaseName("ix_grupo_criado_por");

        builder.HasIndex(e => e.TipoPrivacidade)
            .HasDatabaseName("ix_grupo_tipo_privacidade");

        builder.HasIndex(e => e.Status)
            .HasDatabaseName("ix_grupo_status");

        builder.HasIndex(e => e.DataCriacao)
            .IsDescending()
            .HasDatabaseName("ix_grupo_data_criacao");

        builder.HasIndex(e => new { e.TipoPrivacidade, e.Status })
            .HasDatabaseName("ix_grupo_tipo_status");
    }
}
