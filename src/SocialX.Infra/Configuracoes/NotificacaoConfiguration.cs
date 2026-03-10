using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Entidades;

namespace SocialX.Infra.Configuracoes;

public class NotificacaoConfiguration : IEntityTypeConfiguration<Notificacao>
{
    public void Configure(EntityTypeBuilder<Notificacao> builder)
    {
        builder.ToTable("notificacao");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.PessoaId)
            .HasColumnName("pessoa_id")
            .IsRequired();

        builder.Property(e => e.Tipo)
            .HasColumnName("tipo")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Titulo)
            .HasColumnName("titulo")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Mensagem)
            .HasColumnName("mensagem")
            .IsRequired()
            .HasColumnType("text");

        builder.Property(e => e.ReferenciaTipo)
            .HasColumnName("referencia_tipo")
            .HasMaxLength(30);

        builder.Property(e => e.ReferenciaId)
            .HasColumnName("referencia_id");

        builder.Property(e => e.Lida)
            .HasColumnName("lida")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.DataEnvio)
            .HasColumnName("data_envio")
            .IsRequired()
            .HasDefaultValueSql("current_timestamp");

        builder.HasOne(e => e.Pessoa)
            .WithMany()
            .HasForeignKey(e => e.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.PessoaId)
            .HasDatabaseName("ix_notificacao_pessoa");

        builder.HasIndex(e => new { e.PessoaId, e.Lida })
            .HasDatabaseName("ix_notificacao_pessoa_lida");
    }
}
