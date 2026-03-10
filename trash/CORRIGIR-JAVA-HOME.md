# Corrigir JAVA_HOME para Java 17

##  Situação Atual

- Java 17 instalado: `openjdk version "17.0.17"`
- JAVA_HOME incorreto: `E:\jdk-11.0.0.2`

##  Solução: Atualizar JAVA_HOME

### Método 1: Configurar Permanentemente (Recomendado)

1. **Encontrar o caminho do Java 17**
   ```powershell
   where java
   ```
   
   Vai mostrar algo como:
   ```
   C:\Program Files\Eclipse Adoptium\jdk-17.0.17.10-hotspot\bin\java.exe
   ```
   
   O JAVA_HOME é o caminho SEM o `\bin\java.exe`:
   ```
   C:\Program Files\Eclipse Adoptium\jdk-17.0.17.10-hotspot
   ```

2. **Abrir Variáveis de Ambiente**
   - Pressione `Win + R`
   - Digite: `sysdm.cpl`
   - Pressione Enter
   - Clique na aba: **Avançado**
   - Clique no botão: **Variáveis de Ambiente**

3. **Editar JAVA_HOME**
   - Em "Variáveis do sistema" (parte de baixo)
   - Encontre: `JAVA_HOME`
   - Clique em **Editar**
   - Altere de: `E:\jdk-11.0.0.2`
   - Para: `C:\Program Files\Eclipse Adoptium\jdk-17.0.17.10-hotspot`
   - Clique em **OK**

4. **Verificar PATH**
   - Ainda em "Variáveis do sistema"
   - Encontre: `Path`
   - Clique em **Editar**
   - Certifique-se que tem: `%JAVA_HOME%\bin`
   - Se não tiver, clique em **Novo** e adicione: `%JAVA_HOME%\bin`
   - Mova para o TOPO da lista (use os botões "Mover para cima")
   - Clique em **OK**

5. **Aplicar Mudanças**
   - Clique em **OK** em todas as janelas
   - **FECHE TODOS OS TERMINAIS**
   - Abra um NOVO terminal

6. **Verificar**
   ```powershell
   echo $env:JAVA_HOME
   java -version
   ```

### Método 2: Configurar Temporariamente (Rápido)

Se quiser testar rapidamente sem alterar o sistema:

```powershell
# Configurar JAVA_HOME temporariamente
$env:JAVA_HOME = "C:\Program Files\Eclipse Adoptium\jdk-17.0.17.10-hotspot"
$env:PATH = "$env:JAVA_HOME\bin;$env:PATH"

# Verificar
echo $env:JAVA_HOME
java -version

# Executar build
cd frontend
npx expo run:android
```

 **Nota**: Esta configuração só vale para o terminal atual. Ao fechar, volta ao normal.

##  Encontrar o Caminho Correto do Java 17

Execute este comando para encontrar onde o Java 17 está instalado:

```powershell
where java
```

Ou procure em:
- `C:\Program Files\Eclipse Adoptium\jdk-17.0.17.10-hotspot`
- `C:\Program Files\Java\jdk-17.0.17`
- `C:\Program Files\OpenJDK\jdk-17.0.17`

##  Verificar se Está Correto

Após configurar, execute:

```powershell
# Deve mostrar o caminho do Java 17
echo $env:JAVA_HOME

# Deve mostrar: openjdk version "17.0.17"
java -version

# Deve mostrar: 17
java -version 2>&1 | Select-String "version" | ForEach-Object { $_ -replace '.*"(\d+).*', '$1' }
```

##  Executar Build

Após corrigir o JAVA_HOME:

```powershell
cd frontend
npx expo run:android
```

## � Checklist

- [ ] Encontrou o caminho do Java 17: `where java`
- [ ] Abriu Variáveis de Ambiente: `Win + R` → `sysdm.cpl`
- [ ] Editou JAVA_HOME para apontar para Java 17
- [ ] Verificou que PATH tem `%JAVA_HOME%\bin`
- [ ] Fechou TODOS os terminais
- [ ] Abriu novo terminal
- [ ] Verificou: `echo $env:JAVA_HOME`
- [ ] Verificou: `java -version` (mostra 17)
- [ ] Executou: `npx expo run:android`

##  Resultado Esperado

Após corrigir:

```powershell
PS> echo $env:JAVA_HOME
C:\Program Files\Eclipse Adoptium\jdk-17.0.17.10-hotspot

PS> java -version
openjdk version "17.0.17" 2025-10-21
OpenJDK Runtime Environment Temurin-17.0.17+10 (build 17.0.17+10)
OpenJDK 64-Bit Server VM Temurin-17.0.17+10 (build 17.0.17+10, mixed mode, sharing)

PS> npx expo run:android
› Building app...
[Gradle build inicia com sucesso]
```
