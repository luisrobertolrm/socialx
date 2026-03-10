# Atualizar Java para JDK 17

##  Erro Atual

```
Gradle requires JVM 17 or later to run. Your build is currently configured to use JVM 11.
```

##  Solução: Instalar JDK 17

### Opção 1: Instalar JDK 17 (Recomendado)

1. **Baixar JDK 17**
   - Link: https://adoptium.net/temurin/releases/?version=17
   - Escolha: **Windows x64** (MSI ou ZIP)
   - Versão: **JDK 17 LTS**

2. **Instalar**
   - Execute o instalador
   - Marque: "Set JAVA_HOME variable"
   - Marque: "Add to PATH"

3. **Verificar Instalação**
   ```powershell
   java -version
   ```
   Deve mostrar: `openjdk version "17.x.x"`

### Opção 2: Usar Gradle com Java Específico

Se você tem múltiplas versões do Java instaladas:

1. **Encontrar o caminho do JDK 17**
   ```powershell
   # Exemplo:
   C:\Program Files\Eclipse Adoptium\jdk-17.x.x-hotspot
   ```

2. **Configurar JAVA_HOME temporariamente**
   ```powershell
   $env:JAVA_HOME = "C:\Program Files\Eclipse Adoptium\jdk-17.0.13.11-hotspot"
   $env:PATH = "$env:JAVA_HOME\bin;$env:PATH"
   ```

3. **Verificar**
   ```powershell
   java -version
   ```

4. **Executar o build novamente**
   ```powershell
   npx expo run:android
   ```

### Opção 3: Downgrade do Gradle (Não Recomendado)

Se não puder instalar Java 17, pode usar Gradle 8.x que funciona com Java 11:

1. **Editar `frontend/android/gradle/wrapper/gradle-wrapper.properties`**
   ```properties
   distributionUrl=https\://services.gradle.org/distributions/gradle-8.10.2-bin.zip
   ```

2. **Executar novamente**
   ```powershell
   npx expo run:android
   ```

##  Verificar Versão Atual do Java

```powershell
java -version
```

Saída esperada:
```
openjdk version "17.0.x" 2024-xx-xx
OpenJDK Runtime Environment Temurin-17.0.x+x (build 17.0.x+x)
OpenJDK 64-Bit Server VM Temurin-17.0.x+x (build 17.0.x+x, mixed mode, sharing)
```

## � Checklist

- [ ] JDK 17 instalado
- [ ] JAVA_HOME configurado
- [ ] PATH atualizado
- [ ] `java -version` mostra versão 17
- [ ] Terminal reiniciado (para carregar novas variáveis)
- [ ] `npx expo run:android` executado novamente

##  Troubleshooting

### Ainda mostra Java 11 após instalar Java 17

**Causa**: Variáveis de ambiente não atualizadas.

**Solução**:
1. Feche TODOS os terminais
2. Abra um novo terminal
3. Execute: `java -version`
4. Se ainda mostrar 11, configure manualmente:
   ```powershell
   # Adicionar ao perfil do PowerShell
   notepad $PROFILE
   
   # Adicione estas linhas:
   $env:JAVA_HOME = "C:\Program Files\Eclipse Adoptium\jdk-17.0.13.11-hotspot"
   $env:PATH = "$env:JAVA_HOME\bin;$env:PATH"
   ```

### Múltiplas versões do Java instaladas

**Solução**: Configure JAVA_HOME para apontar para o JDK 17:

1. **Abrir Variáveis de Ambiente**
   - Pressione `Win + R`
   - Digite: `sysdm.cpl`
   - Aba: **Avançado**
   - Botão: **Variáveis de Ambiente**

2. **Editar JAVA_HOME**
   - Em "Variáveis do sistema"
   - Edite `JAVA_HOME`
   - Valor: `C:\Program Files\Eclipse Adoptium\jdk-17.0.13.11-hotspot`

3. **Editar PATH**
   - Certifique-se que `%JAVA_HOME%\bin` está no PATH
   - Mova para o topo da lista

4. **Reiniciar terminal**

### Erro ao baixar Gradle

Se o download do Gradle falhar novamente:

```powershell
# Limpar cache do Gradle
Remove-Item -Recurse -Force $env:USERPROFILE\.gradle\wrapper\dists

# Tentar novamente
npx expo run:android
```

##  Após Atualizar o Java

Execute novamente:

```powershell
cd frontend
npx expo run:android
```

O build deve começar e compilar o app com sucesso!

## � Links Úteis

- [Adoptium JDK 17](https://adoptium.net/temurin/releases/?version=17)
- [Oracle JDK 17](https://www.oracle.com/java/technologies/javase/jdk17-archive-downloads.html)
- [Gradle Requirements](https://docs.gradle.org/current/userguide/compatibility.html)
