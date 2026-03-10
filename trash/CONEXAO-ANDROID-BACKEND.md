# Conexão Android Emulator com Backend

## Problema
O emulador Android não consegue acessar `localhost` ou `127.0.0.1` do host porque ele roda em uma máquina virtual isolada.

## Soluções Possíveis

### 1. Usar IP da Máquina na Rede Local (RECOMENDADO) 
- **IP da sua máquina**: `10.0.0.100`
- **URL configurada**: `http://10.0.0.100:5062/api`
- **Vantagem**: Funciona com emulador Android e dispositivos físicos na mesma rede
- **Configuração**: Backend deve escutar em `0.0.0.0:5062` (todas as interfaces)

### 2. Usar 10.0.2.2 (Apenas para Emulador Android)
- **URL**: `http://10.0.2.2:5062/api`
- **Vantagem**: Endereço especial do emulador que mapeia para localhost do host
- **Desvantagem**: Não funciona com dispositivos físicos

## Configuração Atual

### Frontend (`frontend/src/services/api.ts`)
```typescript
const getBaseURL = (): string => {
  if (Platform.OS === 'android') {
    return 'http://10.0.0.100:5062/api'; // IP da máquina host
  }
  return 'http://localhost:5062/api'; // iOS/Web
};
```

### Backend (`src/SocialX.Api/Properties/launchSettings.json`)
```json
"applicationUrl": "http://0.0.0.0:5062"
```
- `0.0.0.0` significa que o backend aceita conexões de qualquer interface de rede

## Como Testar

1. **Reinicie o backend** para aplicar as mudanças:
   ```powershell
   .\scripts\start-backend.ps1
   ```

2. **Reinicie o frontend** (se já estava rodando):
   ```powershell
   cd frontend
   npm start
   ```
   Pressione `r` para recarregar o app no emulador

3. **Verifique o firewall do Windows**:
   - O Windows pode bloquear conexões externas na porta 5062
   - Se necessário, adicione uma regra para permitir conexões na porta 5062

4. **Teste a conexão** diretamente do emulador:
   - Abra o navegador do emulador
   - Acesse: `http://10.0.0.100:5062/swagger`
   - Se carregar o Swagger, a conexão está OK

## Troubleshooting

### Erro: "Network Error" persiste
1. Verifique se o backend está rodando: `http://localhost:5062/swagger` no navegador do host
2. Verifique o firewall do Windows
3. Teste com `10.0.2.2` em vez de `10.0.0.100`
4. Verifique se o IP da máquina não mudou: `ipconfig`

### Erro: "Connection Refused"
- Backend não está rodando ou não está escutando em `0.0.0.0`

### Erro: "Timeout"
- Firewall bloqueando a porta 5062
- IP da máquina mudou (DHCP)
