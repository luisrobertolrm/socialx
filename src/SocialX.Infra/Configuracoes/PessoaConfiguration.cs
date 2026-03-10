using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Entidades;
using SocialX.Core.Enums;

namespace SocialX.Infra.Configuracoes;

public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
{
    public void Configure(EntityTypeBuilder<Pessoa> builder)
    {
        builder.ToTable("pessoa");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Nome)
            .HasColumnName("nome")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Apelido)
            .HasColumnName("apelido")
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(e => e.Telefone)
            .HasColumnName("telefone")
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.DataNascimento)
            .HasColumnName("data_nascimento")
            .IsRequired();

        builder.Property(e => e.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("uq_pessoa_email");

        builder.Property(e => e.FotoPerfil)
            .HasColumnName("foto_perfil")
            .HasMaxLength(500);

        builder.Property(e => e.Bio)
            .HasColumnName("bio")
            .HasColumnType("text");

        builder.Property(e => e.Cidade)
            .HasColumnName("cidade")
            .HasMaxLength(120);

        builder.Property(e => e.Role)
            .HasColumnName("role")
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(RoleUsuarioEnum.Usuario);

        builder.Property(e => e.StatusConta)
            .HasColumnName("status_conta")
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(StatusContaEnum.Ativa);

        builder.Property(e => e.DataCriacao)
            .HasColumnName("data_criacao")
            .IsRequired()
            .HasDefaultValueSql("current_timestamp");

        builder.Property(e => e.UltimoAcesso)
            .HasColumnName("ultimo_acesso");

        builder.HasIndex(e => e.Apelido)
            .IsUnique()
            .HasDatabaseName("uq_pessoa_apelido");

        builder.HasIndex(e => e.Nome)
            .HasDatabaseName("ix_pessoa_nome");

        builder.HasIndex(e => e.StatusConta)
            .HasDatabaseName("ix_pessoa_status_conta");

        builder.HasIndex(e => e.Role)
            .HasDatabaseName("ix_pessoa_role");
    }
}
