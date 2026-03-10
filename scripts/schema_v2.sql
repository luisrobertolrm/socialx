
-- =========================================================
-- schema_v2.sql
-- Modelo inicial do sistema de micro-posts, grupos e eventos
-- Banco: PostgreSQL
-- Versão 2: inclui papéis de sistema (role) em pessoa
-- =========================================================

create extension if not exists pg_trgm;
create extension if not exists unaccent;

do $$
begin
    if not exists (select 1 from pg_type where typname = 'tipo_evento_enum') then
        create type tipo_evento_enum as enum ('PRESENCIAL', 'ONLINE', 'HIBRIDO');
    end if;

    if not exists (select 1 from pg_type where typname = 'status_conta_enum') then
        create type status_conta_enum as enum ('ATIVA', 'BLOQUEADA', 'DESATIVADA');
    end if;

    if not exists (select 1 from pg_type where typname = 'role_usuario_enum') then
        create type role_usuario_enum as enum ('USUARIO', 'MODERADOR_SISTEMA', 'ADMIN');
    end if;

    if not exists (select 1 from pg_type where typname = 'status_amizade_enum') then
        create type status_amizade_enum as enum ('PENDENTE', 'ACEITA', 'RECUSADA', 'BLOQUEADA');
    end if;

    if not exists (select 1 from pg_type where typname = 'tipo_privacidade_grupo_enum') then
        create type tipo_privacidade_grupo_enum as enum ('PUBLICO', 'PRIVADO');
    end if;

    if not exists (select 1 from pg_type where typname = 'status_grupo_enum') then
        create type status_grupo_enum as enum ('ATIVO', 'BLOQUEADO', 'ARQUIVADO');
    end if;

    if not exists (select 1 from pg_type where typname = 'status_grupo_membro_enum') then
        create type status_grupo_membro_enum as enum ('ATIVO', 'PENDENTE_APROVACAO', 'REMOVIDO', 'SAIU', 'BANIDO');
    end if;

    if not exists (select 1 from pg_type where typname = 'papel_grupo_enum') then
        create type papel_grupo_enum as enum ('OWNER', 'MODERADOR');
    end if;

    if not exists (select 1 from pg_type where typname = 'privacidade_evento_enum') then
        create type privacidade_evento_enum as enum ('PUBLICO', 'MEMBROS_GRUPO', 'CONVIDADOS');
    end if;

    if not exists (select 1 from pg_type where typname = 'status_evento_enum') then
        create type status_evento_enum as enum ('ATIVO', 'CANCELADO', 'ENCERRADO');
    end if;

    if not exists (select 1 from pg_type where typname = 'status_participacao_evento_enum') then
        create type status_participacao_evento_enum as enum ('VAI', 'NAO_VAI', 'TALVEZ');
    end if;

    if not exists (select 1 from pg_type where typname = 'visibilidade_post_enum') then
        create type visibilidade_post_enum as enum ('PUBLICO', 'AMIGOS', 'MEMBROS_GRUPO', 'PARTICIPANTES_EVENTO');
    end if;

    if not exists (select 1 from pg_type where typname = 'status_post_enum') then
        create type status_post_enum as enum ('ATIVO', 'OCULTO_MODERACAO', 'REMOVIDO_ADMIN', 'ARQUIVADO');
    end if;

    if not exists (select 1 from pg_type where typname = 'tipo_dono_midia_enum') then
        create type tipo_dono_midia_enum as enum ('PESSOA', 'GRUPO', 'EVENTO', 'POST');
    end if;

    if not exists (select 1 from pg_type where typname = 'tipo_arquivo_midia_enum') then
        create type tipo_arquivo_midia_enum as enum ('IMAGEM', 'VIDEO');
    end if;

    if not exists (select 1 from pg_type where typname = 'tipo_alvo_enum') then
        create type tipo_alvo_enum as enum ('POST', 'GRUPO', 'EVENTO', 'PESSOA');
    end if;

    if not exists (select 1 from pg_type where typname = 'status_denuncia_enum') then
        create type status_denuncia_enum as enum ('ABERTA', 'EM_ANALISE', 'RESOLVIDA', 'REJEITADA');
    end if;
end$$;

create table if not exists pessoa (
    id bigserial primary key,
    nome varchar(150) not null,
    apelido varchar(60) not null,
    telefone varchar(20) not null,
    data_nascimento date not null,
    email varchar(150),
    foto_perfil varchar(500),
    bio text,
    cidade varchar(120),
    role role_usuario_enum not null default 'USUARIO',
    status_conta status_conta_enum not null default 'ATIVA',
    data_criacao timestamp not null default current_timestamp,
    ultimo_acesso timestamp,
    constraint uq_pessoa_apelido unique (apelido)
);

create table if not exists login_social (
    id bigserial primary key,
    pessoa_id bigint not null references pessoa(id) on delete cascade,
    provider varchar(30) not null,
    provider_user_id varchar(150) not null,
    email_provider varchar(150),
    data_vinculo timestamp not null default current_timestamp,
    constraint uq_login_social_provider_user unique (provider, provider_user_id)
);

create table if not exists amizade (
    id bigserial primary key,
    pessoa_origem_id bigint not null references pessoa(id) on delete cascade,
    pessoa_destino_id bigint not null references pessoa(id) on delete cascade,
    status status_amizade_enum not null,
    data_solicitacao timestamp not null default current_timestamp,
    data_resposta timestamp,
    constraint ck_amizade_pessoas_diferentes
        check (pessoa_origem_id <> pessoa_destino_id),
    constraint uq_amizade_origem_destino
        unique (pessoa_origem_id, pessoa_destino_id)
);

create table if not exists grupo (
    id bigserial primary key,
    nome varchar(120) not null,
    descricao text,
    foto_capa varchar(500),
    foto_perfil varchar(500),
    tipo_privacidade tipo_privacidade_grupo_enum not null,
    regras text,
    status status_grupo_enum not null default 'ATIVO',
    criado_por_pessoa_id bigint not null references pessoa(id),
    data_criacao timestamp not null default current_timestamp
);

create table if not exists grupo_membro (
    id bigserial primary key,
    grupo_id bigint not null references grupo(id) on delete cascade,
    pessoa_id bigint not null references pessoa(id) on delete cascade,
    status status_grupo_membro_enum not null,
    data_entrada timestamp not null default current_timestamp,
    aprovado_por_pessoa_id bigint references pessoa(id),
    data_aprovacao timestamp,
    constraint uq_grupo_membro unique (grupo_id, pessoa_id)
);

create table if not exists grupo_moderador (
    id bigserial primary key,
    grupo_id bigint not null references grupo(id) on delete cascade,
    pessoa_id bigint not null references pessoa(id) on delete cascade,
    papel papel_grupo_enum not null,
    data_inicio timestamp not null default current_timestamp,
    constraint uq_grupo_moderador unique (grupo_id, pessoa_id)
);

create table if not exists evento (
    id bigserial primary key,
    grupo_id bigint references grupo(id) on delete set null,
    criado_por_pessoa_id bigint not null references pessoa(id),
    nome varchar(150) not null,
    descricao text not null,
    tipo_evento tipo_evento_enum not null default 'PRESENCIAL',
    data_hora_inicio timestamp not null,
    data_hora_fim timestamp,
    local_nome varchar(150),
    endereco text,
    latitude numeric(9,6),
    longitude numeric(9,6),
    link_online varchar(500),
    capacidade_maxima integer,
    privacidade privacidade_evento_enum not null,
    status status_evento_enum not null default 'ATIVO',
    data_criacao timestamp not null default current_timestamp,
    constraint ck_evento_data_fim
        check (data_hora_fim is null or data_hora_fim >= data_hora_inicio),
    constraint ck_evento_capacidade
        check (capacidade_maxima is null or capacidade_maxima > 0),
    constraint ck_evento_latitude
        check (latitude is null or (latitude >= -90 and latitude <= 90)),
    constraint ck_evento_longitude
        check (longitude is null or (longitude >= -180 and longitude <= 180)),
    constraint ck_evento_tipo_local
        check (
            (
                tipo_evento = 'PRESENCIAL'
                and latitude is not null
                and longitude is not null
            )
            or
            (
                tipo_evento = 'ONLINE'
                and link_online is not null
                and length(trim(link_online)) > 0
            )
            or
            (
                tipo_evento = 'HIBRIDO'
                and latitude is not null
                and longitude is not null
                and link_online is not null
                and length(trim(link_online)) > 0
            )
        )
);

create table if not exists participacao_evento (
    id bigserial primary key,
    evento_id bigint not null references evento(id) on delete cascade,
    pessoa_id bigint not null references pessoa(id) on delete cascade,
    status_participacao status_participacao_evento_enum not null,
    observacao text,
    data_resposta timestamp not null default current_timestamp,
    constraint uq_participacao_evento unique (evento_id, pessoa_id)
);

create table if not exists post (
    id bigserial primary key,
    autor_pessoa_id bigint not null references pessoa(id),
    grupo_id bigint references grupo(id) on delete set null,
    evento_id bigint references evento(id) on delete set null,
    titulo varchar(150),
    texto text not null,
    visibilidade visibilidade_post_enum not null,
    status status_post_enum not null default 'ATIVO',
    fixado boolean not null default false,
    data_publicacao timestamp not null default current_timestamp,
    data_edicao timestamp
);

create table if not exists midia (
    id bigserial primary key,
    tipo_dono tipo_dono_midia_enum not null,
    dono_id bigint not null,
    tipo_arquivo tipo_arquivo_midia_enum not null,
    url_arquivo varchar(500) not null,
    url_thumbnail varchar(500),
    ordem integer not null default 0,
    mime_type varchar(100),
    tamanho_bytes bigint,
    duracao_segundos integer,
    data_upload timestamp not null default current_timestamp,
    constraint ck_midia_ordem check (ordem >= 0),
    constraint ck_midia_tamanho check (tamanho_bytes is null or tamanho_bytes >= 0),
    constraint ck_midia_duracao check (duracao_segundos is null or duracao_segundos >= 0)
);

create table if not exists denuncia (
    id bigserial primary key,
    denunciante_pessoa_id bigint not null references pessoa(id),
    tipo_alvo tipo_alvo_enum not null,
    alvo_id bigint not null,
    motivo varchar(100) not null,
    descricao text,
    status status_denuncia_enum not null default 'ABERTA',
    criado_em timestamp not null default current_timestamp,
    resolvido_por_admin_id bigint references pessoa(id),
    resolvido_em timestamp
);

create table if not exists notificacao (
    id bigserial primary key,
    pessoa_id bigint not null references pessoa(id) on delete cascade,
    tipo varchar(50) not null,
    titulo varchar(150) not null,
    mensagem text not null,
    referencia_tipo varchar(30),
    referencia_id bigint,
    lida boolean not null default false,
    data_envio timestamp not null default current_timestamp
);

-- índices principais
create index if not exists ix_pessoa_nome on pessoa (nome);
create index if not exists ix_pessoa_status_conta on pessoa (status_conta);
create index if not exists ix_pessoa_role on pessoa (role);

create index if not exists ix_login_social_pessoa on login_social (pessoa_id);

create index if not exists ix_amizade_destino on amizade (pessoa_destino_id);
create index if not exists ix_amizade_status on amizade (status);
create index if not exists ix_amizade_origem_status on amizade (pessoa_origem_id, status);
create index if not exists ix_amizade_destino_status on amizade (pessoa_destino_id, status);

create index if not exists ix_grupo_criado_por on grupo (criado_por_pessoa_id);
create index if not exists ix_grupo_tipo_privacidade on grupo (tipo_privacidade);
create index if not exists ix_grupo_status on grupo (status);
create index if not exists ix_grupo_data_criacao on grupo (data_criacao desc);
create index if not exists ix_grupo_tipo_status on grupo (tipo_privacidade, status);

create index if not exists ix_grupo_membro_pessoa on grupo_membro (pessoa_id);
create index if not exists ix_grupo_membro_grupo_status on grupo_membro (grupo_id, status);
create index if not exists ix_grupo_membro_pessoa_status on grupo_membro (pessoa_id, status);
create index if not exists ix_grupo_membro_aprovado_por on grupo_membro (aprovado_por_pessoa_id);

create index if not exists ix_grupo_moderador_pessoa on grupo_moderador (pessoa_id);
create index if not exists ix_grupo_moderador_grupo_papel on grupo_moderador (grupo_id, papel);

create index if not exists ix_evento_grupo on evento (grupo_id);
create index if not exists ix_evento_criado_por on evento (criado_por_pessoa_id);
create index if not exists ix_evento_data_inicio on evento (data_hora_inicio);
create index if not exists ix_evento_status on evento (status);
create index if not exists ix_evento_privacidade on evento (privacidade);
create index if not exists ix_evento_tipo on evento (tipo_evento);
create index if not exists ix_evento_grupo_data_inicio on evento (grupo_id, data_hora_inicio desc);
create index if not exists ix_evento_status_data_inicio on evento (status, data_hora_inicio);
create index if not exists ix_evento_tipo_status_data on evento (tipo_evento, status, data_hora_inicio);
create index if not exists ix_evento_latitude_longitude on evento (latitude, longitude);

create index if not exists ix_participacao_evento_pessoa on participacao_evento (pessoa_id);
create index if not exists ix_participacao_evento_evento_status on participacao_evento (evento_id, status_participacao);
create index if not exists ix_participacao_evento_pessoa_status on participacao_evento (pessoa_id, status_participacao);

create index if not exists ix_post_autor on post (autor_pessoa_id);
create index if not exists ix_post_grupo on post (grupo_id);
create index if not exists ix_post_evento on post (evento_id);
create index if not exists ix_post_status on post (status);
create index if not exists ix_post_visibilidade on post (visibilidade);
create index if not exists ix_post_data_publicacao on post (data_publicacao desc);
create index if not exists ix_post_autor_data on post (autor_pessoa_id, data_publicacao desc);
create index if not exists ix_post_grupo_data on post (grupo_id, data_publicacao desc);
create index if not exists ix_post_evento_data on post (evento_id, data_publicacao desc);
create index if not exists ix_post_status_visibilidade_data on post (status, visibilidade, data_publicacao desc);
create index if not exists ix_post_grupo_status_data on post (grupo_id, status, data_publicacao desc);
create index if not exists ix_post_evento_status_data on post (evento_id, status, data_publicacao desc);
create index if not exists ix_post_autor_status_data on post (autor_pessoa_id, status, data_publicacao desc);

create index if not exists ix_post_feed_publico_ativo
    on post (data_publicacao desc)
    where status = 'ATIVO' and visibilidade = 'PUBLICO';

create index if not exists ix_post_feed_grupo_ativo
    on post (grupo_id, data_publicacao desc)
    where status = 'ATIVO';

create index if not exists ix_post_feed_evento_ativo
    on post (evento_id, data_publicacao desc)
    where status = 'ATIVO';

create index if not exists ix_midia_dono on midia (tipo_dono, dono_id);
create index if not exists ix_midia_tipo_arquivo on midia (tipo_arquivo);

create index if not exists ix_denuncia_denunciante on denuncia (denunciante_pessoa_id);
create index if not exists ix_denuncia_status on denuncia (status);
create index if not exists ix_denuncia_alvo on denuncia (tipo_alvo, alvo_id);
create index if not exists ix_denuncia_status_criado_em on denuncia (status, criado_em desc);
create index if not exists ix_denuncia_resolvido_por on denuncia (resolvido_por_admin_id);

create index if not exists ix_notificacao_pessoa on notificacao (pessoa_id);
create index if not exists ix_notificacao_pessoa_lida on notificacao (pessoa_id, lida);
create index if not exists ix_notificacao_pessoa_data on notificacao (pessoa_id, data_envio desc);
create index if not exists ix_notificacao_pessoa_lida_data on notificacao (pessoa_id, lida, data_envio desc);

create index if not exists ix_pessoa_nome_trgm on pessoa using gin (nome gin_trgm_ops);
create index if not exists ix_pessoa_apelido_trgm on pessoa using gin (apelido gin_trgm_ops);
create index if not exists ix_grupo_nome_trgm on grupo using gin (nome gin_trgm_ops);
create index if not exists ix_grupo_descricao_trgm on grupo using gin (descricao gin_trgm_ops);
create index if not exists ix_evento_nome_trgm on evento using gin (nome gin_trgm_ops);
create index if not exists ix_evento_descricao_trgm on evento using gin (descricao gin_trgm_ops);
create index if not exists ix_post_titulo_trgm on post using gin (titulo gin_trgm_ops);
create index if not exists ix_post_texto_trgm on post using gin (texto gin_trgm_ops);

create index if not exists ix_post_busca_fts
    on post using gin (
        to_tsvector('portuguese', coalesce(titulo, '') || ' ' || coalesce(texto, ''))
    );

create index if not exists ix_grupo_busca_fts
    on grupo using gin (
        to_tsvector('portuguese', coalesce(nome, '') || ' ' || coalesce(descricao, ''))
    );

create index if not exists ix_evento_busca_fts
    on evento using gin (
        to_tsvector('portuguese', coalesce(nome, '') || ' ' || coalesce(descricao, ''))
    );

comment on table pessoa is 'Usuários da plataforma';
comment on column pessoa.role is 'Papel global no sistema: usuário, moderador do sistema ou admin';
comment on table grupo is 'Comunidades públicas ou privadas';
comment on table evento is 'Eventos presenciais, online ou híbridos';
comment on table post is 'Publicações no perfil, grupo ou evento';
comment on table midia is 'Arquivos de imagem e vídeo associados a entidades do sistema';
