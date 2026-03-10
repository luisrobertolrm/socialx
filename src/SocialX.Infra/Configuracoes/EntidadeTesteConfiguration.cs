using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Entidades;

namespace SocialX.Infra.Configuracoes;

public class EntidadeTesteConfiguration : IEntityTypeConfiguration<EntidadeTeste>
{
    public void Configure(EntityTypeBuilder<EntidadeTeste> builder)
    {
        builder.ToTable("EntidadeTeste");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Valor)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.DataCadastro)
            .IsRequired();
    }
}
