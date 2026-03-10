using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Entidades;

namespace SocialX.Infra.Configuracoes;

public class MidiaConfiguration : IEntityTypeConfiguration<Midia>
{
    public void Configure(EntityTypeBuilder<Midia> builder)
    {
        builder.ToTable("midia");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.TipoDono)
            .HasColumnName("tipo_dono")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.DonoId)
            .HasColumnName("dono_id")
            .IsRequired();

        builder.Property(e => e.TipoArquivo)
            .HasColumnName("tipo_arquivo")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.UrlArquivo)
            .HasColumnName("url_arquivo")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.UrlThumbnail)
            .HasColumnName("url_thumbnail")
            .HasMaxLength(500);

        builder.Property(e => e.Ordem)
            .HasColumnName("ordem")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.MimeType)
            .HasColumnName("mime_type")
            .HasMaxLength(100);

        builder.Property(e => e.TamanhoBytes)
            .HasColumnName("tamanho_bytes");

        builder.Property(e => e.DuracaoSegundos)
            .HasColumnName("duracao_segundos");

        builder.Property(e => e.DataUpload)
            .HasColumnName("data_upload")
            .IsRequired()
            .HasDefaultValueSql("current_timestamp");

        builder.HasIndex(e => new { e.TipoDono, e.DonoId })
            .HasDatabaseName("ix_midia_dono");

        builder.HasIndex(e => e.TipoArquivo)
            .HasDatabaseName("ix_midia_tipo_arquivo");
    }
}
