# Como Executar o Prebuild e Usar Google Sign-In Nativo

##  Por Que Prebuild?

O `@react-native-google-signin/google-signin` usa código nativo (Java/Kotlin para Android), então precisa ser compilado.

O **Expo Go** não suporta módulos nativos customizados, por isso o erro.

##  Solução: Expo Dev Client (Build Customizado)

Você vai criar um build customizado do seu app que inclui o módulo nativo.

## � Passo a Passo

### 1. Instalar Dependência

```powershell
cd frontend
npm install @react-native-google-signin/google-signin
```

### 2. Executar Prebuild

```powershell
npx expo prebuild --clean
```

Este comando vai:
- Criar as pastas `android/` e `ios/`
- Configurar os módulos nativos
- Preparar o projeto para build

### 3. Iniciar com Build Customizado

```powershell
npx expo run:android
```

Este comando vai:
- Compilar o código nativo
- Instalar o app no emulador
- Iniciar o Metro bundler

## ⏱ Tempo Estimado

- Primeira vez: 5-10 minutos (compila tudo)
- Próximas vezes: 2-3 minutos

##  O Que Muda?

### Antes (Expo Go)
```
npm start
Pressiona 'a' → Abre no Expo Go
```

### Agora (Build Customizado)
```
npx expo run:android
Compila → Instala → Abre automaticamente
```

##  Diferenças

| Recurso | Expo Go | Build Customizado |
|---------|---------|-------------------|
| Módulos nativos |  Limitado |  Todos |
| Tempo de start | ⚡ Rápido | � Mais lento |
| Google Sign-In nativo |  Não funciona |  Funciona |
| Hot reload |  Sim |  Sim |
| Precisa recompilar |  Não |  Só quando muda código nativo |

##  Troubleshooting

### Erro: "Android SDK not found"
**Solução**: Instale o Android Studio e configure o SDK.

### Erro: "Gradle build failed"
**Solução**: 
```powershell
cd android
./gradlew clean
cd ..
npx expo run:android
```

### Erro: "No devices found"
**Solução**: Inicie o emulador Android antes de executar o comando.

### Demora Muito
**Normal**: A primeira compilação demora. Próximas são mais rápidas.

##  Comandos Úteis

### Limpar e Recompilar
```powershell
npx expo prebuild --clean
npx expo run:android
```

### Apenas Iniciar (sem recompilar)
```powershell
npm start
# Pressione 'a'
```

### Limpar Cache
```powershell
npm start -- --clear
```

## � Checklist

- [ ] Dependência instalada: `npm install @react-native-google-signin/google-signin`
- [ ] Prebuild executado: `npx expo prebuild --clean`
- [ ] Emulador Android iniciado
- [ ] Build executado: `npx expo run:android`
- [ ] App abriu no emulador
- [ ] Testou o login com Google

##  Após o Prebuild

Você terá:
-  Google Sign-In nativo funcionando
-  Sem problemas de redirect URI
-  Login mais rápido e confiável
-  Melhor experiência do usuário

##  Dica

Após o primeiro build, você pode usar `npm start` normalmente. Só precisa recompilar quando:
- Adicionar novos módulos nativos
- Mudar configurações do `app.json`
- Atualizar versões de dependências nativas

## � Referências

- [Expo Prebuild](https://docs.expo.dev/workflow/prebuild/)
- [Expo Dev Client](https://docs.expo.dev/development/introduction/)
- [Google Sign-In](https://github.com/react-native-google-signin/google-signin)
