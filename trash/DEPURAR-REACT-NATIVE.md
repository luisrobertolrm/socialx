# � Como Depurar React Native no VS Code

##  Importante: React Native tem limitações para debug com breakpoints

O debug tradicional com breakpoints no React Native é **limitado**. A melhor forma é usar:

---

##  Método 1: Console.log (Mais Simples e Eficaz)

### Passo a Passo:

1. **Adicione console.log no código:**

```typescript
// frontend/src/components/EntidadeTesteComponent.tsx
const buscarEntidade = async (): Promise<void> => {
  console.log(' Iniciando busca...');
  setLoading(true);
  setError(null);
  try {
    console.log(' Chamando API...');
    const resultado = await entidadeTesteService.obterPorId(1);
    console.log(' Resultado:', resultado);
    setEntidade(resultado);
  } catch (err) {
    console.error(' Erro:', err);
    setError('Erro ao buscar entidade: ' + (err as Error).message);
  } finally {
    setLoading(false);
  }
};
```

2. **Abra o DevTools do navegador:**
   - Pressione `F12` no navegador
   - Vá na aba **Console**
   - Você verá todos os logs

3. **No terminal do Expo também aparecem os logs:**
   - Os `console.log` aparecem no terminal onde você rodou `npm start`

---

##  Método 2: React DevTools (Inspecionar Componentes)

### Instalação:

```bash
npm install -g react-devtools
```

### Uso:

1. Execute no terminal:
```bash
react-devtools
```

2. Inicie seu app Expo
3. O React DevTools conectará automaticamente
4. Você pode inspecionar:
   - Props dos componentes
   - State
   - Hooks
   - Árvore de componentes

---

##  Método 3: Chrome DevTools (Web)

Se estiver rodando no navegador (web):

1. **Inicie o Expo:**
```bash
cd frontend
npm start
```

2. **Pressione `w` para abrir no navegador**

3. **Abra o DevTools (F12)**

4. **Vá na aba Sources:**
   - Navegue até `webpack://` → `./src`
   - Coloque breakpoints nos arquivos `.tsx`
   - Os breakpoints funcionam no navegador!

5. **Ou use o debugger do VS Code:**
   - Pressione `F5`
   - Selecione: **Debug React Native (Web)**
   - Coloque breakpoints no VS Code
   - Eles funcionarão no navegador!

---

##  Método 4: Debugger Statement

Use a palavra-chave `debugger` no código:

```typescript
const buscarEntidade = async (): Promise<void> => {
  debugger; // ⬅ Execução para aqui se DevTools estiver aberto
  setLoading(true);
  // ...
};
```

Quando o código chegar nessa linha, o navegador pausará automaticamente (se o DevTools estiver aberto).

---

##  Método 5: VS Code Debug (Web apenas)

### Passo a Passo:

1. **Inicie o Expo:**
```bash
cd frontend
npm start
```

2. **Pressione `w` para abrir no navegador**

3. **No VS Code:**
   - Pressione `F5`
   - Selecione: **Debug React Native (Web)**
   - Ou vá em `Run > Start Debugging`

4. **Coloque breakpoints nos arquivos `.tsx`**

5. **Recarregue a página no navegador**

6. **Os breakpoints devem funcionar!**

---

## � Limitações do Debug no React Native

### O que NÃO funciona bem:

 Breakpoints no código TypeScript quando rodando no celular (Expo Go)
 Step debugging complexo
 Watch de variáveis em tempo real

### O que FUNCIONA bem:

 `console.log()` - Sempre funciona
 `console.error()` - Para erros
 `console.warn()` - Para avisos
 `console.table()` - Para objetos/arrays
 React DevTools - Para inspecionar componentes
 Chrome DevTools - Quando rodando no navegador (web)
 Breakpoints no navegador - Quando rodando web

---

##  Dicas Práticas

### 1. Use console.log estratégico:

```typescript
// Marque com emojis para facilitar identificação
console.log(' Buscando dados...');
console.log(' API Response:', data);
console.log(' Sucesso!');
console.error(' Erro:', error);
```

### 2. Use console.table para objetos:

```typescript
console.table(entidade);
```

### 3. Use try-catch para capturar erros:

```typescript
try {
  const resultado = await api.get('/endpoint');
  console.log('Resultado:', resultado.data);
} catch (error) {
  console.error('Erro completo:', error);
  console.error('Mensagem:', error.message);
  console.error('Response:', error.response?.data);
}
```

### 4. Inspecione requisições HTTP:

No Chrome DevTools:
- Aba **Network**
- Veja todas as requisições
- Clique para ver detalhes (headers, body, response)

---

##  Exemplo Prático de Debug

### Arquivo: `frontend/src/components/EntidadeTesteComponent.tsx`

```typescript
const buscarEntidade = async (): Promise<void> => {
  console.log('=== INÍCIO DA BUSCA ===');
  console.log('Estado atual:', { loading, error, entidade });
  
  setLoading(true);
  setError(null);
  
  try {
    console.log('Chamando API em:', api.defaults.baseURL);
    console.log('Endpoint:', '/EntidadeTeste/1');
    
    const resultado = await entidadeTesteService.obterPorId(1);
    
    console.log('Resposta da API:', resultado);
    console.table(resultado);
    
    setEntidade(resultado);
    console.log(' Estado atualizado com sucesso');
    
  } catch (err) {
    console.error(' ERRO CAPTURADO:');
    console.error('Tipo:', err.constructor.name);
    console.error('Mensagem:', (err as Error).message);
    console.error('Stack:', (err as Error).stack);
    
    if (axios.isAxiosError(err)) {
      console.error('Status:', err.response?.status);
      console.error('Data:', err.response?.data);
      console.error('Headers:', err.response?.headers);
    }
    
    setError('Erro ao buscar entidade: ' + (err as Error).message);
  } finally {
    setLoading(false);
    console.log('=== FIM DA BUSCA ===');
  }
};
```

---

##  Debug no Celular (Expo Go)

Se estiver usando Expo Go no celular:

1. **Abra o menu do Expo:**
   - Agite o celular
   - Ou pressione `m` no terminal do Expo

2. **Selecione "Debug Remote JS"**

3. **Abrirá uma aba no navegador**

4. **Abra o DevTools (F12)**

5. **Veja os logs na aba Console**

---

##  Troubleshooting

### Logs não aparecem?

1. Verifique se o DevTools está aberto (F12)
2. Verifique a aba Console
3. Limpe o console (ícone �)
4. Recarregue a página

### Breakpoints não funcionam?

1. Certifique-se de estar rodando no **navegador web** (não no celular)
2. Use `console.log()` em vez de breakpoints
3. Ou use a palavra-chave `debugger`

### Erro de CORS?

Adicione no backend (`Program.cs`):
```csharp
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
```

---

##  Resumo

| Método | Funciona Web | Funciona Celular | Dificuldade |
|--------|--------------|------------------|-------------|
| console.log |  |  | ⭐ Fácil |
| Chrome DevTools |  |  | ⭐⭐ Médio |
| VS Code Breakpoints |  |  | ⭐⭐⭐ Difícil |
| React DevTools |  |  | ⭐⭐ Médio |
| debugger statement |  |  | ⭐ Fácil |

**Recomendação:** Use `console.log()` para 90% dos casos. É simples, funciona sempre, e é o que desenvolvedores React Native mais usam.
