# � Como Depurar no VS Code

## Método 1: Debug com F5 (Recomendado)

### Passo a Passo:

1. **Abra o arquivo que quer depurar**
   - Exemplo: `src/SocialX.Api/Controllers/EntidadeTesteController.cs`

2. **Coloque breakpoints**
   - Clique na margem esquerda (ao lado do número da linha)
   - Aparecerá um círculo vermelho �

3. **Inicie o debug**
   - Pressione `F5`
   - Ou clique em `Run > Start Debugging`
   - Ou clique no ícone de "play" na barra lateral esquerda

4. **Teste a API**
   - Abra o Swagger: http://localhost:5062/swagger
   - Ou use o frontend
   - Quando a execução chegar no breakpoint, o VS Code vai pausar

5. **Controles do Debug**
   - `F10` - Step Over (próxima linha)
   - `F11` - Step Into (entrar na função)
   - `Shift+F11` - Step Out (sair da função)
   - `F5` - Continue (continuar até próximo breakpoint)
   - `Shift+F5` - Stop (parar debug)

---

## Método 2: Attach to Process

Se o processo já estiver rodando:

1. Pressione `Ctrl+Shift+P`
2. Digite: `.NET: Attach to Process`
3. Procure por `SocialX.Api` ou `dotnet`
4. Selecione o processo
5. Coloque breakpoints e teste

---

## Método 3: Debug do Frontend (React Native)

### Para depurar o TypeScript:

1. Instale a extensão: **React Native Tools**
2. Adicione breakpoints nos arquivos `.ts` ou `.tsx`
3. Use `console.log()` para debug rápido
4. Abra o DevTools do navegador (F12)

---

##  Verificando se o Debug está funcionando

### Teste Rápido:

1. Abra: `src/SocialX.Api/Controllers/EntidadeTesteController.cs`

2. Coloque um breakpoint na linha do método `Get`:
   ```csharp
   public async Task<ActionResult<EntidadeTesteDto>> Get(long id)
   {
       // Coloque breakpoint AQUI �
       EntidadeTesteDto? entidadeTeste = await entidadeTesteService.ObterPorIdAsync(id);
   ```

3. Pressione `F5`

4. Abra o Swagger: http://localhost:5062/swagger

5. Execute o endpoint `GET /api/EntidadeTeste/1`

6. O VS Code deve pausar no breakpoint! 

---

##  Problemas Comuns

### Breakpoint não funciona (círculo vazio ⚪)

**Causa:** Código não compilado ou desatualizado

**Solução:**
```bash
dotnet clean
dotnet build
```
Depois pressione `F5` novamente

### "No symbols loaded"

**Causa:** Debug symbols não foram gerados

**Solução:** Verifique se está em modo Debug (não Release)

### Breakpoint ignorado

**Causa:** Código não está sendo executado

**Solução:** 
- Verifique se a rota está correta
- Verifique se o método está sendo chamado
- Use `Console.WriteLine()` para confirmar

---

##  Dicas

1. **Use o Debug Console** (Ctrl+Shift+Y) para ver logs
2. **Use Watch** para monitorar variáveis
3. **Use Conditional Breakpoints** (clique direito no breakpoint)
4. **Use Logpoints** para log sem parar a execução

---

##  Atalhos Úteis

| Atalho | Ação |
|--------|------|
| `F5` | Start/Continue Debug |
| `Shift+F5` | Stop Debug |
| `Ctrl+Shift+F5` | Restart Debug |
| `F9` | Toggle Breakpoint |
| `F10` | Step Over |
| `F11` | Step Into |
| `Shift+F11` | Step Out |

---

##  Mais Informações

- [VS Code Debugging](https://code.visualstudio.com/docs/editor/debugging)
- [.NET Debugging](https://code.visualstudio.com/docs/languages/dotnet)
