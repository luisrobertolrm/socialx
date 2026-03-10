#  Início Rápido - SocialX

## Opção 1: Usando Scripts PowerShell (Mais Fácil)

### 1⃣ Configurar o Banco de Dados (Primeira vez apenas)

```powershell
.\scripts\setup-database.ps1
```

### 2⃣ Iniciar o Backend

```powershell
.\scripts\start-backend.ps1
```

 API rodando em: `https://localhost:7000`
 Swagger em: `https://localhost:7000/swagger`

### 3⃣ Iniciar o Frontend (Em outro terminal)

```powershell
.\scripts\start-frontend.ps1
```

Depois pressione `w` para abrir no navegador

---

## Opção 2: Usando o VS Code

### Backend

1. Pressione `F5` ou clique em `Run > Start Debugging`
2. Selecione `.NET Core Launch (Backend API)`
3. O Swagger abrirá automaticamente

### Frontend

1. Abra um novo terminal (Ctrl + Shift + `)
2. Execute:
```bash
cd frontend
npm start
```
3. Pressione `w` para web

---

## Opção 3: Comandos Manuais

### Backend
```bash
dotnet run --project src/SocialX.Api
```

### Frontend
```bash
cd frontend
npm start
```

---

## � Testando a Integração

1. Com o backend rodando, acesse: `https://localhost:7000/swagger`
2. Teste o endpoint: `GET /api/EntidadeTeste/1`
3. No frontend, clique em "Buscar Entidade (ID: 1)"
4. Você deve ver os dados retornados da API

---

##  Configuração Necessária

Antes de rodar pela primeira vez, edite:

**`src/SocialX.Api/appsettings.json`**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=socialx;Username=postgres;Password=SUA_SENHA"
  }
}
```

---

##  Troubleshooting Rápido

### Backend não inicia
-  PostgreSQL está rodando?
-  Connection string está correta?
-  Migrations foram aplicadas?

### Frontend não conecta
-  Backend está rodando?
-  URL em `frontend/src/services/api.ts` está correta?
-  Se no celular, trocou `localhost` pelo IP da máquina?

### Erro de certificado SSL
No desenvolvimento, você pode ignorar (veja COMO-RODAR.md)

---

##  Documentação Completa

Para mais detalhes, veja:
- `COMO-RODAR.md` - Guia completo
- `README-PROJETO.md` - Visão geral do projeto
- `scripts/ENTIDADES-CRIADAS.md` - Entidades do banco

---

##  Próximos Passos

1.  Rodar os projetos
2.  Testar a integração
3.  Implementar casos de uso (seguindo o README.md)
4. � Criar testes
5.  Deploy

---

##  Dicas

- Use `Ctrl + C` para parar os servidores
- Use `F5` no VS Code para debug do backend
- Use o Swagger para testar a API rapidamente
- O frontend recarrega automaticamente ao salvar arquivos
