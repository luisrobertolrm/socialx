# Entidades do Entity Framework Criadas

Baseado no schema SQL `schema_v2.sql`, foram criadas todas as classes do Entity Framework Core.

## Enums Criados (17)

Todos os enums estão em `src/SocialX.Core/Enums/`:

1. `StatusContaEnum` - Status da conta do usuário
2. `StatusAmizadeEnum` - Status da solicitação de amizade
3. `StatusDenunciaEnum` - Status da denúncia
4. `StatusGrupoMembroEnum` - Status do membro no grupo
5. `StatusGrupoEnum` - Status do grupo
6. `VisibilidadePostEnum` - Visibilidade do post
7. `TipoArquivoMidiaEnum` - Tipo de arquivo de mídia
8. `TipoPrivacidadeGrupoEnum` - Privacidade do grupo
9. `StatusEventoEnum` - Status do evento
10. `RoleUsuarioEnum` - Papel do usuário no sistema
11. `TipoEventoEnum` - Tipo de evento (presencial, online, híbrido)
12. `StatusParticipacaoEventoEnum` - Status de participação no evento
13. `TipoAlvoEnum` - Tipo de alvo da denúncia
14. `PrivacidadeEventoEnum` - Privacidade do evento
15. `TipoDonoMidiaEnum` - Tipo do dono da mídia
16. `PapelGrupoEnum` - Papel no grupo (owner, moderador)
17. `StatusPostEnum` - Status do post

## Entidades Criadas (12)

Todas as entidades estão em `src/SocialX.Core/Entidades/`:

1. **Pessoa** - Usuários da plataforma
   - Campos: Nome, Apelido, Telefone, DataNascimento, Email, FotoPerfil, Bio, Cidade, Role, StatusConta
   - Métodos: Atualizar, AtualizarUltimoAcesso, AlterarRole, AlterarStatusConta

2. **LoginSocial** - Login via provedores sociais
   - Campos: PessoaId, Provider, ProviderUserId, EmailProvider, DataVinculo

3. **Amizade** - Relacionamento de amizade entre pessoas
   - Campos: PessoaOrigemId, PessoaDestinoId, Status, DataSolicitacao, DataResposta
   - Métodos: Aceitar, Recusar, Bloquear

4. **Grupo** - Comunidades públicas ou privadas
   - Campos: Nome, Descricao, FotoCapa, FotoPerfil, TipoPrivacidade, Regras, Status, CriadoPorPessoaId
   - Métodos: Atualizar, AlterarStatus

5. **GrupoMembro** - Membros de um grupo
   - Campos: GrupoId, PessoaId, Status, DataEntrada, AprovadoPorPessoaId, DataAprovacao
   - Métodos: Aprovar, Remover, Sair, Banir

6. **GrupoModerador** - Moderadores de um grupo
   - Campos: GrupoId, PessoaId, Papel, DataInicio
   - Métodos: AlterarPapel

7. **Evento** - Eventos presenciais, online ou híbridos
   - Campos: Nome, Descricao, TipoEvento, DataHoraInicio, DataHoraFim, LocalNome, Endereco, Latitude, Longitude, LinkOnline, CapacidadeMaxima, Privacidade, Status
   - Métodos: Atualizar, AlterarStatus

8. **ParticipacaoEvento** - Participação em eventos
   - Campos: EventoId, PessoaId, StatusParticipacao, Observacao, DataResposta
   - Métodos: AtualizarStatus

9. **Post** - Publicações no perfil, grupo ou evento
   - Campos: AutorPessoaId, GrupoId, EventoId, Titulo, Texto, Visibilidade, Status, Fixado, DataPublicacao, DataEdicao
   - Métodos: Editar, AlterarStatus, Fixar, Desafixar, AlterarVisibilidade

10. **Midia** - Arquivos de imagem e vídeo
    - Campos: TipoDono, DonoId, TipoArquivo, UrlArquivo, UrlThumbnail, Ordem, MimeType, TamanhoBytes, DuracaoSegundos
    - Métodos: AtualizarMetadados, AlterarOrdem

11. **Denuncia** - Denúncias de conteúdo
    - Campos: DenunciantePessoaId, TipoAlvo, AlvoId, Motivo, Descricao, Status, ResolvidoPorAdminId, ResolvidoEm
    - Métodos: IniciarAnalise, Resolver, Rejeitar

12. **Notificacao** - Notificações do sistema
    - Campos: PessoaId, Tipo, Titulo, Mensagem, ReferenciaTipo, ReferenciaId, Lida, DataEnvio
    - Métodos: MarcarComoLida

## Configurações do Entity Framework (12)

Todas as configurações estão em `src/SocialX.Infra/Configuracoes/`:

1. `PessoaConfiguration`
2. `LoginSocialConfiguration`
3. `AmizadeConfiguration`
4. `GrupoConfiguration`
5. `GrupoMembroConfiguration`
6. `GrupoModeradorConfiguration`
7. `EventoConfiguration`
8. `ParticipacaoEventoConfiguration`
9. `PostConfiguration`
10. `MidiaConfiguration`
11. `DenunciaConfiguration`
12. `NotificacaoConfiguration`

Cada configuração inclui:
- Mapeamento de tabela e colunas
- Tipos de dados corretos
- Relacionamentos (Foreign Keys)
- Índices conforme o schema SQL
- Constraints e valores padrão

## DbContext Atualizado

O `CustomDbContext` foi atualizado com todos os DbSets:
- EntidadesTeste
- Pessoas
- LoginsSociais
- Amizades
- Grupos
- GruposMembros
- GruposModeradores
- Eventos
- ParticipacoesEventos
- Posts
- Midias
- Denuncias
- Notificacoes

## Características das Entidades

Todas as entidades seguem as regras do projeto:

1. **Construtores privados** - Setters privados para encapsulamento
2. **Métodos de negócio** - Lógica de domínio dentro das entidades
3. **Tipagem explícita** - Sem uso de `var`
4. **Navegação** - Propriedades de navegação configuradas
5. **Validações** - Constraints do banco mapeadas

## Próximos Passos

1. Criar migrations do Entity Framework:
   ```bash
   dotnet ef migrations add InitialCreate --project src/SocialX.Infra --startup-project src/SocialX.Api
   ```

2. Aplicar migrations ao banco:
   ```bash
   dotnet ef database update --project src/SocialX.Infra --startup-project src/SocialX.Api
   ```

3. Implementar os serviços e DTOs para cada entidade
4. Criar os controllers da API
5. Adicionar validadores com FluentValidation
