# Resolver Erro: Gradle requires JVM 17

##  Erro

```
Gradle requires JVM 17 or later to run. 
Your build is currently configured to use JVM 11.
```

##  Solução Rápida

### 1. Baixar e Instalar JDK 17

� **Link direto**: https://adoptium.net/temurin/releases/?version=17

- Escolha: **Windows x64 MSI**
- Versão: **JDK 17 LTS**
- Durante instalação, marque:
  -  Set JAVA_HOME variable
  -  Add to PATH

### 2. Verificar Instalação

Abra um NOVO terminal e execute:

```powershell
java -version
```

Deve mostrar: `openjdk version "17.x.x"`

### 3. Executar Build Novamente

```powershell
cd frontend
npx expo run:android
```

## ⚡ Solução Temporária (Se Não Puder Instalar Java 17)

Configure JAVA_HOME temporariamente no terminal:

```powershell
# Substitua pelo caminho do seu JDK 17
$env:JAVA_HOME = "C:\Program Files\Eclipse Adoptium\jdk-17.0.13.11-hotspot"
$env:PATH = "$env:JAVA_HOME\bin;$env:PATH"

# Verificar
java -version

# Executar build
cd frontend
npx expo run:android
```

## � Checklist Rápido

- [ ] Baixar JDK 17 do link acima
- [ ] Instalar (marcar opções JAVA_HOME e PATH)
- [ ] Fechar TODOS os terminais
- [ ] Abrir novo terminal
- [ ] Executar: `java -version` (deve mostrar 17)
- [ ] Executar: `npx expo run:android`

##  Após Resolver

O build vai:
1. Compilar o código nativo (5-10 minutos na primeira vez)
2. Instalar o app no emulador
3. Iniciar automaticamente
4. Google Sign-In vai funcionar! 

Leia `ATUALIZAR-JAVA.md` para mais detalhes e troubleshooting!
