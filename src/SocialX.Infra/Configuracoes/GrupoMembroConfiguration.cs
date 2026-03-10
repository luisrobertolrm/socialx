using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Entidades;

namespace SocialX.Infra.Configuracoes;

public class GrupoMembroConfiguration : IEntityTypeConfiguration<GrupoMembro>
{
    public void Configure(EntityTypeBuilder<GrupoMembro> builder)
    {
        builder.ToTable("grupo_membro");

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

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.DataEntrada)
            .HasColumnName("data_entrada")
            .IsRequired()
            .HasDefaultValueSql("current_timestamp");

        builder.Property(e => e.AprovadoPorPessoaId)
            .HasColumnName("aprovado_por_pessoa_id");

        builder.Property(e => e.DataAprovacao)
            .HasColumnName("data_aprovacao");

        builder.HasOne(e => e.Grupo)
            .WithMany()
            .HasForeignKey(e => e.GrupoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Pessoa)
            .WithMany()
            .HasForeignKey(e => e.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.AprovadoPor)
            .WithMany()
            .HasForeignKey(e => e.AprovadoPorPessoaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.GrupoId, e.PessoaId })
            .IsUnique()
            .HasDatabaseName("uq_grupo_membro");

        builder.HasIndex(e => e.PessoaId)
            .HasDatabaseName("ix_grupo_membro_pessoa");
    }
}
