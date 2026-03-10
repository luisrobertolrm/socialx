# Design Document - Autenticação SocialX

## Overview

O sistema de autenticação do SocialX implementa um fluxo completo de login social usando Google Sign-In, com gerenciamento de sessão, perfis de usuário e controle de acesso. A arquitetura é dividida em duas camadas principais:

**Backend (.NET)**: Responsável por validar tokens do Google, gerenciar entidades de usuário (Pessoa e LoginSocial), controlar status de contas e proteger recursos da API.

**Frontend (React Native + TypeScript)**: Responsável por integrar com o Google Sign-In SDK nativo, gerenciar estado de autenticação local, apresentar telas de login/cadastro/perfil e comunicar com o backend.

O sistema suporta três fluxos principais:
1. **Login de usuário existente**: Validação de token → Verificação de LoginSocial → Verificação de status → Redirecionamento para Home
2. **Primeiro acesso**: Validação de token → Completar cadastro → Criação de Pessoa + LoginSocial → Redirecionamento para Home
3. **Edição de perfil**: Usuário autenticado atualiza dados pessoais

## Architecture

### Backend Architecture (.NET)

```
┌─────────────────────────────────────────────────────────────┐
│                      API Layer                               │
│  ┌──────────────────────────────────────────────────────┐   │
│  │           AuthController                              │   │
│  │  - POST /api/auth/google                             │   │
│  │  - POST /api/auth/completar-cadastro                 │   │
│  │  - PUT /api/auth/perfil                              │   │
│  │  - GET /api/auth/perfil                              │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                   Service Layer                              │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  GoogleTokenValidator                                 │   │
│  │  - ValidateTokenAsync(idToken)                       │   │
│  │  - ExtractUserInfo(validatedToken)                   │   │
│  └──────────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  AuthService                                          │   │
│  │  - VerifyLoginSocialAsync(googleId)                  │   │
│  │  - CheckAccountStatusAsync(pessoaId)                 │   │
│  │  - UpdateLastAccessAsync(pessoaId)                   │   │
│  │  - CreateUserAsync(completarCadastroDto)             │   │
│  │  - UpdateProfileAsync(pessoaId, updateDto)           │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                   Data Layer                                 │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  DbContext                                            │   │
│  │  - DbSet<Pessoa>                                      │   │
│  │  - DbSet<LoginSocial>                                 │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                   Database (PostgreSQL)                      │
│  - Tabela: pessoa                                            │
│  - Tabela: login_social                                      │
└─────────────────────────────────────────────────────────────┘
```

**Middleware de Autenticação**: Intercepta requisições para endpoints protegidos, valida o ID Token no header Authorization e extrai o user ID para uso nos controllers.

### Frontend Architecture (React Native)

```
┌─────────────────────────────────────────────────────────────┐
│                    Screen Layer                              │
│  - LoginScreen                                               │
│  - CompletarCadastroScreen                                   │
│  - EditarPerfilScreen                                        │
│  - HomeScreen / FeedScreen                                   │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                 State Management                             │
│  - AuthContext (React Context)                               │
│  - AuthState: { isAuthenticated, user, loading, error }     │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                   Service Layer                              │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  googleAuthService                                    │   │
│  │  - signIn(): Promise<GoogleSignInResult>             │   │
│  │  - signOut(): Promise<void>                          │   │
│  │  - silentSignIn(): Promise<GoogleSignInResult>       │   │
│  └──────────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  authApiService                                       │   │
│  │  - validateGoogleToken(idToken)                      │   │
│  │  - completarCadastro(dto)                            │   │
│  │  - updateProfile(dto)                                │   │
│  │  - getProfile()                                      │   │
│  └──────────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  sessionService                                       │   │
│  │  - saveSession(authState)                            │   │
│  │  - loadSession(): Promise<AuthState | null>         │   │
│  │  - clearSession()                                    │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│              External Dependencies                           │
│  - @react-native-google-signin/google-signin                │
│  - @react-native-async-storage/async-storage                │
│  - react-native-encrypted-storage                           │
└─────────────────────────────────────────────────────────────┘
```

### Communication Flow

```
[Frontend]                    [Backend]                [Google]
    │                             │                        │
    │  1. User taps login         │                        │
    │─────────────────────────────┼───────────────────────>│
    │                             │   Google Sign-In SDK   │
    │<────────────────────────────┼────────────────────────│
    │  2. ID Token + User Info    │                        │
    │                             │                        │
    │  3. POST /api/auth/google   │                        │
    │────────────────────────────>│                        │
    │                             │  4. Validate token     │
    │                             │───────────────────────>│
    │                             │<───────────────────────│
    │                             │  5. Token valid        │
    │                             │                        │
    │                             │  6. Check LoginSocial  │
    │                             │  7. Check Status       │
    │                             │  8. Update LastAccess  │
    │                             │                        │
    │  9. Response: User + Status │                        │
    │<────────────────────────────│                        │
    │                             │                        │
    │  10. Navigate to Home       │                        │
    │      or CompletarCadastro   │                        │
```

## Components and Interfaces

### Backend Components (.NET)

#### AuthController

Responsável por expor endpoints REST para autenticação e gerenciamento de perfil.

**Endpoints**:

1. `POST /api/auth/google`
   - Input: `ValidateGoogleTokenRequest { IdToken: string }`
   - Output: `AuthResponse { Success: bool, RequiresRegistration: bool, User: PessoaDto?, Message: string? }`
   - Valida token do Google, verifica LoginSocial, verifica status da conta, atualiza último acesso

2. `POST /api/auth/completar-cadastro`
   - Input: `CompletarCadastroRequest { IdToken: string, Apelido: string, Telefone: string, DataNascimento: DateTime }`
   - Output: `AuthResponse { Success: bool, User: PessoaDto, Message: string? }`
   - Cria Pessoa e LoginSocial para primeiro acesso

3. `PUT /api/auth/perfil`
   - Input: `UpdateProfileRequest { Apelido: string, Telefone: string, DataNascimento: DateTime, FotoPerfil: string?, Bio: string?, Cidade: string? }`
   - Output: `UpdateProfileResponse { Success: bool, User: PessoaDto, Message: string? }`
   - Atualiza dados do perfil (requer autenticação)

4. `GET /api/auth/perfil`
   - Output: `PessoaDto`
   - Retorna dados do perfil do usuário autenticado

#### GoogleTokenValidator

Serviço responsável por validar tokens JWT do Google.

**Methods**:
- `ValidateTokenAsync(string idToken): Task<GoogleJsonWebSignature.Payload>`
  - Valida assinatura do token usando chaves públicas do Google
  - Verifica expiração
  - Verifica audience (web client ID)
  - Retorna payload com informações do usuário

- `ExtractUserInfo(GoogleJsonWebSignature.Payload payload): GoogleUserInfo`
  - Extrai id, email, name, given_name, family_name, picture do payload

#### AuthService

Serviço de lógica de negócio para autenticação.

**Methods**:
- `VerifyLoginSocialAsync(string googleId): Task<LoginSocialVerificationResult>`
  - Busca LoginSocial por provider=GOOGLE e provider_user_id=googleId
  - Retorna Pessoa associada se encontrado

- `CheckAccountStatusAsync(int pessoaId): Task<AccountStatusResult>`
  - Verifica Status_Conta da Pessoa
  - Retorna se conta está ativa, bloqueada ou desativada

- `UpdateLastAccessAsync(int pessoaId): Task`
  - Atualiza campo Ultimo_Acesso com timestamp atual

- `CreateUserAsync(CompletarCadastroDto dto): Task<Pessoa>`
  - Valida campos obrigatórios
  - Cria Pessoa com role=USUARIO, status_conta=ATIVA
  - Cria LoginSocial vinculado
  - Define Ultimo_Acesso
  - Retorna Pessoa criada

- `UpdateProfileAsync(int pessoaId, UpdateProfileDto dto): Task<Pessoa>`
  - Valida campos obrigatórios
  - Atualiza Pessoa com novos valores
  - Retorna Pessoa atualizada

### Frontend Components (React Native)

#### LoginScreen

Tela inicial de autenticação.

**Props**: Nenhuma (usa navegação)

**State**: 
- `loading: boolean` - indica se autenticação está em progresso
- `error: string | null` - mensagem de erro se houver

**Methods**:
- `handleGoogleSignIn()` - Inicia fluxo de Google Sign-In
- `handleSignInResult(result: GoogleSignInResult)` - Processa resultado do sign-in

**UI Elements**:
- Botão "Entrar com Google" com ícone do Google
- Indicador de loading durante autenticação
- Mensagem de erro se houver

#### CompletarCadastroScreen

Tela para usuários de primeiro acesso completarem cadastro.

**Props**: 
- `googleUserInfo: GoogleUserInfo` - informações do Google (nome, email, foto)
- `idToken: string` - token para enviar ao backend

**State**:
- `apelido: string`
- `telefone: string`
- `dataNascimento: Date | null`
- `errors: { apelido?: string, telefone?: string, dataNascimento?: string }`
- `loading: boolean`

**Methods**:
- `validateForm(): boolean` - Valida campos obrigatórios
- `handleSubmit()` - Envia dados para backend
- `handleDateChange(date: Date)` - Atualiza data de nascimento

**UI Elements**:
- Campo nome (disabled, pré-preenchido)
- Campo email (disabled, pré-preenchido)
- Foto de perfil (exibida, não editável)
- Campo apelido (obrigatório)
- Campo telefone (obrigatório, com máscara)
- Seletor de data de nascimento (obrigatório)
- Botão "Completar Cadastro"
- Mensagens de validação

#### EditarPerfilScreen

Tela para usuários autenticados editarem perfil.

**Props**: Nenhuma (carrega dados do contexto de autenticação)

**State**:
- `apelido: string`
- `telefone: string`
- `dataNascimento: Date`
- `fotoPerfil: string | null`
- `bio: string | null`
- `cidade: string | null`
- `errors: Record<string, string>`
- `loading: boolean`
- `successMessage: string | null`

**Methods**:
- `loadProfile()` - Carrega dados atuais do perfil
- `validateForm(): boolean` - Valida campos obrigatórios
- `handleSubmit()` - Envia atualizações para backend
- `handleImagePick()` - Permite selecionar nova foto de perfil

**UI Elements**:
- Campo nome (disabled)
- Campo email (disabled)
- Foto de perfil (editável)
- Campo apelido (editável, obrigatório)
- Campo telefone (editável, obrigatório)
- Seletor de data de nascimento (editável, obrigatório)
- Campo bio (editável, opcional)
- Campo cidade (editável, opcional)
- Botão "Salvar Alterações"
- Mensagens de validação e sucesso

#### Services

##### googleAuthService

Integração com Google Sign-In SDK nativo.

```typescript
interface GoogleAuthService {
  configure(webClientId: string): void;
  signIn(): Promise<GoogleSignInResult>;
  signOut(): Promise<void>;
  silentSignIn(): Promise<GoogleSignInResult>;
}

type GoogleSignInResult = 
  | { type: 'success', idToken: string, user: GoogleUserInfo }
  | { type: 'cancel' }
  | { type: 'error', error: string };

interface GoogleUserInfo {
  id: string;
  email: string;
  name: string;
  givenName: string;
  familyName: string;
  photo: string | null;
}
```

##### authApiService

Comunicação com backend de autenticação.

```typescript
interface AuthApiService {
  validateGoogleToken(idToken: string): Promise<AuthResponse>;
  completarCadastro(dto: CompletarCadastroRequest): Promise<AuthResponse>;
  updateProfile(dto: UpdateProfileRequest): Promise<UpdateProfileResponse>;
  getProfile(): Promise<PessoaDto>;
}

interface AuthResponse {
  success: boolean;
  requiresRegistration: boolean;
  user: PessoaDto | null;
  message: string | null;
}

interface CompletarCadastroRequest {
  idToken: string;
  apelido: string;
  telefone: string;
  dataNascimento: string; // ISO 8601 format
}

interface UpdateProfileRequest {
  apelido: string;
  telefone: string;
  dataNascimento: string;
  fotoPerfil?: string;
  bio?: string;
  cidade?: string;
}

interface UpdateProfileResponse {
  success: boolean;
  user: PessoaDto;
  message: string | null;
}
```

##### sessionService

Gerenciamento de sessão local com criptografia.

```typescript
interface SessionService {
  saveSession(authState: AuthState): Promise<void>;
  loadSession(): Promise<AuthState | null>;
  clearSession(): Promise<void>;
  isSessionValid(): Promise<boolean>;
}
```

## Data Models

### Backend Entities (.NET)

#### Pessoa

Entidade principal representando um usuário do sistema.

```csharp
public class Pessoa
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Apelido { get; set; }
    
    [Required]
    [Phone]
    public string Telefone { get; set; }
    
    [Required]
    public DateTime DataNascimento { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [MaxLength(500)]
    public string? FotoPerfil { get; set; }
    
    [MaxLength(500)]
    public string? Bio { get; set; }
    
    [MaxLength(100)]
    public string? Cidade { get; set; }
    
    [Required]
    public Role Role { get; set; } = Role.USUARIO;
    
    [Required]
    public StatusConta StatusConta { get; set; } = StatusConta.ATIVA;
    
    [Required]
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    
    public DateTime? UltimoAcesso { get; set; }
    
    // Navigation property
    public ICollection<LoginSocial> LoginsSociais { get; set; }
}

public enum Role
{
    USUARIO,
    MODERADOR,
    ADMIN
}

public enum StatusConta
{
    ATIVA,
    BLOQUEADA,
    DESATIVADA
}
```

#### LoginSocial

Entidade que vincula uma Pessoa a um provedor de autenticação social.

```csharp
public class LoginSocial
{
    public int Id { get; set; }
    
    [Required]
    public int PessoaId { get; set; }
    
    [Required]
    public Provider Provider { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string ProviderUserId { get; set; }
    
    [Required]
    [EmailAddress]
    public string EmailProvider { get; set; }
    
    [Required]
    public DateTime DataVinculo { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public Pessoa Pessoa { get; set; }
}

public enum Provider
{
    GOOGLE
}
```

**Database Constraints**:
- Unique constraint on `(Provider, ProviderUserId)`
- Unique constraint on `Pessoa.Email`
- Foreign key `LoginSocial.PessoaId` references `Pessoa.Id` with cascade delete

#### DTOs

```csharp
public class PessoaDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Apelido { get; set; }
    public string Telefone { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Email { get; set; }
    public string? FotoPerfil { get; set; }
    public string? Bio { get; set; }
    public string? Cidade { get; set; }
    public Role Role { get; set; }
    public StatusConta StatusConta { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? UltimoAcesso { get; set; }
}

public class ValidateGoogleTokenRequest
{
    [Required]
    public string IdToken { get; set; }
}

public class CompletarCadastroRequest
{
    [Required]
    public string IdToken { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Apelido { get; set; }
    
    [Required]
    [Phone]
    public string Telefone { get; set; }
    
    [Required]
    public DateTime DataNascimento { get; set; }
}

public class UpdateProfileRequest
{
    [Required]
    [MaxLength(50)]
    public string Apelido { get; set; }
    
    [Required]
    [Phone]
    public string Telefone { get; set; }
    
    [Required]
    public DateTime DataNascimento { get; set; }
    
    [MaxLength(500)]
    public string? FotoPerfil { get; set; }
    
    [MaxLength(500)]
    public string? Bio { get; set; }
    
    [MaxLength(100)]
    public string? Cidade { get; set; }
}
```

### Frontend Types (TypeScript)

```typescript
interface AuthState {
  isAuthenticated: boolean;
  user: Pessoa | null;
  loading: boolean;
  error: string | null;
}

interface Pessoa {
  id: number;
  nome: string;
  apelido: string;
  telefone: string;
  dataNascimento: string; // ISO 8601
  email: string;
  fotoPerfil: string | null;
  bio: string | null;
  cidade: string | null;
  role: Role;
  statusConta: StatusConta;
  dataCriacao: string; // ISO 8601
  ultimoAcesso: string | null; // ISO 8601
}

interface LoginSocial {
  id: number;
  pessoaId: number;
  provider: Provider;
  providerUserId: string;
  emailProvider: string;
  dataVinculo: string; // ISO 8601
}

enum StatusConta {
  ATIVA = 'ATIVA',
  BLOQUEADA = 'BLOQUEADA',
  DESATIVADA = 'DESATIVADA'
}

enum Provider {
  GOOGLE = 'GOOGLE'
}

enum Role {
  USUARIO = 'USUARIO',
  MODERADOR = 'MODERADOR',
  ADMIN = 'ADMIN'
}

interface GoogleUserInfo {
  id: string;
  email: string;
  name: string;
  givenName: string;
  familyName: string;
  photo: string | null;
}

type GoogleSignInResult = 
  | { type: 'success'; idToken: string; user: GoogleUserInfo }
  | { type: 'cancel' }
  | { type: 'error'; error: string };
```


## Correctness Properties

*A property is a characteristic or behavior that should hold true across all valid executions of a system-essentially, a formal statement about what the system should do. Properties serve as the bridge between human-readable specifications and machine-verifiable correctness guarantees.*

### Property 1: Token Validation Completeness

*For any* ID Token received from the frontend, the Backend_Auth_Service SHALL validate the token signature using Google's public keys, verify the token is not expired, and verify the audience matches the configured web client ID before accepting it as valid.

**Validates: Requirements 2.1, 2.5**

### Property 2: User Information Extraction

*For any* valid ID Token, the Backend_Auth_Service SHALL successfully extract user information including id, email, name, and photo from the token payload.

**Validates: Requirements 2.2**

### Property 3: Pessoa Required Fields Validation

*For any* attempt to create or update a Pessoa record, the Backend_Auth_Service SHALL enforce that nome, email, apelido, telefone, and data_nascimento fields are present and non-empty, rejecting requests that lack any of these required fields.

**Validates: Requirements 3.3, 3.4, 7.4, 8.4**

### Property 4: Field Format Validation

*For any* Pessoa record being created or updated, the Backend_Auth_Service SHALL validate that telefone matches a valid phone format and data_nascimento is a valid date, rejecting records with invalid formats.

**Validates: Requirements 7.5, 7.6, 8.5, 8.6**

### Property 5: Pessoa Default Values

*For any* new Pessoa record created during registration, the Backend_Auth_Service SHALL automatically set role to USUARIO, status_conta to ATIVA, data_criacao to the current timestamp, and ultimo_acesso to the current timestamp if not explicitly provided.

**Validates: Requirements 3.5, 3.6, 3.7, 7.7, 7.9**

### Property 6: Database Constraint Enforcement

*For any* attempt to create a LoginSocial record, the system SHALL enforce: (1) foreign key constraint that pessoa_id references a valid Pessoa.id, (2) uniqueness constraint on the combination of provider and provider_user_id, and (3) uniqueness constraint on Pessoa.email.

**Validates: Requirements 3.8, 3.9, 3.10**

### Property 7: LoginSocial Verification and Pessoa Retrieval

*For any* validated ID Token with a Google ID, the Backend_Auth_Service SHALL check if a LoginSocial record exists with provider=GOOGLE and provider_user_id matching the Google ID, and if found, SHALL retrieve the associated Pessoa record.

**Validates: Requirements 4.1, 4.2**

### Property 8: Registration Required Response

*For any* validated ID Token that does not have an associated LoginSocial record, the Backend_Auth_Service SHALL return a response with requiresRegistration=true and user=null.

**Validates: Requirements 4.3**

### Property 9: Account Status-Based Access Control

*For any* Pessoa record retrieved during authentication, the Backend_Auth_Service SHALL check status_conta and: (1) allow authentication to proceed if ATIVA, (2) return HTTP 403 with message "Sua conta está bloqueada. Entre em contato com o suporte." if BLOQUEADA, (3) return HTTP 403 with message "Sua conta está desativada." if DESATIVADA.

**Validates: Requirements 5.1, 5.2, 5.3, 5.4**

### Property 10: Blocked Account Logging

*For any* authentication attempt where status_conta is BLOQUEADA or DESATIVADA, the Backend_Auth_Service SHALL create a log entry recording the pessoa_id, status, and timestamp of the blocked access attempt.

**Validates: Requirements 5.5**

### Property 11: Last Access Update on Successful Authentication

*For any* user with status_conta=ATIVA who successfully authenticates, the Backend_Auth_Service SHALL update the ultimo_acesso field with the current timestamp before returning the authentication response.

**Validates: Requirements 6.1, 6.2**

### Property 12: Complete Registration Flow

*For any* valid CompletarCadastroRequest with all required fields, the Backend_Auth_Service SHALL create both a Pessoa record and a LoginSocial record linking the Pessoa to the Google provider, ensuring both records are persisted atomically.

**Validates: Requirements 7.7, 7.8**

### Property 13: Registration Validation Error Messages

*For any* CompletarCadastroRequest with missing required fields, invalid telefone format, or invalid data_nascimento, the system SHALL return specific validation error messages: "Campo obrigatório" for missing fields, "Formato de telefone inválido" for invalid phone, and "Data de nascimento inválida" for invalid date.

**Validates: Requirements 7.11, 7.12, 7.13**

### Property 14: Profile Update Persistence

*For any* valid UpdateProfileRequest from an authenticated user, the Backend_Auth_Service SHALL update the Pessoa record with the new values for apelido, telefone, data_nascimento, foto_perfil, bio, and cidade, while preserving nome and email as read-only.

**Validates: Requirements 8.2, 8.3, 8.7**

### Property 15: Navigation Based on Authentication State

*For any* authentication response, the Authentication_System SHALL navigate to: (1) Home_Feed_Screen if LoginSocial exists and status_conta=ATIVA, (2) Completar_Cadastro_Screen if requiresRegistration=true, (3) remain on login screen with blocking message if status_conta=BLOQUEADA, (4) remain on login screen with deactivation message if status_conta=DESATIVADA.

**Validates: Requirements 9.1, 9.2, 9.3, 9.4**

### Property 16: Session Persistence and Restoration

*For any* successful authentication, the Session_Manager SHALL persist the Auth_State locally with encryption, and upon application restart, SHALL check for a valid session and automatically restore the Auth_State if the session is valid.

**Validates: Requirements 10.1, 10.2, 10.3, 10.6**

### Property 17: Session Refresh on Token Expiration

*For any* stored session with an expired ID Token, the Session_Manager SHALL attempt to silently refresh the authentication, and if refresh fails, SHALL clear the session and redirect to the login screen.

**Validates: Requirements 10.4, 10.5**

### Property 18: Complete Logout Cleanup

*For any* logout action, the Authentication_System SHALL: (1) revoke the Google Sign-In session, (2) clear all locally stored authentication data, (3) clear the Auth_State, and (4) redirect to the login screen.

**Validates: Requirements 11.1, 11.2, 11.3, 11.4**

### Property 19: Protected Resource Authorization

*For any* request to a protected resource, the Backend_Auth_Service SHALL verify the presence of a valid ID Token in the Authorization header, return HTTP 401 if no token is present or if the token is invalid/expired, and extract the user ID for the endpoint handler if the token is valid.

**Validates: Requirements 12.1, 12.2, 12.3, 12.4, 12.5**

### Property 20: Error Message Mapping

*For any* error during authentication, the Authentication_System SHALL display appropriate user-friendly messages: "Erro de conexão. Verifique sua internet e tente novamente" for network errors, "Erro de configuração. Contate o suporte" for configuration errors, and SHALL log unexpected errors while displaying a generic message to the user, always clearing the loading state after displaying the error.

**Validates: Requirements 13.1, 13.2, 13.3, 13.4, 13.5**

### Property 21: Profile Display After Authentication

*For any* successful authentication, the Authentication_System SHALL display the User_Profile containing name, email, and photo, loading the profile image if a photo URL is available, or displaying a default avatar placeholder if the photo fails to load.

**Validates: Requirements 14.1, 14.2, 14.3, 14.4**

### Property 22: Session Manager API Completeness

*For any* Session_Manager implementation, it SHALL provide methods to: (1) check if the user is currently authenticated, (2) retrieve the current User_Profile, (3) notify all registered observers when Auth_State changes, (4) expose Auth_State as an observable stream, and (5) provide current Auth_State synchronously when components mount.

**Validates: Requirements 15.1, 15.2, 15.3, 15.4, 15.5**

### Property 23: TypeScript Interface Completeness

*For any* TypeScript implementation of the Authentication_System, it SHALL define interfaces with all required properties: AuthState (isAuthenticated, user, loading, error), UserProfile (id, email, name, givenName, familyName, photo), Pessoa (id, nome, apelido, telefone, data_nascimento, email, foto_perfil, bio, cidade, role, status_conta, data_criacao, ultimo_acesso), LoginSocial (id, pessoa_id, provider, provider_user_id, email_provider, data_vinculo), StatusConta enum (ATIVA, BLOQUEADA, DESATIVADA), Provider enum (GOOGLE), and GoogleSignInResult discriminated union.

**Validates: Requirements 16.1, 16.2, 16.3, 16.4, 16.5, 16.6, 16.7**

### Property 24: TypeScript Strict Mode Compliance

*For any* service method in the Authentication_System, it SHALL have an explicit return type with no use of the any type, enforcing TypeScript strict mode throughout the codebase.

**Validates: Requirements 16.8, 16.9**

### Property 25: UserProfile Serialization Round-Trip

*For any* valid UserProfile object, serializing it to JSON and then deserializing it back SHALL produce an equivalent UserProfile object with all properties preserved.

**Validates: Requirements 17.10**

### Property 26: Pessoa Serialization Round-Trip

*For any* valid Pessoa object, serializing it to JSON and then deserializing it back SHALL produce an equivalent Pessoa object with all properties preserved.

**Validates: Requirements 17.11**

## Error Handling

### Backend Error Handling (.NET)

**Token Validation Errors**:
- Invalid signature → HTTP 401 with message "Token inválido"
- Expired token → HTTP 401 with message "Token expirado"
- Invalid audience → HTTP 401 with message "Token inválido"
- Missing token → HTTP 401 with message "Token não fornecido"

**Account Status Errors**:
- BLOQUEADA → HTTP 403 with message "Sua conta está bloqueada. Entre em contato com o suporte."
- DESATIVADA → HTTP 403 with message "Sua conta está desativada."

**Validation Errors**:
- Missing required fields → HTTP 400 with field-specific messages
- Invalid phone format → HTTP 400 with message "Formato de telefone inválido"
- Invalid date format → HTTP 400 with message "Data de nascimento inválida"
- Duplicate email → HTTP 409 with message "Email já cadastrado"
- Duplicate LoginSocial → HTTP 409 with message "Conta Google já vinculada a outro usuário"

**Database Errors**:
- Foreign key violation → HTTP 400 with message "Dados inválidos"
- Connection errors → HTTP 503 with message "Serviço temporariamente indisponível"

**Google API Errors**:
- Token validation failure → HTTP 401 with message "Falha ao validar token do Google"
- Network timeout → HTTP 504 with message "Timeout ao validar token"

### Frontend Error Handling (React Native)

**Google Sign-In Errors**:
- User cancellation → No error message, return to login screen
- Network error → Display "Erro de conexão. Verifique sua internet e tente novamente"
- Configuration error → Display "Erro de configuração. Contate o suporte"
- Unknown error → Log error, display "Erro inesperado. Tente novamente"

**API Communication Errors**:
- Network timeout → Display "Erro de conexão. Verifique sua internet e tente novamente"
- HTTP 401 → Clear session, redirect to login
- HTTP 403 → Display message from backend (account blocked/deactivated)
- HTTP 400 → Display validation errors from backend
- HTTP 409 → Display conflict message from backend
- HTTP 5xx → Display "Erro no servidor. Tente novamente mais tarde"

**Session Errors**:
- Corrupted session data → Clear session, redirect to login
- Decryption failure → Clear session, redirect to login
- Expired session with failed refresh → Clear session, redirect to login

**Form Validation Errors**:
- Empty required field → Display "Campo obrigatório"
- Invalid phone format → Display "Formato de telefone inválido"
- Invalid date → Display "Data de nascimento inválida"
- Future date of birth → Display "Data de nascimento não pode ser no futuro"

**Error Recovery**:
- All errors SHALL clear loading state
- Network errors SHALL allow retry
- Validation errors SHALL preserve user input
- Session errors SHALL clear sensitive data

## Testing Strategy

### Dual Testing Approach

The authentication system requires both unit tests and property-based tests for comprehensive coverage:

**Unit Tests**: Focus on specific examples, edge cases, and integration points
**Property Tests**: Verify universal properties across all inputs through randomization

### Backend Testing (.NET)

**Property-Based Testing Library**: FsCheck for .NET

**Property Test Configuration**:
- Minimum 100 iterations per property test
- Each test tagged with: **Feature: autenticacao, Property {number}: {property_text}**

**Property Tests to Implement**:

1. **Property 1: Token Validation Completeness**
   - Generate random valid and invalid tokens
   - Verify signature validation, expiration check, and audience verification
   - Tag: **Feature: autenticacao, Property 1: Token Validation Completeness**

2. **Property 3: Pessoa Required Fields Validation**
   - Generate Pessoa objects with various combinations of missing fields
   - Verify all required fields are enforced
   - Tag: **Feature: autenticacao, Property 3: Pessoa Required Fields Validation**

3. **Property 4: Field Format Validation**
   - Generate random phone numbers and dates (valid and invalid)
   - Verify format validation works correctly
   - Tag: **Feature: autenticacao, Property 4: Field Format Validation**

4. **Property 5: Pessoa Default Values**
   - Generate random Pessoa creation requests
   - Verify defaults are always set correctly
   - Tag: **Feature: autenticacao, Property 5: Pessoa Default Values**

5. **Property 6: Database Constraint Enforcement**
   - Generate random LoginSocial records with invalid foreign keys and duplicates
   - Verify constraints are enforced
   - Tag: **Feature: autenticacao, Property 6: Database Constraint Enforcement**

6. **Property 9: Account Status-Based Access Control**
   - Generate Pessoa records with all possible status_conta values
   - Verify correct HTTP status and messages for each
   - Tag: **Feature: autenticacao, Property 9: Account Status-Based Access Control**

7. **Property 11: Last Access Update on Successful Authentication**
   - Generate random successful authentication scenarios
   - Verify ultimo_acesso is always updated
   - Tag: **Feature: autenticacao, Property 11: Last Access Update on Successful Authentication**

8. **Property 19: Protected Resource Authorization**
   - Generate requests with various token states (missing, invalid, expired, valid)
   - Verify correct authorization behavior
   - Tag: **Feature: autenticacao, Property 19: Protected Resource Authorization**

9. **Property 26: Pessoa Serialization Round-Trip**
   - Generate random Pessoa objects
   - Verify serialize → deserialize produces equivalent object
   - Tag: **Feature: autenticacao, Property 26: Pessoa Serialization Round-Trip**

**Unit Tests to Implement**:

- GoogleTokenValidator with mocked Google API responses
- AuthService methods with in-memory database
- AuthController endpoints with mocked services
- Middleware authentication with various token scenarios
- Edge cases: empty strings, null values, boundary dates
- Integration tests for complete authentication flows

### Frontend Testing (React Native)

**Property-Based Testing Library**: fast-check for TypeScript/JavaScript

**Property Test Configuration**:
- Minimum 100 iterations per property test
- Each test tagged with: **Feature: autenticacao, Property {number}: {property_text}**

**Property Tests to Implement**:

1. **Property 15: Navigation Based on Authentication State**
   - Generate random authentication responses with different states
   - Verify correct navigation for each state
   - Tag: **Feature: autenticacao, Property 15: Navigation Based on Authentication State**

2. **Property 16: Session Persistence and Restoration**
   - Generate random AuthState objects
   - Verify save → load produces equivalent state
   - Tag: **Feature: autenticacao, Property 16: Session Persistence and Restoration**

3. **Property 18: Complete Logout Cleanup**
   - Generate random authenticated states
   - Verify all cleanup steps are performed
   - Tag: **Feature: autenticacao, Property 18: Complete Logout Cleanup**

4. **Property 20: Error Message Mapping**
   - Generate random error types
   - Verify correct user-friendly messages are displayed
   - Tag: **Feature: autenticacao, Property 20: Error Message Mapping**

5. **Property 23: TypeScript Interface Completeness**
   - Generate random objects conforming to interfaces
   - Verify all required properties are present
   - Tag: **Feature: autenticacao, Property 23: TypeScript Interface Completeness**

6. **Property 24: TypeScript Strict Mode Compliance**
   - Use TypeScript compiler API to verify no any types
   - Verify all methods have explicit return types
   - Tag: **Feature: autenticacao, Property 24: TypeScript Strict Mode Compliance**

7. **Property 25: UserProfile Serialization Round-Trip**
   - Generate random UserProfile objects
   - Verify JSON.stringify → JSON.parse produces equivalent object
   - Tag: **Feature: autenticacao, Property 25: UserProfile Serialization Round-Trip**

**Unit Tests to Implement**:

- googleAuthService with mocked Google Sign-In SDK
- authApiService with mocked fetch/axios
- sessionService with mocked AsyncStorage
- LoginScreen component with user interactions
- CompletarCadastroScreen with form validation
- EditarPerfilScreen with form validation and submission
- Edge cases: network failures, cancelled sign-in, corrupted session data
- Integration tests with mocked backend responses

### Test Coverage Goals

- Minimum 80% code coverage for authentication-related code
- 100% coverage of error handling paths
- All 26 correctness properties implemented as property-based tests
- All edge cases covered by unit tests
- Integration tests for complete user flows

### Testing Best Practices

**Balance Unit and Property Tests**:
- Use property tests for universal behaviors across many inputs
- Use unit tests for specific examples and edge cases
- Avoid writing too many unit tests when property tests provide better coverage

**Property Test Generators**:
- Create custom generators for domain objects (Pessoa, LoginSocial, AuthState)
- Include edge cases in generators (empty strings, boundary dates, null values)
- Use shrinking to find minimal failing examples

**Mocking Strategy**:
- Mock external dependencies (Google API, database, storage)
- Use dependency injection for testability
- Verify mock interactions for critical operations

**Test Organization**:
- Group tests by component/service
- Use descriptive test names following "should" pattern
- Tag all property tests with feature name and property number
