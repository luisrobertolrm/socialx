using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Entidades;
using SocialX.Core.Enums;

namespace SocialX.Infra.Configuracoes;

public class DenunciaConfiguration : IEntityTypeConfiguration<Denuncia>
{
    public void Configure(EntityTypeBuilder<Denuncia> builder)
    {
        builder.ToTable("denuncia");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.DenunciantePessoaId)
            .HasColumnName("denunciante_pessoa_id")
            .IsRequired();

        builder.Property(e => e.TipoAlvo)
            .HasColumnName("tipo_alvo")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.AlvoId)
            .HasColumnName("alvo_id")
            .IsRequired();

        builder.Property(e => e.Motivo)
            .HasColumnName("motivo")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Descricao)
            .HasColumnName("descricao")
            .HasColumnType("text");

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(StatusDenunciaEnum.Aberta);

        builder.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .IsRequired()
            .HasDefaultValueSql("current_timestamp");

        builder.Property(e => e.ResolvidoPorAdminId)
            .HasColumnName("resolvido_por_admin_id");

        builder.Property(e => e.ResolvidoEm)
            .HasColumnName("resolvido_em");

        builder.HasOne(e => e.Denunciante)
            .WithMany()
            .HasForeignKey(e => e.DenunciantePessoaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ResolvidoPor)
            .WithMany()
            .HasForeignKey(e => e.ResolvidoPorAdminId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.DenunciantePessoaId)
            .HasDatabaseName("ix_denuncia_denunciante");

        builder.HasIndex(e => e.Status)
            .HasDatabaseName("ix_denuncia_status");

        builder.HasIndex(e => new { e.TipoAlvo, e.AlvoId })
            .HasDatabaseName("ix_denuncia_alvo");
    }
}
