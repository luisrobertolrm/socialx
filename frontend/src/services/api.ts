import axios from 'axios';
import { Platform } from 'react-native';

// Para Android emulator, use o IP da sua máquina na rede local
// Para iOS simulator e web, use localhost
const getBaseURL = (): string => {
  if (Platform.OS === 'android') {
    // IP da máquina host na rede local
    return 'http://10.0.0.100:5062/api';
  }
  return 'http://localhost:5062/api';
};

const api = axios.create({
  baseURL: getBaseURL(),
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 10000, // 10 segundos
});

// Interceptor para log de requisições (útil para debug)
api.interceptors.request.use(
  (config) => {
    console.log(`🌐 API Request: ${config.method?.toUpperCase()} ${config.baseURL}${config.url}`);
    return config;
  },
  (error) => {
    console.error('❌ API Request Error:', error);
    return Promise.reject(error);
  }
);

// Interceptor para log de respostas
api.interceptors.response.use(
  (response) => {
    console.log(`✅ API Response: ${response.status} ${response.config.url}`);
    return response;
  },
  (error) => {
    if (error.response) {
      console.error(`❌ API Error Response: ${error.response.status}`, error.response.data);
    } else if (error.request) {
      console.error('❌ API No Response:', error.message);
    } else {
      console.error('❌ API Error:', error.message);
    }
    return Promise.reject(error);
  }
);

export default api;
