# Resolver Erro: NDK did not have a source.properties file

##  Erro

```
[CXX1101] NDK at C:\Users\luisr\AppData\Local\Android\Sdk\ndk\27.1.12297006 
did not have a source.properties file
```

##  Solução 1: Reinstalar o NDK (Recomendado)

### Via Android Studio

1. **Abrir Android Studio**
2. **Tools > SDK Manager**
3. **SDK Tools** (aba)
4. **Desmarcar**: NDK (Side by side)
5. **Apply** (para desinstalar)
6. **Marcar**: NDK (Side by side)
7. **Apply** (para reinstalar)
8. **OK**

### Via Linha de Comando

```powershell
# Navegar para o SDK Manager
cd C:\Users\luisr\AppData\Local\Android\Sdk\cmdline-tools\latest\bin

# Desinstalar NDK corrompido
.\sdkmanager.bat --uninstall "ndk;27.1.12297006"

# Instalar NDK novamente
.\sdkmanager.bat --install "ndk;27.1.12297006"
```

##  Solução 2: Usar Versão Específica do NDK

Edite `frontend/android/build.gradle` e force uma versão específica:

```gradle
android {
    ndkVersion = "26.1.10909125"  // Versão estável
}
```

##  Solução 3: Remover NDK Corrompido

```powershell
# Remover pasta do NDK corrompido
Remove-Item -Recurse -Force "C:\Users\luisr\AppData\Local\Android\Sdk\ndk\27.1.12297006"

# Limpar cache do Gradle
cd frontend\android
.\gradlew clean

# Tentar build novamente
cd ..
npx expo run:android
```

O Gradle vai baixar o NDK automaticamente.

##  Solução 4: Desabilitar NDK Temporariamente

Se não precisar de código C/C++ nativo, pode desabilitar:

Edite `frontend/android/gradle.properties` e adicione:

```properties
android.useAndroidX=true
android.enableJetifier=true
EXPO_USE_EXOTIC_ARCHITECTURES=false
```

##  Solução Rápida (Recomendada)

Execute este comando para limpar e tentar novamente:

```powershell
cd frontend

# Limpar tudo
Remove-Item -Recurse -Force android\build
Remove-Item -Recurse -Force android\app\build
Remove-Item -Recurse -Force node_modules\.cache

# Limpar Gradle
cd android
.\gradlew clean
cd ..

# Tentar novamente
npx expo run:android
```

## � Checklist

- [ ] Abrir Android Studio
- [ ] SDK Manager > SDK Tools
- [ ] Desinstalar NDK (Side by side)
- [ ] Reinstalar NDK (Side by side)
- [ ] Fechar Android Studio
- [ ] Limpar build: `cd frontend\android && .\gradlew clean`
- [ ] Executar: `npx expo run:android`

##  Se Ainda Não Funcionar

### Opção A: Usar NDK Mais Antigo

Edite `frontend/android/build.gradle`:

```gradle
buildscript {
    ext {
        ndkVersion = "26.1.10909125"
    }
}
```

### Opção B: Baixar NDK Manualmente

1. Acesse: https://developer.android.com/ndk/downloads
2. Baixe: NDK r26c
3. Extraia para: `C:\Users\luisr\AppData\Local\Android\Sdk\ndk\26.1.10909125`
4. Tente novamente

##  Dica

O erro geralmente acontece quando:
- Download do NDK foi interrompido
- Antivírus bloqueou arquivos
- Espaço em disco insuficiente durante download

##  Após Resolver

Execute:

```powershell
cd frontend
npx expo run:android
```

O build deve continuar sem erros de NDK.
