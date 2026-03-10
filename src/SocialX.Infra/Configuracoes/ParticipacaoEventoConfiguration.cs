using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Entidades;

namespace SocialX.Infra.Configuracoes;

public class ParticipacaoEventoConfiguration : IEntityTypeConfiguration<ParticipacaoEvento>
{
    public void Configure(EntityTypeBuilder<ParticipacaoEvento> builder)
    {
        builder.ToTable("participacao_evento");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.EventoId)
            .HasColumnName("evento_id")
            .IsRequired();

        builder.Property(e => e.PessoaId)
            .HasColumnName("pessoa_id")
            .IsRequired();

        builder.Property(e => e.StatusParticipacao)
            .HasColumnName("status_participacao")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.Observacao)
            .HasColumnName("observacao")
            .HasColumnType("text");

        builder.Property(e => e.DataResposta)
            .HasColumnName("data_resposta")
            .IsRequired()
            .HasDefaultValueSql("current_timestamp");

        builder.HasOne(e => e.Evento)
            .WithMany()
            .HasForeignKey(e => e.EventoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Pessoa)
            .WithMany()
            .HasForeignKey(e => e.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.EventoId, e.PessoaId })
            .IsUnique()
            .HasDatabaseName("uq_participacao_evento");

        builder.HasIndex(e => e.PessoaId)
            .HasDatabaseName("ix_participacao_evento_pessoa");
    }
}
