import { StatusBar } from 'expo-status-bar';
import { StyleSheet, View, ScrollView } from 'react-native';
import { EntidadeTesteComponent } from './src/components/EntidadeTesteComponent';
import { GoogleSignInTestScreen } from './src/components/GoogleSignInTestScreen';
import React from 'react';

export default function App(): React.JSX.Element {
  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent}>
        <GoogleSignInTestScreen />
        <View style={styles.divider} />
        <EntidadeTesteComponent />
      </ScrollView>
      <StatusBar style="auto" />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
  },
  scrollContent: {
    flexGrow: 1,
    paddingVertical: 20,
  },
  divider: {
    height: 2,
    backgroundColor: '#e0e0e0',
    marginVertical: 30,
    marginHorizontal: 20,
  },
});
