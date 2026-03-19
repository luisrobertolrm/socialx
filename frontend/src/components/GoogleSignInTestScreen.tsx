import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  TouchableOpacity,
  StyleSheet,
  Image,
  ScrollView,
  ActivityIndicator,
  Alert,
} from 'react-native';
import { GoogleSignInService } from '../services/googleSignInService';
import { AuthState } from '../types/AuthTypes';

export const GoogleSignInTestScreen: React.FC = () => {
  const [authState, setAuthState] = useState<AuthState>({
    isAuthenticated: false,
    user: null,
    loading: false,
    error: null,
  });

  // Configurar Google Sign-In ao montar o componente
  useEffect(() => {
    console.log('🔧 Configurando Google Sign-In...');
    GoogleSignInService.configure();
    checkIfSignedIn();
  }, []);

  const checkIfSignedIn = async (): Promise<void> => {
    const isSignedIn = await GoogleSignInService.isSignedIn();
    
    if (isSignedIn) {
      console.log('✅ Usuário já está logado');
      const user = await GoogleSignInService.getCurrentUser();
      
      if (user) {
        setAuthState({
          isAuthenticated: true,
          user,
          loading: false,
          error: null,
        });
      }
    } else {
      console.log('ℹ️ Nenhum usuário logado');
    }
  };

  const handleLogin = async (): Promise<void> => {
    console.log('🚀 Iniciando login com Google...');
    setAuthState((prev) => ({ ...prev, loading: true, error: null }));

    const result = await GoogleSignInService.signIn();

    if (result.type === 'success' && result.user) {
      console.log('✅ Login bem-sucedido!');
      
      setAuthState({
        isAuthenticated: true,
        user: result.user,
        loading: false,
        error: null,
      });

      Alert.alert('Sucesso!', `Bem-vindo, ${result.user.name}!`);
    } else {
      console.error('❌ Erro no login:', result.error);
      
      setAuthState({
        isAuthenticated: false,
        user: null,
        loading: false,
        error: result.error || 'Erro desconhecido',
      });

      if (result.type !== 'cancel') {
        Alert.alert('Erro', result.error || 'Falha na autenticação');
      }
    }
  };

  const handleLogout = async (): Promise<void> => {
    console.log('👋 Fazendo logout...');
    
    await GoogleSignInService.signOut();
    
    setAuthState({
      isAuthenticated: false,
      user: null,
      loading: false,
      error: null,
    });

    Alert.alert('Logout', 'Você saiu da conta com sucesso');
  };

  return (
    <ScrollView contentContainerStyle={styles.container}>
      <Text style={styles.title}>🔐 Google Sign-In (Native)</Text>

      {authState.loading && (
        <View style={styles.loadingContainer}>
          <ActivityIndicator size="large" color="#4285F4" />
          <Text style={styles.loadingText}>Autenticando...</Text>
        </View>
      )}

      {authState.error && (
        <View style={styles.errorContainer}>
          <Text style={styles.errorText}>❌ {authState.error}</Text>
        </View>
      )}

      {!authState.isAuthenticated && !authState.loading && (
        <View style={styles.loginContainer}>
          <Text style={styles.subtitle}>Faça login com sua conta Google</Text>
          
          <TouchableOpacity
            style={styles.googleButton}
            onPress={handleLogin}
            disabled={authState.loading}
          >
            <Text style={styles.googleButtonText}>🔑 Entrar com Google</Text>
          </TouchableOpacity>

          <View style={styles.infoContainer}>
            <Text style={styles.infoTitle}>ℹ️ Usando Google Sign-In Nativo:</Text>
            <Text style={styles.infoText}>✅ Não precisa de redirect URI</Text>
            <Text style={styles.infoText}>✅ Funciona offline</Text>
            <Text style={styles.infoText}>✅ Mais rápido e confiável</Text>
            <Text style={styles.infoText}>Package: com.example.socialx</Text>
          </View>
        </View>
      )}

      {authState.isAuthenticated && authState.user && (
        <View style={styles.profileContainer}>
          <Text style={styles.welcomeText}>✅ Autenticado com sucesso!</Text>

          {authState.user.photo && (
            <Image source={{ uri: authState.user.photo }} style={styles.avatar} />
          )}

          <View style={styles.userInfo}>
            <Text style={styles.userName}>{authState.user.name}</Text>
            <Text style={styles.userEmail}>{authState.user.email}</Text>
          </View>

          <View style={styles.detailsContainer}>
            <Text style={styles.detailLabel}>ID:</Text>
            <Text style={styles.detailValue}>{authState.user.id}</Text>

            <Text style={styles.detailLabel}>Nome:</Text>
            <Text style={styles.detailValue}>{authState.user.givenName}</Text>

            <Text style={styles.detailLabel}>Sobrenome:</Text>
            <Text style={styles.detailValue}>{authState.user.familyName}</Text>
          </View>

          <TouchableOpacity style={styles.logoutButton} onPress={handleLogout}>
            <Text style={styles.logoutButtonText}>🚪 Sair</Text>
          </TouchableOpacity>
        </View>
      )}
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flexGrow: 1,
    padding: 20,
    backgroundColor: '#f5f5f5',
    alignItems: 'center',
    justifyContent: 'center',
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 30,
    textAlign: 'center',
    color: '#333',
  },
  subtitle: {
    fontSize: 16,
    color: '#666',
    marginBottom: 20,
    textAlign: 'center',
  },
  loadingContainer: {
    alignItems: 'center',
    marginVertical: 30,
  },
  loadingText: {
    marginTop: 10,
    fontSize: 16,
    color: '#4285F4',
  },
  errorContainer: {
    backgroundColor: '#ffebee',
    padding: 15,
    borderRadius: 8,
    marginBottom: 20,
    width: '100%',
  },
  errorText: {
    color: '#c62828',
    fontSize: 14,
    textAlign: 'center',
  },
  loginContainer: {
    width: '100%',
    alignItems: 'center',
  },
  googleButton: {
    backgroundColor: '#4285F4',
    paddingVertical: 15,
    paddingHorizontal: 30,
    borderRadius: 8,
    elevation: 3,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    marginBottom: 30,
  },
  googleButtonText: {
    color: '#fff',
    fontSize: 18,
    fontWeight: 'bold',
  },
  infoContainer: {
    backgroundColor: '#e3f2fd',
    padding: 15,
    borderRadius: 8,
    width: '100%',
  },
  infoTitle: {
    fontSize: 14,
    fontWeight: 'bold',
    marginBottom: 10,
    color: '#1565c0',
  },
  infoText: {
    fontSize: 12,
    color: '#1976d2',
    marginBottom: 5,
  },
  profileContainer: {
    width: '100%',
    alignItems: 'center',
  },
  welcomeText: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#4caf50',
    marginBottom: 20,
  },
  avatar: {
    width: 100,
    height: 100,
    borderRadius: 50,
    marginBottom: 20,
    borderWidth: 3,
    borderColor: '#4285F4',
  },
  userInfo: {
    alignItems: 'center',
    marginBottom: 20,
  },
  userName: {
    fontSize: 22,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 5,
  },
  userEmail: {
    fontSize: 16,
    color: '#666',
  },
  detailsContainer: {
    backgroundColor: '#fff',
    padding: 20,
    borderRadius: 8,
    width: '100%',
    marginBottom: 20,
    elevation: 2,
  },
  detailLabel: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#333',
    marginTop: 10,
  },
  detailValue: {
    fontSize: 14,
    color: '#666',
    marginBottom: 5,
  },
  logoutButton: {
    backgroundColor: '#f44336',
    paddingVertical: 15,
    paddingHorizontal: 30,
    borderRadius: 8,
    elevation: 3,
  },
  logoutButtonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold',
  },
});
