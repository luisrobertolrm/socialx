using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Entidades;

namespace SocialX.Infra.Configuracoes;

public class AmizadeConfiguration : IEntityTypeConfiguration<Amizade>
{
    public void Configure(EntityTypeBuilder<Amizade> builder)
    {
        builder.ToTable("amizade");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.PessoaOrigemId)
            .HasColumnName("pessoa_origem_id")
            .IsRequired();

        builder.Property(e => e.PessoaDestinoId)
            .HasColumnName("pessoa_destino_id")
            .IsRequired();

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.DataSolicitacao)
            .HasColumnName("data_solicitacao")
            .IsRequired()
            .HasDefaultValueSql("current_timestamp");

        builder.Property(e => e.DataResposta)
            .HasColumnName("data_resposta");

        builder.HasOne(e => e.PessoaOrigem)
            .WithMany()
            .HasForeignKey(e => e.PessoaOrigemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.PessoaDestino)
            .WithMany()
            .HasForeignKey(e => e.PessoaDestinoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.PessoaOrigemId, e.PessoaDestinoId })
            .IsUnique()
            .HasDatabaseName("uq_amizade_origem_destino");

        builder.HasIndex(e => e.PessoaDestinoId)
            .HasDatabaseName("ix_amizade_destino");

        builder.HasIndex(e => e.Status)
            .HasDatabaseName("ix_amizade_status");
    }
}
