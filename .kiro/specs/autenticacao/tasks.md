# Implementation Plan: Autenticação SocialX

## Overview

Este plano implementa o sistema completo de autenticação usando Google Sign-In, com backend em .NET 10 e frontend em React Native + TypeScript. A implementação segue uma abordagem incremental, construindo primeiro a infraestrutura de dados, depois os serviços de autenticação, e finalmente as telas e integração.

## Tasks

- [ ] 1. Configurar infraestrutura de dados no backend
  - [x] 1.1 Criar entidades Pessoa e LoginSocial no SocialX.Core
    - Criar enums Role, StatusConta e Provider
    - Definir entidade Pessoa com todos os campos obrigatórios
    - Definir entidade LoginSocial com relacionamento para Pessoa
    - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7, 3.8, 3.9, 3.10_
  
  - [ ]* 1.2 Escrever testes de propriedade para validação de campos obrigatórios
    - **Property 3: Pessoa Required Fields Validation**
    - **Validates: Requirements 3.3, 3.4, 7.4, 8.4**
  
  - [ ]* 1.3 Escrever testes de propriedade para valores padrão
    - **Property 5: Pessoa Default Values**
    - **Validates: Requirements 3.5, 3.6, 3.7, 7.7, 7.9**
  
  - [x] 1.4 Criar configurações do Entity Framework para Pessoa e LoginSocial
    - Configurar mapeamento de tabelas e colunas
    - Definir constraints de unicidade e foreign keys
    - Configurar tamanhos máximos de campos
    - _Requirements: 3.8, 3.9, 3.10_
  
  - [ ]* 1.5 Escrever testes de propriedade para constraints do banco
    - **Property 6: Database Constraint Enforcement**
    - **Validates: Requirements 3.8, 3.9, 3.10**
  
  - [x] 1.6 Criar e aplicar migrations para Pessoa e LoginSocial
    - Gerar migration inicial
    - Aplicar migration no banco de dados
    - _Requirements: 3.1, 3.2_

- [ ] 2. Checkpoint - Validar estrutura de dados
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 3. Implementar validação de token do Google
  - [x] 3.1 Criar GoogleTokenValidator service no SocialX.Service
    - Implementar ValidateTokenAsync usando Google.Apis.Auth
    - Implementar ExtractUserInfo para extrair dados do payload
    - Configurar validação de audience e issuer
    - _Requirements: 2.1, 2.2, 2.5_
  
  - [ ]* 3.2 Escrever testes de propriedade para validação de token
    - **Property 1: Token Validation Completeness**
    - **Validates: Requirements 2.1, 2.5**
  
  - [ ]* 3.3 Escrever testes de propriedade para extração de informações
    - **Property 2: User Information Extraction**
    - **Validates: Requirements 2.2**
  
  - [ ]* 3.4 Escrever testes unitários para GoogleTokenValidator
    - Testar token válido
    - Testar token expirado
    - Testar token com assinatura inválida
    - Testar token com audience incorreto
    - _Requirements: 2.3, 2.4_

- [ ] 4. Implementar AuthService no backend
  - [x] 4.1 Criar interface IAuthService e implementação AuthService
    - Implementar VerifyLoginSocialAsync
    - Implementar CheckAccountStatusAsync
    - Implementar UpdateLastAccessAsync
    - Implementar CreateUserAsync
    - Implementar UpdateProfileAsync
    - _Requirements: 4.1, 4.2, 4.3, 5.1, 5.2, 5.3, 6.1, 7.7, 7.8, 8.7_
  
  - [ ]* 4.2 Escrever testes de propriedade para verificação de LoginSocial
    - **Property 7: LoginSocial Verification and Pessoa Retrieval**
    - **Validates: Requirements 4.1, 4.2**
  
  - [ ]* 4.3 Escrever testes de propriedade para resposta de registro necessário
    - **Property 8: Registration Required Response**
    - **Validates: Requirements 4.3**
  
  - [ ]* 4.4 Escrever testes de propriedade para controle de acesso por status
    - **Property 9: Account Status-Based Access Control**
    - **Validates: Requirements 5.1, 5.2, 5.3, 5.4**
  
  - [ ]* 4.5 Escrever testes de propriedade para logging de contas bloqueadas
    - **Property 10: Blocked Account Logging**
    - **Validates: Requirements 5.5**
  
  - [ ]* 4.6 Escrever testes de propriedade para atualização de último acesso
    - **Property 11: Last Access Update on Successful Authentication**
    - **Validates: Requirements 6.1, 6.2**
  
  - [ ]* 4.7 Escrever testes de propriedade para fluxo completo de registro
    - **Property 12: Complete Registration Flow**
    - **Validates: Requirements 7.7, 7.8**
  
  - [ ]* 4.8 Escrever testes de propriedade para atualização de perfil
    - **Property 14: Profile Update Persistence**
    - **Validates: Requirements 8.2, 8.3, 8.7**
  
  - [ ]* 4.9 Escrever testes unitários para AuthService
    - Testar cada método com cenários válidos e inválidos
    - Usar banco de dados em memória
    - _Requirements: 4.1, 4.2, 4.3, 5.1, 5.2, 5.3, 6.1, 7.7, 7.8, 8.7_

- [ ] 5. Criar DTOs e validadores
  - [x] 5.1 Criar DTOs no SocialX.Service
    - Criar PessoaDto
    - Criar ValidateGoogleTokenRequest
    - Criar CompletarCadastroRequest
    - Criar UpdateProfileRequest
    - Criar AuthResponse
    - Criar UpdateProfileResponse
    - _Requirements: 2.1, 7.4, 8.4_
  
  - [x] 5.2 Criar validadores FluentValidation
    - Criar CompletarCadastroRequestValidator
    - Criar UpdateProfileRequestValidator
    - Validar formatos de telefone e data
    - _Requirements: 7.5, 7.6, 8.5, 8.6_
  
  - [ ]* 5.3 Escrever testes de propriedade para validação de formato
    - **Property 4: Field Format Validation**
    - **Validates: Requirements 7.5, 7.6, 8.5, 8.6**
  
  - [ ]* 5.4 Escrever testes de propriedade para mensagens de erro de validação
    - **Property 13: Registration Validation Error Messages**
    - **Validates: Requirements 7.11, 7.12, 7.13**
  
  - [ ]* 5.5 Escrever testes unitários para validadores
    - Testar cada regra de validação
    - Verificar mensagens de erro
    - _Requirements: 7.11, 7.12, 7.13_
  
  - [x] 5.6 Criar AutoMapper Profile para mapeamento Pessoa <-> PessoaDto
    - Configurar mapeamento bidirecional
    - _Requirements: 3.1_

- [ ] 6. Checkpoint - Validar serviços e DTOs
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 7. Implementar AuthController
  - [ ] 7.1 Criar AuthController no SocialX.Api
    - Implementar POST /api/auth/google
    - Implementar POST /api/auth/completar-cadastro
    - Implementar PUT /api/auth/perfil
    - Implementar GET /api/auth/perfil
    - _Requirements: 2.1, 4.4, 7.10, 8.8, 9.1, 9.2_
  
  - [ ]* 7.2 Escrever testes unitários para AuthController
    - Testar cada endpoint com cenários válidos e inválidos
    - Mockar AuthService e GoogleTokenValidator
    - Verificar códigos de status HTTP corretos
    - _Requirements: 2.3, 2.4, 5.3, 5.4_

- [ ] 8. Implementar middleware de autenticação
  - [ ] 8.1 Criar middleware de autenticação JWT
    - Validar token no header Authorization
    - Extrair user ID do token
    - Disponibilizar user ID para controllers
    - _Requirements: 12.1, 12.2, 12.3, 12.4, 12.5_
  
  - [ ]* 8.2 Escrever testes de propriedade para proteção de recursos
    - **Property 19: Protected Resource Authorization**
    - **Validates: Requirements 12.1, 12.2, 12.3, 12.4, 12.5**
  
  - [ ]* 8.3 Escrever testes unitários para middleware
    - Testar com token ausente
    - Testar com token inválido
    - Testar com token expirado
    - Testar com token válido
    - _Requirements: 12.1, 12.2, 12.3_

- [ ] 9. Configurar injeção de dependências e pipeline
  - [ ] 9.1 Registrar serviços no Program.cs
    - Registrar GoogleTokenValidator
    - Registrar AuthService
    - Registrar validadores FluentValidation
    - Registrar AutoMapper
    - Configurar middleware de autenticação
    - Configurar tratamento global de exceções
    - _Requirements: 2.1, 4.1, 7.4, 8.4_
  
  - [ ] 9.2 Configurar appsettings.json
    - Adicionar Google Client ID
    - Configurar connection string do PostgreSQL
    - _Requirements: 1.1, 2.5_

- [ ] 10. Checkpoint - Validar backend completo
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 11. Criar interfaces TypeScript no frontend
  - [ ] 11.1 Criar tipos e interfaces em types/auth.ts
    - Criar interface AuthState
    - Criar interface Pessoa
    - Criar interface LoginSocial
    - Criar interface GoogleUserInfo
    - Criar enums StatusConta, Provider, Role
    - Criar type GoogleSignInResult (discriminated union)
    - _Requirements: 16.1, 16.2, 16.3, 16.4, 16.5, 16.6, 16.7_
  
  - [ ]* 11.2 Escrever testes de propriedade para completude de interfaces
    - **Property 23: TypeScript Interface Completeness**
    - **Validates: Requirements 16.1, 16.2, 16.3, 16.4, 16.5, 16.6, 16.7**
  
  - [ ]* 11.3 Escrever testes de propriedade para strict mode
    - **Property 24: TypeScript Strict Mode Compliance**
    - **Validates: Requirements 16.8, 16.9**
  
  - [ ]* 11.4 Escrever testes de propriedade para serialização de UserProfile
    - **Property 25: UserProfile Serialization Round-Trip**
    - **Validates: Requirements 17.10**
  
  - [ ]* 11.5 Escrever testes de propriedade para serialização de Pessoa
    - **Property 26: Pessoa Serialization Round-Trip**
    - **Validates: Requirements 17.11**

- [ ] 12. Implementar googleAuthService
  - [ ] 12.1 Criar googleAuthService em services/googleAuthService.ts
    - Implementar configure com webClientId
    - Implementar signIn usando @react-native-google-signin/google-signin
    - Implementar signOut
    - Implementar silentSignIn
    - _Requirements: 1.1, 1.2, 1.3, 11.1, 11.2_
  
  - [ ]* 12.2 Escrever testes unitários para googleAuthService
    - Mockar Google Sign-In SDK
    - Testar cenários de sucesso, cancelamento e erro
    - _Requirements: 1.3, 1.4, 1.5, 17.1, 17.6_

- [ ] 13. Implementar authApiService
  - [ ] 13.1 Criar authApiService em services/authApiService.ts
    - Implementar validateGoogleToken
    - Implementar completarCadastro
    - Implementar updateProfile
    - Implementar getProfile
    - Configurar base URL da API
    - _Requirements: 2.1, 7.4, 8.4, 14.1_
  
  - [ ]* 13.2 Escrever testes unitários para authApiService
    - Mockar fetch/axios
    - Testar cada método com respostas válidas e inválidas
    - Testar tratamento de erros HTTP
    - _Requirements: 13.1, 13.2, 13.3_

- [ ] 14. Implementar sessionService
  - [ ] 14.1 Criar sessionService em services/sessionService.ts
    - Implementar saveSession com criptografia
    - Implementar loadSession com descriptografia
    - Implementar clearSession
    - Implementar isSessionValid
    - Usar react-native-encrypted-storage
    - _Requirements: 10.1, 10.2, 10.3, 10.6_
  
  - [ ]* 14.2 Escrever testes de propriedade para persistência de sessão
    - **Property 16: Session Persistence and Restoration**
    - **Validates: Requirements 10.1, 10.2, 10.3, 10.6**
  
  - [ ]* 14.3 Escrever testes de propriedade para refresh de sessão
    - **Property 17: Session Refresh on Token Expiration**
    - **Validates: Requirements 10.4, 10.5**
  
  - [ ]* 14.4 Escrever testes unitários para sessionService
    - Mockar AsyncStorage/EncryptedStorage
    - Testar save e load
    - Testar dados corrompidos
    - _Requirements: 17.2_

- [ ] 15. Checkpoint - Validar services do frontend
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 16. Criar AuthContext e provider
  - [ ] 16.1 Criar AuthContext em contexts/AuthContext.tsx
    - Criar contexto com AuthState
    - Implementar AuthProvider com estado e métodos
    - Implementar login, logout, completarCadastro, updateProfile
    - Integrar com googleAuthService, authApiService e sessionService
    - Implementar restauração de sessão no mount
    - _Requirements: 10.2, 10.3, 15.1, 15.2, 15.3, 15.4, 15.5_
  
  - [ ]* 16.2 Escrever testes unitários para AuthContext
    - Testar cada método do provider
    - Verificar atualizações de estado
    - Verificar notificações de observers
    - _Requirements: 15.3, 15.4_

- [ ] 17. Implementar LoginScreen
  - [ ] 17.1 Criar LoginScreen em screens/LoginScreen.tsx
    - Criar UI com botão "Entrar com Google"
    - Implementar handleGoogleSignIn
    - Integrar com AuthContext
    - Exibir loading state
    - Exibir mensagens de erro
    - _Requirements: 1.2, 1.3, 1.4, 13.1, 13.5_
  
  - [ ]* 17.2 Escrever testes de propriedade para mapeamento de erros
    - **Property 20: Error Message Mapping**
    - **Validates: Requirements 13.1, 13.2, 13.3, 13.4, 13.5**
  
  - [ ]* 17.3 Escrever testes unitários para LoginScreen
    - Testar interação com botão
    - Testar exibição de loading
    - Testar exibição de erros
    - Mockar AuthContext
    - _Requirements: 17.3_

- [ ] 18. Implementar CompletarCadastroScreen
  - [ ] 18.1 Criar CompletarCadastroScreen em screens/CompletarCadastroScreen.tsx
    - Criar formulário com campos obrigatórios
    - Pré-preencher nome, email e foto do Google (disabled)
    - Implementar campos editáveis: apelido, telefone, data_nascimento
    - Implementar validação de formulário
    - Implementar handleSubmit
    - Exibir mensagens de validação
    - _Requirements: 7.2, 7.3, 7.4, 7.11, 7.12, 7.13_
  
  - [ ]* 18.2 Escrever testes unitários para CompletarCadastroScreen
    - Testar validação de campos
    - Testar submissão de formulário
    - Testar exibição de erros
    - Mockar AuthContext
    - _Requirements: 17.4_

- [ ] 19. Implementar EditarPerfilScreen
  - [ ] 19.1 Criar EditarPerfilScreen em screens/EditarPerfilScreen.tsx
    - Carregar dados do perfil atual
    - Criar formulário com campos editáveis
    - Manter nome e email como read-only
    - Implementar validação de formulário
    - Implementar handleSubmit
    - Implementar seleção de foto de perfil
    - Exibir mensagens de validação e sucesso
    - _Requirements: 8.1, 8.2, 8.3, 8.4, 8.8, 8.9_
  
  - [ ]* 19.2 Escrever testes unitários para EditarPerfilScreen
    - Testar carregamento de dados
    - Testar validação de campos
    - Testar submissão de formulário
    - Testar exibição de sucesso
    - Mockar AuthContext
    - _Requirements: 17.5_

- [ ] 20. Implementar navegação condicional
  - [ ] 20.1 Configurar React Navigation com navegação condicional
    - Criar stack navigator para autenticação (Login, CompletarCadastro)
    - Criar stack navigator para app autenticado (Home, EditarPerfil)
    - Implementar lógica de navegação baseada em AuthState
    - Redirecionar para Home se autenticado e LoginSocial existe
    - Redirecionar para CompletarCadastro se requiresRegistration
    - Permanecer em Login se conta bloqueada/desativada
    - _Requirements: 9.1, 9.2, 9.3, 9.4, 9.5_
  
  - [ ]* 20.2 Escrever testes de propriedade para navegação
    - **Property 15: Navigation Based on Authentication State**
    - **Validates: Requirements 9.1, 9.2, 9.3, 9.4**

- [ ] 21. Implementar exibição de perfil
  - [ ] 21.1 Criar componente UserProfile para exibir dados do usuário
    - Exibir nome, email e foto
    - Implementar fallback para foto (avatar padrão)
    - Atualizar UI em 500ms após login
    - _Requirements: 14.1, 14.2, 14.3, 14.4, 14.5_
  
  - [ ]* 21.2 Escrever testes de propriedade para exibição de perfil
    - **Property 21: Profile Display After Authentication**
    - **Validates: Requirements 14.1, 14.2, 14.3, 14.4**
  
  - [ ]* 21.3 Escrever testes unitários para UserProfile
    - Testar exibição de dados
    - Testar fallback de foto
    - Testar atualização de UI
    - _Requirements: 14.1, 14.2, 14.3_

- [ ] 22. Implementar logout
  - [ ] 22.1 Adicionar funcionalidade de logout
    - Criar botão de logout na UI
    - Implementar handleLogout no AuthContext
    - Revogar sessão do Google
    - Limpar dados locais
    - Limpar AuthState
    - Redirecionar para Login
    - _Requirements: 11.1, 11.2, 11.3, 11.4, 11.5_
  
  - [ ]* 22.2 Escrever testes de propriedade para logout
    - **Property 18: Complete Logout Cleanup**
    - **Validates: Requirements 11.1, 11.2, 11.3, 11.4**
  
  - [ ]* 22.3 Escrever testes unitários para logout
    - Testar limpeza de dados
    - Testar redirecionamento
    - Verificar tempo de execução
    - _Requirements: 11.5_

- [ ] 23. Checkpoint - Validar frontend completo
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 24. Configurar Google Sign-In no frontend
  - [ ] 24.1 Configurar Google Sign-In SDK
    - Instalar @react-native-google-signin/google-signin
    - Configurar webClientId no código
    - Configurar Google Sign-In no Android (google-services.json)
    - Configurar Google Sign-In no iOS (Info.plist)
    - _Requirements: 1.1, 1.6_

- [ ] 25. Configurar validação de token no backend
  - [ ] 25.1 Configurar Google.Apis.Auth no backend
    - Instalar pacote NuGet Google.Apis.Auth
    - Configurar Client ID no appsettings.json
    - _Requirements: 2.1, 2.5_

- [ ] 26. Testes de integração end-to-end
  - [ ]* 26.1 Testar fluxo completo de primeiro acesso
    - Login com Google → CompletarCadastro → Home
    - Verificar criação de Pessoa e LoginSocial
    - _Requirements: 7.10, 9.2_
  
  - [ ]* 26.2 Testar fluxo completo de usuário existente
    - Login com Google → Home
    - Verificar atualização de último acesso
    - _Requirements: 6.1, 9.1_
  
  - [ ]* 26.3 Testar fluxo de edição de perfil
    - Editar perfil → Salvar → Verificar atualização
    - _Requirements: 8.8, 8.10_
  
  - [ ]* 26.4 Testar fluxo de conta bloqueada
    - Login com conta bloqueada → Mensagem de erro
    - _Requirements: 5.3, 9.3_
  
  - [ ]* 26.5 Testar fluxo de logout e re-login
    - Logout → Login novamente → Home
    - _Requirements: 11.4_
  
  - [ ]* 26.6 Testar persistência de sessão
    - Login → Fechar app → Reabrir app → Verificar sessão restaurada
    - _Requirements: 10.2, 10.3_

- [ ] 27. Validação final e documentação
  - [ ] 27.1 Executar todos os testes
    - Executar dotnet test no backend
    - Executar npm test no frontend
    - Verificar cobertura de testes (mínimo 80%)
    - _Requirements: 17.9_
  
  - [ ] 27.2 Executar validações de código
    - Executar dotnet build no backend
    - Executar npm run lint no frontend
    - Executar npm run typecheck no frontend
    - _Requirements: 16.9_
  
  - [ ] 27.3 Testar fluxo completo manualmente
    - Testar em dispositivo Android
    - Testar em dispositivo iOS
    - Verificar todos os cenários de erro

- [ ] 28. Checkpoint final - Sistema completo
  - Ensure all tests pass, ask the user if questions arise.

## Notes

- Tasks marcadas com `*` são opcionais e podem ser puladas para MVP mais rápido
- Cada task referencia requirements específicos para rastreabilidade
- Checkpoints garantem validação incremental
- Property tests validam propriedades universais de corretude
- Unit tests validam exemplos específicos e casos extremos
- Backend usa .NET 10, Entity Framework Core, PostgreSQL, AutoMapper, FluentValidation, xUnit e FsCheck
- Frontend usa React Native, TypeScript, React Navigation, Jest, React Native Testing Library e fast-check
- Seguir regras de desenvolvimento incremental: criar testes antes de implementar
- Cada tarefa deve terminar compilando e com testes passando
