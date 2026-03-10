using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Entidades;
using SocialX.Core.Enums;

namespace SocialX.Infra.Configuracoes;

public class LoginSocialConfiguration : IEntityTypeConfiguration<LoginSocial>
{
    public void Configure(EntityTypeBuilder<LoginSocial> builder)
    {
        builder.ToTable("login_social");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.PessoaId)
            .HasColumnName("pessoa_id")
            .IsRequired();

        builder.Property(e => e.Provider)
            .HasColumnName("provider")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.ProviderUserId)
            .HasColumnName("provider_user_id")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.EmailProvider)
            .HasColumnName("email_provider")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.DataVinculo)
            .HasColumnName("data_vinculo")
            .IsRequired()
            .HasDefaultValueSql("current_timestamp");

        builder.HasOne(e => e.Pessoa)
            .WithMany(p => p.LoginsSociais)
            .HasForeignKey(e => e.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.Provider, e.ProviderUserId })
            .IsUnique()
            .HasDatabaseName("uq_login_social_provider_user");

        builder.HasIndex(e => e.PessoaId)
            .HasDatabaseName("ix_login_social_pessoa");
    }
}
