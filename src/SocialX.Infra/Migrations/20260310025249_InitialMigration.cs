using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SocialX.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntidadeTeste",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntidadeTeste", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "midia",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tipo_dono = table.Column<string>(type: "text", nullable: false),
                    dono_id = table.Column<long>(type: "bigint", nullable: false),
                    tipo_arquivo = table.Column<string>(type: "text", nullable: false),
                    url_arquivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    url_thumbnail = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ordem = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    mime_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    tamanho_bytes = table.Column<long>(type: "bigint", nullable: true),
                    duracao_segundos = table.Column<int>(type: "integer", nullable: true),
                    data_upload = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_midia", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pessoa",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    apelido = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    data_nascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    foto_perfil = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    bio = table.Column<string>(type: "text", nullable: true),
                    cidade = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    role = table.Column<string>(type: "text", nullable: false, defaultValue: "Usuario"),
                    status_conta = table.Column<string>(type: "text", nullable: false, defaultValue: "Ativa"),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp"),
                    ultimo_acesso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pessoa", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "amizade",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pessoa_origem_id = table.Column<long>(type: "bigint", nullable: false),
                    pessoa_destino_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    data_solicitacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp"),
                    data_resposta = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_amizade", x => x.id);
                    table.ForeignKey(
                        name: "FK_amizade_pessoa_pessoa_destino_id",
                        column: x => x.pessoa_destino_id,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_amizade_pessoa_pessoa_origem_id",
                        column: x => x.pessoa_origem_id,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "denuncia",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    denunciante_pessoa_id = table.Column<long>(type: "bigint", nullable: false),
                    tipo_alvo = table.Column<string>(type: "text", nullable: false),
                    alvo_id = table.Column<long>(type: "bigint", nullable: false),
                    motivo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Aberta"),
                    criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp"),
                    resolvido_por_admin_id = table.Column<long>(type: "bigint", nullable: true),
                    resolvido_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_denuncia", x => x.id);
                    table.ForeignKey(
                        name: "FK_denuncia_pessoa_denunciante_pessoa_id",
                        column: x => x.denunciante_pessoa_id,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_denuncia_pessoa_resolvido_por_admin_id",
                        column: x => x.resolvido_por_admin_id,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "grupo",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    descricao = table.Column<string>(type: "text", nullable: true),
                    foto_capa = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    foto_perfil = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    tipo_privacidade = table.Column<string>(type: "text", nullable: false),
                    regras = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Ativo"),
                    criado_por_pessoa_id = table.Column<long>(type: "bigint", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grupo", x => x.id);
                    table.ForeignKey(
                        name: "FK_grupo_pessoa_criado_por_pessoa_id",
                        column: x => x.criado_por_pessoa_id,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "login_social",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pessoa_id = table.Column<long>(type: "bigint", nullable: false),
                    provider = table.Column<string>(type: "text", nullable: false),
                    provider_user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email_provider = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    data_vinculo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login_social", x => x.id);
                    table.ForeignKey(
                        name: "FK_login_social_pessoa_pessoa_id",
                        column: x => x.pessoa_id,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notificacao",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pessoa_id = table.Column<long>(type: "bigint", nullable: false),
                    tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    titulo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    mensagem = table.Column<string>(type: "text", nullable: false),
                    referencia_tipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    referencia_id = table.Column<long>(type: "bigint", nullable: true),
                    lida = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    data_envio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notificacao", x => x.id);
                    table.ForeignKey(
                        name: "FK_notificacao_pessoa_pessoa_id",
                        column: x => x.pessoa_id,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "evento",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grupo_id = table.Column<long>(type: "bigint", nullable: true),
                    criado_por_pessoa_id = table.Column<long>(type: "bigint", nullable: false),
                    nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    descricao = table.Column<string>(type: "text", nullable: false),
                    tipo_evento = table.Column<string>(type: "text", nullable: false, defaultValue: "Presencial"),
                    data_hora_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_hora_fim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    local_nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    endereco = table.Column<string>(type: "text", nullable: true),
                    latitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    longitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    link_online = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    capacidade_maxima = table.Column<int>(type: "integer", nullable: true),
                    privacidade = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Ativo"),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evento", x => x.id);
                    table.ForeignKey(
                        name: "FK_evento_grupo_grupo_id",
                        column: x => x.grupo_id,
                        principalTable: "grupo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_evento_pessoa_criado_por_pessoa_id",
                        column: x => x.criado_por_pessoa_id,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "grupo_membro",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grupo_id = table.Column<long>(type: "bigint", nullable: false),
                    pessoa_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    data_entrada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp"),
                    aprovado_por_pessoa_id = table.Column<long>(type: "bigint", nullable: true),
                    data_aprovacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grupo_membro", x => x.id);
                    table.ForeignKey(
                        name: "FK_grupo_membro_grupo_grupo_id",
                        column: x => x.grupo_id,
                        principalTable: "grupo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_grupo_membro_pessoa_aprovado_por_pessoa_id",
                        column: x => x.aprovado_por_pessoa_id,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_grupo_membro_pessoa_pessoa_id",
                        column: x => x.pessoa_id,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grupo_moderador",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grupo_id = table.Column<long>(type: "bigint", nullable: false),
                    pessoa_id = table.Column<long>(type: "bigint", nullable: false),
                    papel = table.Column<string>(type: "text", nullable: false),
                    data_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grupo_moderador", x => x.id);
                    table.ForeignKey(
                        name: "FK_grupo_moderador_grupo_grupo_id",
                        column: x => x.grupo_id,
                        principalTable: "grupo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_grupo_moderador_pessoa_pessoa_id",
                        column: x => x.pessoa_id,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "participacao_evento",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    pessoa_id = table.Column<long>(type: "bigint", nullable: false),
                    status_participacao = table.Column<string>(type: "text", nullable: false),
                    observacao = table.Column<string>(type: "text", nullable: true),
                    data_resposta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_participacao_evento", x => x.id);
                    table.ForeignKey(
                        name: "FK_participacao_evento_evento_evento_id",
                        column: x => x.evento_id,
                        principalTable: "evento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_participacao_evento_pessoa_pessoa_id",
                        column: x => x.pessoa_id,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    autor_pessoa_id = table.Column<long>(type: "bigint", nullable: false),
                    grupo_id = table.Column<long>(type: "bigint", nullable: true),
                    evento_id = table.Column<long>(type: "bigint", nullable: true),
                    titulo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    texto = table.Column<string>(type: "text", nullable: false),
                    visibilidade = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Ativo"),
                    fixado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    data_publicacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp"),
                    data_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post", x => x.id);
                    table.ForeignKey(
                        name: "FK_post_evento_evento_id",
                        column: x => x.evento_id,
                        principalTable: "evento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_post_grupo_grupo_id",
                        column: x => x.grupo_id,
                        principalTable: "grupo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_post_pessoa_autor_pessoa_id",
                        column: x => x.autor_pessoa_id,
                        principalTable: "pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_amizade_destino",
                table: "amizade",
                column: "pessoa_destino_id");

            migrationBuilder.CreateIndex(
                name: "ix_amizade_status",
                table: "amizade",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "uq_amizade_origem_destino",
                table: "amizade",
                columns: new[] { "pessoa_origem_id", "pessoa_destino_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_denuncia_alvo",
                table: "denuncia",
                columns: new[] { "tipo_alvo", "alvo_id" });

            migrationBuilder.CreateIndex(
                name: "ix_denuncia_denunciante",
                table: "denuncia",
                column: "denunciante_pessoa_id");

            migrationBuilder.CreateIndex(
                name: "IX_denuncia_resolvido_por_admin_id",
                table: "denuncia",
                column: "resolvido_por_admin_id");

            migrationBuilder.CreateIndex(
                name: "ix_denuncia_status",
                table: "denuncia",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_evento_criado_por",
                table: "evento",
                column: "criado_por_pessoa_id");

            migrationBuilder.CreateIndex(
                name: "ix_evento_data_inicio",
                table: "evento",
                column: "data_hora_inicio");

            migrationBuilder.CreateIndex(
                name: "ix_evento_grupo",
                table: "evento",
                column: "grupo_id");

            migrationBuilder.CreateIndex(
                name: "ix_evento_latitude_longitude",
                table: "evento",
                columns: new[] { "latitude", "longitude" });

            migrationBuilder.CreateIndex(
                name: "ix_evento_privacidade",
                table: "evento",
                column: "privacidade");

            migrationBuilder.CreateIndex(
                name: "ix_evento_status",
                table: "evento",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_evento_tipo",
                table: "evento",
                column: "tipo_evento");

            migrationBuilder.CreateIndex(
                name: "ix_grupo_criado_por",
                table: "grupo",
                column: "criado_por_pessoa_id");

            migrationBuilder.CreateIndex(
                name: "ix_grupo_data_criacao",
                table: "grupo",
                column: "data_criacao",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "ix_grupo_status",
                table: "grupo",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_grupo_tipo_privacidade",
                table: "grupo",
                column: "tipo_privacidade");

            migrationBuilder.CreateIndex(
                name: "ix_grupo_tipo_status",
                table: "grupo",
                columns: new[] { "tipo_privacidade", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_grupo_membro_aprovado_por_pessoa_id",
                table: "grupo_membro",
                column: "aprovado_por_pessoa_id");

            migrationBuilder.CreateIndex(
                name: "ix_grupo_membro_pessoa",
                table: "grupo_membro",
                column: "pessoa_id");

            migrationBuilder.CreateIndex(
                name: "uq_grupo_membro",
                table: "grupo_membro",
                columns: new[] { "grupo_id", "pessoa_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_grupo_moderador_pessoa",
                table: "grupo_moderador",
                column: "pessoa_id");

            migrationBuilder.CreateIndex(
                name: "uq_grupo_moderador",
                table: "grupo_moderador",
                columns: new[] { "grupo_id", "pessoa_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_login_social_pessoa",
                table: "login_social",
                column: "pessoa_id");

            migrationBuilder.CreateIndex(
                name: "uq_login_social_provider_user",
                table: "login_social",
                columns: new[] { "provider", "provider_user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_midia_dono",
                table: "midia",
                columns: new[] { "tipo_dono", "dono_id" });

            migrationBuilder.CreateIndex(
                name: "ix_midia_tipo_arquivo",
                table: "midia",
                column: "tipo_arquivo");

            migrationBuilder.CreateIndex(
                name: "ix_notificacao_pessoa",
                table: "notificacao",
                column: "pessoa_id");

            migrationBuilder.CreateIndex(
                name: "ix_notificacao_pessoa_lida",
                table: "notificacao",
                columns: new[] { "pessoa_id", "lida" });

            migrationBuilder.CreateIndex(
                name: "ix_participacao_evento_pessoa",
                table: "participacao_evento",
                column: "pessoa_id");

            migrationBuilder.CreateIndex(
                name: "uq_participacao_evento",
                table: "participacao_evento",
                columns: new[] { "evento_id", "pessoa_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_pessoa_nome",
                table: "pessoa",
                column: "nome");

            migrationBuilder.CreateIndex(
                name: "ix_pessoa_role",
                table: "pessoa",
                column: "role");

            migrationBuilder.CreateIndex(
                name: "ix_pessoa_status_conta",
                table: "pessoa",
                column: "status_conta");

            migrationBuilder.CreateIndex(
                name: "uq_pessoa_apelido",
                table: "pessoa",
                column: "apelido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_pessoa_email",
                table: "pessoa",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_post_autor",
                table: "post",
                column: "autor_pessoa_id");

            migrationBuilder.CreateIndex(
                name: "ix_post_data_publicacao",
                table: "post",
                column: "data_publicacao",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "ix_post_evento",
                table: "post",
                column: "evento_id");

            migrationBuilder.CreateIndex(
                name: "ix_post_grupo",
                table: "post",
                column: "grupo_id");

            migrationBuilder.CreateIndex(
                name: "ix_post_status",
                table: "post",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_post_visibilidade",
                table: "post",
                column: "visibilidade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "amizade");

            migrationBuilder.DropTable(
                name: "denuncia");

            migrationBuilder.DropTable(
                name: "EntidadeTeste");

            migrationBuilder.DropTable(
                name: "grupo_membro");

            migrationBuilder.DropTable(
                name: "grupo_moderador");

            migrationBuilder.DropTable(
                name: "login_social");

            migrationBuilder.DropTable(
                name: "midia");

            migrationBuilder.DropTable(
                name: "notificacao");

            migrationBuilder.DropTable(
                name: "participacao_evento");

            migrationBuilder.DropTable(
                name: "post");

            migrationBuilder.DropTable(
                name: "evento");

            migrationBuilder.DropTable(
                name: "grupo");

            migrationBuilder.DropTable(
                name: "pessoa");
        }
    }
}
