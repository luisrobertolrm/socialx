using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Entidades;
using SocialX.Core.Enums;

namespace SocialX.Infra.Configuracoes;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("post");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.AutorPessoaId)
            .HasColumnName("autor_pessoa_id")
            .IsRequired();

        builder.Property(e => e.GrupoId)
            .HasColumnName("grupo_id");

        builder.Property(e => e.EventoId)
            .HasColumnName("evento_id");

        builder.Property(e => e.Titulo)
            .HasColumnName("titulo")
            .HasMaxLength(150);

        builder.Property(e => e.Texto)
            .HasColumnName("texto")
            .IsRequired()
            .HasColumnType("text");

        builder.Property(e => e.Visibilidade)
            .HasColumnName("visibilidade")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(StatusPostEnum.Ativo);

        builder.Property(e => e.Fixado)
            .HasColumnName("fixado")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.DataPublicacao)
            .HasColumnName("data_publicacao")
            .IsRequired()
            .HasDefaultValueSql("current_timestamp");

        builder.Property(e => e.DataEdicao)
            .HasColumnName("data_edicao");

        builder.HasOne(e => e.Autor)
            .WithMany()
            .HasForeignKey(e => e.AutorPessoaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Grupo)
            .WithMany()
            .HasForeignKey(e => e.GrupoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Evento)
            .WithMany()
            .HasForeignKey(e => e.EventoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.AutorPessoaId)
            .HasDatabaseName("ix_post_autor");

        builder.HasIndex(e => e.GrupoId)
            .HasDatabaseName("ix_post_grupo");

        builder.HasIndex(e => e.EventoId)
            .HasDatabaseName("ix_post_evento");

        builder.HasIndex(e => e.Status)
            .HasDatabaseName("ix_post_status");

        builder.HasIndex(e => e.Visibilidade)
            .HasDatabaseName("ix_post_visibilidade");

        builder.HasIndex(e => e.DataPublicacao)
            .IsDescending()
            .HasDatabaseName("ix_post_data_publicacao");
    }
}
