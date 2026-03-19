import { GoogleSignin, statusCodes } from '@react-native-google-signin/google-signin';
import { GOOGLE_CONFIG } from '../config/googleAuth';
import { GoogleUser, GoogleAuthResponse } from '../types/AuthTypes';

export class GoogleSignInService {
  // Configurar o Google Sign-In
  static configure(): void {
    GoogleSignin.configure({
      webClientId: GOOGLE_CONFIG.webClientId,
      offlineAccess: true,
      scopes: GOOGLE_CONFIG.scopes,
    });

    console.log('✅ Google Sign-In configurado');
    console.log('📋 Web Client ID:', GOOGLE_CONFIG.webClientId);
  }

  // Verificar se já está logado
  static async isSignedIn(): Promise<boolean> {
    return await GoogleSignin.isSignedIn();
  }

  // Fazer login
  static async signIn(): Promise<GoogleAuthResponse> {
    try {
      console.log('🚀 Iniciando Google Sign-In...');

      // Verificar se o Google Play Services está disponível
      await GoogleSignin.hasPlayServices();
      console.log('✅ Google Play Services disponível');

      // Fazer login
      const response = await GoogleSignin.signIn();
      console.log('✅ Login bem-sucedido!');
      console.log('📦 Resposta completa:', JSON.stringify(response, null, 2));
      
      // Verificar se tem dados do usuário
      if (!response.data) {
        console.error('❌ Resposta sem dados do usuário');
        return {
          type: 'error',
          error: 'Resposta sem dados do usuário',
        };
      }

      const userData = response.data;
      console.log('👤 Usuário:', userData.user.name);
      console.log('📧 Email:', userData.user.email);

      // Obter tokens separadamente
      const tokens = await GoogleSignin.getTokens();
      console.log('🔑 Tokens obtidos');

      const user: GoogleUser = {
        id: userData.user.id,
        email: userData.user.email,
        name: userData.user.name || '',
        givenName: userData.user.givenName || '',
        familyName: userData.user.familyName || '',
        photo: userData.user.photo || '',
      };

      return {
        type: 'success',
        user,
        accessToken: tokens.accessToken,
        idToken: tokens.idToken,
      };
    } catch (error: unknown) {
      console.error('❌ Erro no Google Sign-In:', error);
      console.error('📋 Tipo do erro:', typeof error);
      console.error('📋 Erro completo:', JSON.stringify(error, null, 2));

      if (error && typeof error === 'object' && 'code' in error) {
        const errorCode = (error as { code: string }).code;
        console.error('📋 Código do erro:', errorCode);

        if (errorCode === statusCodes.SIGN_IN_CANCELLED) {
          console.log('ℹ️ Login cancelado pelo usuário');
          return {
            type: 'cancel',
            error: 'Login cancelado pelo usuário',
          };
        }

        if (errorCode === statusCodes.IN_PROGRESS) {
          console.log('⏳ Login já em progresso');
          return {
            type: 'error',
            error: 'Login já em progresso',
          };
        }

        if (errorCode === statusCodes.PLAY_SERVICES_NOT_AVAILABLE) {
          console.error('❌ Google Play Services não disponível');
          return {
            type: 'error',
            error: 'Google Play Services não disponível',
          };
        }

        // Erro DEVELOPER_ERROR (código 10)
        if (errorCode === '10' || errorCode === statusCodes.SIGN_IN_REQUIRED) {
          console.error('❌ DEVELOPER_ERROR: SHA-1 ou package name incorreto');
          console.error('📋 Package configurado: com.example.socialx');
          console.error('📋 SHA-1 esperado: 78:DF:6A:D2:A3:15:34:C5:BE:AC:4C:CE:32:B6:18:80:82:A7:A9:98');
          console.error('📋 Verifique o Google Cloud Console');
          return {
            type: 'error',
            error: 'Erro de configuração: Verifique SHA-1 e package name no Google Cloud Console',
          };
        }

        return {
          type: 'error',
          error: `Erro: ${errorCode}`,
        };
      }

      return {
        type: 'error',
        error: error instanceof Error ? error.message : 'Erro desconhecido',
      };
    }
  }

  // Fazer logout
  static async signOut(): Promise<void> {
    try {
      await GoogleSignin.signOut();
      console.log('👋 Logout realizado com sucesso');
    } catch (error) {
      console.error('❌ Erro ao fazer logout:', error);
    }
  }

  // Obter usuário atual
  static async getCurrentUser(): Promise<GoogleUser | null> {
    try {
      const response = await GoogleSignin.signInSilently();
      
      if (!response.data) {
        return null;
      }

      const userData = response.data;
      
      return {
        id: userData.user.id,
        email: userData.user.email,
        name: userData.user.name || '',
        givenName: userData.user.givenName || '',
        familyName: userData.user.familyName || '',
        photo: userData.user.photo || '',
      };
    } catch (error) {
      console.log('ℹ️ Nenhum usuário logado');
      return null;
    }
  }

  // Revogar acesso
  static async revokeAccess(): Promise<void> {
    try {
      await GoogleSignin.revokeAccess();
      console.log('🔒 Acesso revogado');
    } catch (error) {
      console.error('❌ Erro ao revogar acesso:', error);
    }
  }
}
