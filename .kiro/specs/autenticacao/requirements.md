# Requirements Document - Autenticação SocialX

## Introduction

Este documento especifica os requisitos para o sistema de autenticação da plataforma SocialX. O sistema permitirá que usuários façam login usando suas contas Google através do Google Sign-In nativo, gerenciem sessões de forma segura e acessem recursos protegidos da aplicação. A autenticação é fundamental para identificar usuários, proteger dados pessoais e habilitar funcionalidades sociais personalizadas.

## Glossary

- **Authentication_System**: Sistema responsável por autenticar usuários e gerenciar sessões
- **Google_Sign_In_Service**: Serviço que integra com a API nativa do Google Sign-In
- **Token_Manager**: Componente que gerencia tokens de autenticação (ID token e access token)
- **Session_Manager**: Componente que gerencia o estado de autenticação do usuário
- **Backend_Auth_Service**: Serviço no backend .NET que valida tokens e gerencia usuários
- **User_Profile**: Perfil do usuário contendo informações básicas (id, nome, email, foto)
- **ID_Token**: Token JWT fornecido pelo Google contendo informações do usuário
- **Access_Token**: Token OAuth2 para acessar APIs do Google
- **Auth_State**: Estado de autenticação contendo informações sobre login, usuário e erros
- **Protected_Resource**: Recurso da API que requer autenticação válida
- **Pessoa**: Entidade do banco de dados representando um usuário do sistema com dados pessoais completos
- **LoginSocial**: Entidade do banco de dados que vincula uma Pessoa a um provedor de autenticação social (Google)
- **Status_Conta**: Estado da conta do usuário (ATIVA, BLOQUEADA, DESATIVADA)
- **Provider**: Provedor de autenticação social (atualmente apenas GOOGLE)
- **Provider_User_Id**: Identificador único do usuário no provedor de autenticação (Google ID)
- **Completar_Cadastro_Screen**: Tela onde usuário de primeiro acesso preenche dados obrigatórios
- **Editar_Perfil_Screen**: Tela onde usuário autenticado pode editar seus dados pessoais
- **Home_Feed_Screen**: Tela inicial do aplicativo após autenticação bem-sucedida
- **Ultimo_Acesso**: Data e hora do último acesso bem-sucedido do usuário ao sistema

## Requirements

### Requirement 1: Autenticação com Google Sign-In

**User Story:** Como um usuário do aplicativo, eu quero fazer login usando minha conta Google, para que eu possa acessar a plataforma SocialX de forma rápida e segura sem criar uma nova senha.

#### Acceptance Criteria

1. THE Google_Sign_In_Service SHALL configure the native Google Sign-In SDK with the web client ID at application startup
2. WHEN the user taps the login button, THE Google_Sign_In_Service SHALL present the native Google account picker
3. WHEN the user selects a Google account and grants permissions, THE Google_Sign_In_Service SHALL return an ID_Token and user profile information within 5 seconds
4. WHEN the user cancels the Google account picker, THE Authentication_System SHALL return to the login screen without displaying an error message
5. IF the Google Sign-In fails due to network error, THEN THE Authentication_System SHALL display a descriptive error message to the user
6. THE Google_Sign_In_Service SHALL request profile and email scopes from Google

### Requirement 2: Validação de Token no Backend

**User Story:** Como desenvolvedor do sistema, eu quero validar tokens do Google no backend, para que apenas usuários autenticados possam acessar recursos protegidos.

#### Acceptance Criteria

1. WHEN the frontend sends an ID_Token to the backend, THE Backend_Auth_Service SHALL validate the token signature using Google's public keys
2. WHEN the ID_Token is valid, THE Backend_Auth_Service SHALL extract user information (id, email, name, photo) from the token
3. IF the ID_Token is expired, THEN THE Backend_Auth_Service SHALL return an HTTP 401 Unauthorized status
4. IF the ID_Token signature is invalid, THEN THE Backend_Auth_Service SHALL return an HTTP 401 Unauthorized status with a descriptive error message
5. THE Backend_Auth_Service SHALL verify that the token audience matches the configured web client ID

### Requirement 3: Estrutura de Dados de Usuário

**User Story:** Como desenvolvedor do sistema, eu quero uma estrutura de dados bem definida para armazenar informações de usuários, para que o sistema possa gerenciar perfis completos e vínculos com provedores de autenticação.

#### Acceptance Criteria

1. THE Backend_Auth_Service SHALL persist Pessoa records with fields: id, nome, apelido, telefone, data_nascimento, email, foto_perfil, bio, cidade, role, status_conta, data_criacao, and ultimo_acesso
2. THE Backend_Auth_Service SHALL persist LoginSocial records with fields: id, pessoa_id, provider, provider_user_id, email_provider, and data_vinculo
3. THE Backend_Auth_Service SHALL enforce that apelido, telefone, and data_nascimento are required fields in Pessoa
4. THE Backend_Auth_Service SHALL enforce that nome and email are required fields in Pessoa
5. THE Backend_Auth_Service SHALL set role to USUARIO by default when creating new Pessoa records
6. THE Backend_Auth_Service SHALL set status_conta to ATIVA by default when creating new Pessoa records
7. THE Backend_Auth_Service SHALL set data_criacao to the current timestamp when creating new Pessoa records
8. THE Backend_Auth_Service SHALL enforce a foreign key relationship between LoginSocial.pessoa_id and Pessoa.id
9. THE Backend_Auth_Service SHALL enforce uniqueness on the combination of LoginSocial.provider and LoginSocial.provider_user_id
10. THE Backend_Auth_Service SHALL enforce uniqueness on Pessoa.email

### Requirement 4: Verificação de Vínculo com LoginSocial

**User Story:** Como desenvolvedor do sistema, eu quero verificar se o usuário já possui cadastro vinculado ao Google, para que eu possa direcionar usuários novos para completar cadastro e usuários existentes para o sistema.

#### Acceptance Criteria

1. WHEN the ID_Token is validated successfully, THE Backend_Auth_Service SHALL check if a LoginSocial record exists with provider equals GOOGLE and provider_user_id matching the Google ID
2. WHEN a LoginSocial record is found, THE Backend_Auth_Service SHALL retrieve the associated Pessoa record
3. WHEN no LoginSocial record is found, THE Backend_Auth_Service SHALL return a response indicating that the user needs to complete registration
4. THE Backend_Auth_Service SHALL return the Pessoa data and authentication status to the frontend

### Requirement 5: Verificação de Status da Conta

**User Story:** Como administrador do sistema, eu quero controlar o acesso de usuários através do status da conta, para que eu possa bloquear ou desativar contas quando necessário.

#### Acceptance Criteria

1. WHEN a Pessoa record is retrieved after LoginSocial verification, THE Backend_Auth_Service SHALL check the Status_Conta field
2. WHEN Status_Conta equals ATIVA, THE Backend_Auth_Service SHALL allow authentication to proceed
3. WHEN Status_Conta equals BLOQUEADA, THE Backend_Auth_Service SHALL return HTTP 403 Forbidden with message "Sua conta está bloqueada. Entre em contato com o suporte."
4. WHEN Status_Conta equals DESATIVADA, THE Backend_Auth_Service SHALL return HTTP 403 Forbidden with message "Sua conta está desativada."
5. THE Backend_Auth_Service SHALL log all blocked and deactivated account access attempts

### Requirement 6: Atualização de Último Acesso

**User Story:** Como desenvolvedor do sistema, eu quero registrar quando usuários acessam o sistema, para que eu possa monitorar atividade e identificar contas inativas.

#### Acceptance Criteria

1. WHEN a user with Status_Conta equals ATIVA successfully authenticates, THE Backend_Auth_Service SHALL update the Ultimo_Acesso field with the current timestamp
2. THE Backend_Auth_Service SHALL update Ultimo_Acesso before returning the authentication response to the frontend
3. THE Backend_Auth_Service SHALL persist the Ultimo_Acesso update to the database within 1 second

### Requirement 7: Completar Cadastro (Primeiro Acesso)

**User Story:** Como um usuário que está acessando o aplicativo pela primeira vez, eu quero completar meu cadastro com informações adicionais, para que eu possa ter um perfil completo no sistema.

#### Acceptance Criteria

1. WHEN the Backend_Auth_Service indicates that no LoginSocial exists for the user, THE Authentication_System SHALL redirect to the Completar_Cadastro_Screen
2. THE Completar_Cadastro_Screen SHALL display nome, email, and foto_perfil fields pre-filled with data from Google and disabled for editing
3. THE Completar_Cadastro_Screen SHALL require the user to fill apelido, telefone, and data_nascimento fields before submission
4. WHEN the user submits the form with all required fields, THE Backend_Auth_Service SHALL validate that apelido, telefone, and data_nascimento are present
5. WHEN the user submits the form, THE Backend_Auth_Service SHALL validate that telefone matches a valid phone format
6. WHEN the user submits the form, THE Backend_Auth_Service SHALL validate that data_nascimento is a valid date
7. WHEN validation succeeds, THE Backend_Auth_Service SHALL create a new Pessoa record with role equals USUARIO and Status_Conta equals ATIVA
8. WHEN the Pessoa record is created, THE Backend_Auth_Service SHALL create a LoginSocial record linking the Pessoa to the Google provider
9. WHEN the Pessoa and LoginSocial records are created, THE Backend_Auth_Service SHALL set Ultimo_Acesso to the current timestamp
10. WHEN registration completes successfully, THE Authentication_System SHALL authenticate the user and redirect to the Home_Feed_Screen
11. IF any required field is missing, THEN THE Completar_Cadastro_Screen SHALL display validation errors for each missing field
12. IF telefone format is invalid, THEN THE Completar_Cadastro_Screen SHALL display the message "Formato de telefone inválido"
13. IF data_nascimento is invalid, THEN THE Completar_Cadastro_Screen SHALL display the message "Data de nascimento inválida"

### Requirement 8: Editar Perfil

**User Story:** Como um usuário autenticado, eu quero editar meus dados pessoais, para que eu possa manter minhas informações atualizadas no sistema.

#### Acceptance Criteria

1. WHEN an authenticated user accesses the Editar_Perfil_Screen, THE Authentication_System SHALL load the current Pessoa data
2. THE Editar_Perfil_Screen SHALL allow editing of apelido, telefone, data_nascimento, foto_perfil, bio, and cidade fields
3. THE Editar_Perfil_Screen SHALL display nome and email fields as read-only (disabled for editing)
4. WHEN the user submits changes, THE Backend_Auth_Service SHALL validate that required fields (apelido, telefone, data_nascimento) are present
5. WHEN the user submits changes, THE Backend_Auth_Service SHALL validate telefone format
6. WHEN the user submits changes, THE Backend_Auth_Service SHALL validate data_nascimento is a valid date
7. WHEN validation succeeds, THE Backend_Auth_Service SHALL update the Pessoa record with the new values
8. WHEN the update succeeds, THE Editar_Perfil_Screen SHALL display a success message and update the displayed profile data
9. IF validation fails, THEN THE Editar_Perfil_Screen SHALL display appropriate error messages for each invalid field
10. THE Backend_Auth_Service SHALL persist profile updates to the database within 2 seconds

### Requirement 9: Redirecionamento Pós-Autenticação

**User Story:** Como um usuário do aplicativo, eu quero ser direcionado para a tela apropriada após autenticação, para que eu tenha uma experiência fluida baseado no meu status de cadastro.

#### Acceptance Criteria

1. WHEN authentication with Google succeeds and LoginSocial exists and Status_Conta equals ATIVA, THE Authentication_System SHALL redirect to the Home_Feed_Screen
2. WHEN authentication with Google succeeds and no LoginSocial exists, THE Authentication_System SHALL redirect to the Completar_Cadastro_Screen
3. WHEN authentication with Google succeeds and Status_Conta equals BLOQUEADA, THE Authentication_System SHALL display the blocking message and remain on the login screen
4. WHEN authentication with Google succeeds and Status_Conta equals DESATIVADA, THE Authentication_System SHALL display the deactivation message and remain on the login screen
5. THE Authentication_System SHALL complete the redirection within 1 second of receiving the backend response

### Requirement 10: Gerenciamento de Sessão

**User Story:** Como um usuário autenticado, eu quero que minha sessão seja mantida entre aberturas do aplicativo, para que eu não precise fazer login toda vez que abrir o app.

#### Acceptance Criteria

1. WHEN the user successfully authenticates, THE Session_Manager SHALL persist the Auth_State locally on the device
2. WHEN the application starts, THE Session_Manager SHALL check if a valid session exists
3. WHILE a valid session exists, THE Authentication_System SHALL automatically restore the user's Auth_State
4. WHEN the stored ID_Token is expired, THE Session_Manager SHALL attempt to silently refresh the authentication
5. IF silent refresh fails, THEN THE Session_Manager SHALL clear the session and redirect the user to the login screen
6. THE Session_Manager SHALL encrypt sensitive authentication data before persisting it locally

### Requirement 11: Logout

**User Story:** Como um usuário autenticado, eu quero fazer logout da minha conta, para que outras pessoas não possam acessar meus dados no mesmo dispositivo.

#### Acceptance Criteria

1. WHEN the user taps the logout button, THE Authentication_System SHALL revoke the Google Sign-In session
2. WHEN logout is initiated, THE Session_Manager SHALL clear all locally stored authentication data
3. WHEN logout is initiated, THE Session_Manager SHALL clear the Auth_State
4. WHEN logout completes, THE Authentication_System SHALL redirect the user to the login screen
5. THE Authentication_System SHALL complete the logout process within 2 seconds

### Requirement 12: Proteção de Recursos da API

**User Story:** Como desenvolvedor do sistema, eu quero proteger endpoints da API, para que apenas usuários autenticados possam acessá-los.

#### Acceptance Criteria

1. WHEN a request is made to a Protected_Resource, THE Backend_Auth_Service SHALL verify the presence of a valid ID_Token in the Authorization header
2. IF no ID_Token is present in the request, THEN THE Backend_Auth_Service SHALL return HTTP 401 Unauthorized
3. IF the ID_Token is invalid or expired, THEN THE Backend_Auth_Service SHALL return HTTP 401 Unauthorized
4. WHEN the ID_Token is valid, THE Backend_Auth_Service SHALL extract the user ID and make it available to the endpoint handler
5. THE Backend_Auth_Service SHALL validate the token on every request to a Protected_Resource

### Requirement 13: Tratamento de Erros de Autenticação

**User Story:** Como um usuário do aplicativo, eu quero receber mensagens claras quando algo der errado na autenticação, para que eu saiba como resolver o problema.

#### Acceptance Criteria

1. WHEN a network error occurs during authentication, THE Authentication_System SHALL display the message "Erro de conexão. Verifique sua internet e tente novamente"
2. WHEN Google Sign-In is not properly configured, THE Authentication_System SHALL display the message "Erro de configuração. Contate o suporte"
3. WHEN the backend returns an error, THE Authentication_System SHALL display a user-friendly error message based on the error type
4. WHEN an unexpected error occurs, THE Authentication_System SHALL log the error details and display a generic error message to the user
5. THE Authentication_System SHALL clear the loading state after displaying any error message

### Requirement 14: Exibição de Perfil do Usuário

**User Story:** Como um usuário autenticado, eu quero ver minhas informações de perfil após o login, para que eu confirme que estou logado com a conta correta.

#### Acceptance Criteria

1. WHEN authentication succeeds, THE Authentication_System SHALL display the User_Profile containing name, email, and photo
2. WHEN the User_Profile photo URL is available, THE Authentication_System SHALL load and display the profile image
3. IF the profile photo fails to load, THEN THE Authentication_System SHALL display a default avatar placeholder
4. THE Authentication_System SHALL display the user's full name, given name, and family name from the User_Profile
5. THE Authentication_System SHALL update the UI to reflect the authenticated state within 500 milliseconds of successful login

### Requirement 15: Verificação de Estado de Autenticação

**User Story:** Como desenvolvedor do sistema, eu quero verificar o estado de autenticação antes de acessar recursos protegidos, para que o aplicativo funcione corretamente em todas as telas.

#### Acceptance Criteria

1. THE Session_Manager SHALL provide a method to check if the user is currently authenticated
2. THE Session_Manager SHALL provide a method to retrieve the current User_Profile
3. WHEN the Auth_State changes, THE Session_Manager SHALL notify all registered observers
4. THE Session_Manager SHALL expose the Auth_State as an observable stream for reactive components
5. WHEN a component mounts, THE Authentication_System SHALL provide the current Auth_State synchronously

### Requirement 16: Integração com Tipos TypeScript

**User Story:** Como desenvolvedor frontend, eu quero interfaces TypeScript bem definidas para autenticação, para que o código seja type-safe e previna erros em tempo de compilação.

#### Acceptance Criteria

1. THE Authentication_System SHALL define an AuthState interface containing isAuthenticated, user, loading, and error properties
2. THE Authentication_System SHALL define a UserProfile interface containing id, email, name, givenName, familyName, and photo properties
3. THE Authentication_System SHALL define a Pessoa interface containing id, nome, apelido, telefone, data_nascimento, email, foto_perfil, bio, cidade, role, status_conta, data_criacao, and ultimo_acesso properties
4. THE Authentication_System SHALL define a LoginSocial interface containing id, pessoa_id, provider, provider_user_id, email_provider, and data_vinculo properties
5. THE Authentication_System SHALL define a StatusConta enum with values ATIVA, BLOQUEADA, and DESATIVADA
6. THE Authentication_System SHALL define a Provider enum with value GOOGLE
7. THE Authentication_System SHALL define a GoogleSignInResult discriminated union type for success, cancel, and error cases
8. THE Authentication_System SHALL define all service methods with explicit return types
9. THE Authentication_System SHALL use strict TypeScript mode with no any types

### Requirement 17: Testes Automatizados de Autenticação

**User Story:** Como desenvolvedor do sistema, eu quero testes automatizados para o fluxo de autenticação, para que eu possa garantir que mudanças futuras não quebrem a funcionalidade.

#### Acceptance Criteria

1. THE Authentication_System SHALL include unit tests for the Google_Sign_In_Service covering success, cancel, and error scenarios
2. THE Authentication_System SHALL include unit tests for the Session_Manager covering persistence and restoration of Auth_State
3. THE Authentication_System SHALL include component tests for the login screen verifying button interactions and state updates
4. THE Authentication_System SHALL include component tests for the Completar_Cadastro_Screen verifying form validation and submission
5. THE Authentication_System SHALL include component tests for the Editar_Perfil_Screen verifying field editing and validation
6. THE Authentication_System SHALL include integration tests mocking the Google Sign-In SDK responses
7. THE Authentication_System SHALL include integration tests for the LoginSocial verification flow
8. THE Authentication_System SHALL include integration tests for Status_Conta verification (ATIVA, BLOQUEADA, DESATIVADA)
9. THE Authentication_System SHALL achieve at least 80% code coverage for authentication-related code
10. FOR ALL valid User_Profile objects, THE Authentication_System SHALL verify that serializing then deserializing produces an equivalent object (round-trip property)
11. FOR ALL valid Pessoa objects, THE Authentication_System SHALL verify that serializing then deserializing produces an equivalent object (round-trip property)

