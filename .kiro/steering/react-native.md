# Regras React Native - Frontend SocialX

## Estrutura do Projeto

```
frontend/
  screens/       - Telas principais
  components/    - Componentes reutilizáveis
  services/      - Chamadas de API
  types/         - Interfaces TypeScript
  tests/         - Testes automatizados
```

## TypeScript Estrito

Nunca usar `any`. Sempre interfaces explícitas:

```ts
interface EntidadeTesteDto {
    id: number;
    nome: string;
    valor: number;
    dataCadastro: string;
}

interface CriarEntidadeTesteRequest {
    nome: string;
    valor: number;
}
```

## Tipagem Obrigatória

Sempre tipar:
- Retorno do servidor (DTOs)
- Parâmetros de requisição
- Estados do componente
- Props de componentes

## Componente de Teste de Referência

Criar componente inicial para validar integração com backend:
- Chamar `GET /api/entidadeteste/{id}`
- Exibir dados retornados
- Validar configuração de base URL e serialização

Exemplo de estrutura:
```ts
interface EntidadeTesteScreenProps {}

const EntidadeTesteScreen: React.FC<EntidadeTesteScreenProps> = () => {
    const [data, setData] = useState<EntidadeTesteDto | null>(null);
    // implementação
};
```

## Service de API

Criar service para cada entidade:
```ts
// services/entidadeTesteService.ts
export const entidadeTesteService = {
    obterPorId: async (id: number): Promise<EntidadeTesteDto> => {
        // implementação
    },
    criar: async (dto: CriarEntidadeTesteRequest): Promise<EntidadeTesteDto> => {
        // implementação
    },
    scroll: async (params: ParametrosScrollInfinito): Promise<ListaScrollInfinito<EntidadeTesteDto>> => {
        // implementação
    }
};
```

## Testes Automatizados

Frameworks:
- Jest
- React Native Testing Library

Testar:
- Renderização de componentes
- Interação do usuário
- Chamadas de API (mock)

Exemplo:
```ts
describe('EntidadeTesteScreen', () => {
    it('deve renderizar dados da API', async () => {
        // teste
    });
});
```

## Fluxo de Desenvolvimento

1. Solicitar Caso de Uso específico
2. Gerar testes
3. Gerar estrutura mínima
4. Implementar após autorização
5. Validar compilação e testes

## Validação Obrigatória

Antes de finalizar:
```bash
npm run lint
npm run typecheck
npm test
```

Tarefa só é concluída após todos os comandos passarem.

## Tecnologias

- React Native
- TypeScript (strict mode)
- React Navigation
- Jest
- React Native Testing Library
- ESLint
- Prettier

## tsconfig.json

```json
{
  "compilerOptions": {
    "strict": true
  }
}
```

## ESLint

Regras obrigatórias:
- `no-explicit-any`
- strict typing
- import ordering

## Qualidade

- Código deve compilar sem erros
- Testes devem passar
- Lint sem warnings
- TypeCheck sem erros
- Nomes claros e descritivos
- Componentes pequenos e focados