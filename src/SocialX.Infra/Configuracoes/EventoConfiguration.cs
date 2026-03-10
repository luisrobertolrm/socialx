using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Entidades;
using SocialX.Core.Enums;

namespace SocialX.Infra.Configuracoes;

public class EventoConfiguration : IEntityTypeConfiguration<Evento>
{
    public void Configure(EntityTypeBuilder<Evento> builder)
    {
        builder.ToTable("evento");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.GrupoId)
            .HasColumnName("grupo_id");

        builder.Property(e => e.CriadoPorPessoaId)
            .HasColumnName("criado_por_pessoa_id")
            .IsRequired();

        builder.Property(e => e.Nome)
            .HasColumnName("nome")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Descricao)
            .HasColumnName("descricao")
            .IsRequired()
            .HasColumnType("text");

        builder.Property(e => e.TipoEvento)
            .HasColumnName("tipo_evento")
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(TipoEventoEnum.Presencial);

        builder.Property(e => e.DataHoraInicio)
            .HasColumnName("data_hora_inicio")
            .IsRequired();

        builder.Property(e => e.DataHoraFim)
            .HasColumnName("data_hora_fim");

        builder.Property(e => e.LocalNome)
            .HasColumnName("local_nome")
            .HasMaxLength(150);

        builder.Property(e => e.Endereco)
            .HasColumnName("endereco")
            .HasColumnType("text");

        builder.Property(e => e.Latitude)
            .HasColumnName("latitude")
            .HasPrecision(9, 6);

        builder.Property(e => e.Longitude)
            .HasColumnName("longitude")
            .HasPrecision(9, 6);

        builder.Property(e => e.LinkOnline)
            .HasColumnName("link_online")
            .HasMaxLength(500);

        builder.Property(e => e.CapacidadeMaxima)
            .HasColumnName("capacidade_maxima");

        builder.Property(e => e.Privacidade)
            .HasColumnName("privacidade")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(StatusEventoEnum.Ativo);

        builder.Property(e => e.DataCriacao)
            .HasColumnName("data_criacao")
            .IsRequired()
            .HasDefaultValueSql("current_timestamp");

        builder.HasOne(e => e.Grupo)
            .WithMany()
            .HasForeignKey(e => e.GrupoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.CriadoPor)
            .WithMany()
            .HasForeignKey(e => e.CriadoPorPessoaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.GrupoId)
            .HasDatabaseName("ix_evento_grupo");

        builder.HasIndex(e => e.CriadoPorPessoaId)
            .HasDatabaseName("ix_evento_criado_por");

        builder.HasIndex(e => e.DataHoraInicio)
            .HasDatabaseName("ix_evento_data_inicio");

        builder.HasIndex(e => e.Status)
            .HasDatabaseName("ix_evento_status");

        builder.HasIndex(e => e.Privacidade)
            .HasDatabaseName("ix_evento_privacidade");

        builder.HasIndex(e => e.TipoEvento)
            .HasDatabaseName("ix_evento_tipo");

        builder.HasIndex(e => new { e.Latitude, e.Longitude })
            .HasDatabaseName("ix_evento_latitude_longitude");
    }
}
