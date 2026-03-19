import React, { useState } from 'react';
import { View, Text, Button, StyleSheet, ActivityIndicator } from 'react-native';
import { entidadeTesteService } from '../services/entidadeTesteService';
import { EntidadeTesteDto } from '../types/EntidadeTesteDto';

export const EntidadeTesteComponent: React.FC = () => {
  const [entidade, setEntidade] = useState<EntidadeTesteDto | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const buscarEntidade = async (): Promise<void> => {
    console.log('🔍 === INÍCIO DA BUSCA ===');
    
    setLoading(true);
    setError(null);
    
    try {
      console.log('📡 Chamando entidadeTesteService.obterPorId(1)...');
      const resultado = await entidadeTesteService.obterPorId(1);
      
      console.log('✅ Resposta recebida:');
      console.table(resultado);
      
      setEntidade(resultado);
      console.log('✅ Estado atualizado com sucesso!');
      
    } catch (err: unknown) {
      console.error('❌ ERRO CAPTURADO:');
      
      if (err instanceof Error) {
        console.error('Tipo:', err.constructor.name);
        console.error('Mensagem:', err.message);
        setError('Erro ao buscar entidade: ' + err.message);
      } else {
        console.error('Erro desconhecido:', err);
        setError('Erro desconhecido ao buscar entidade');
      }
      
      console.error('Detalhes completos:', err);
    } finally {
      setLoading(false);
      console.log('🏁 === FIM DA BUSCA ===\n');
    }
  };

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Teste de Integração Backend</Text>

      <Button title="Buscar Entidade (ID: 1)" onPress={buscarEntidade} />

      {loading && <ActivityIndicator size="large" color="#0000ff" style={styles.loader} />}

      {error && <Text style={styles.error}>{error}</Text>}

      {entidade && (
        <View style={styles.result}>
          <Text style={styles.label}>ID: {entidade.id}</Text>
          <Text style={styles.label}>Nome: {entidade.nome}</Text>
          <Text style={styles.label}>Valor: R$ {entidade.valor.toFixed(2)}</Text>
          <Text style={styles.label}>Data: {new Date(entidade.dataCadastro).toLocaleString()}</Text>
        </View>
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
    justifyContent: 'center',
    backgroundColor: '#fff',
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 20,
    textAlign: 'center',
  },
  loader: {
    marginTop: 20,
  },
  error: {
    color: 'red',
    marginTop: 20,
    textAlign: 'center',
  },
  result: {
    marginTop: 20,
    padding: 15,
    backgroundColor: '#f0f0f0',
    borderRadius: 8,
  },
  label: {
    fontSize: 16,
    marginBottom: 8,
  },
});
