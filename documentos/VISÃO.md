# 💼 Documento de Visão: **MyInvest**

## 1. Resumo Executivo

O **MyInvest** é uma plataforma web destinada a investidores, com o objetivo de fornecer informações detalhadas sobre suas carteiras de ativos e auxiliar nas decisões de compra e venda de ações e FIIs, com base no cálculo do preço-teto pelo modelo de Décio Bazin. O sistema permite aos usuários gerenciar seus ativos e receber recomendações de compra baseadas em dados objetivos.

## 2. Objetivo

O objetivo principal do **MyInvest** é oferecer uma ferramenta prática e eficiente para investidores, que os ajude a tomar decisões embasadas sobre a compra de ativos. Através de cálculos automáticos baseados no modelo de preço-teto de Bazin, o sistema fornece uma visão clara sobre se os ativos estão subvalorizados ou supervalorizados, recomendando a compra ou não.

## 3. Escopo do Sistema

O **MyInvest** inclui uma série de funcionalidades voltadas à gestão de ativos e à geração de insights com base em análises financeiras, tais como:

- **Consulta de ativos em bolsa**: Permite que os usuários pesquisem e visualizem detalhes de qualquer ativo disponível no mercado de ações e FIIs.
- **Gestão de carteira de investimentos**: Criação e manutenção de carteiras de ativos, com a possibilidade de adicionar e remover títulos.
- **Sugestão de compra e venda**: Baseada no preço-teto, o sistema sugere se o ativo está em um preço adequado para compra ou venda.
- **Histórico de decisões**: Armazena um registro das recomendações fornecidas, bem como as ações realizadas pelos usuários, para acompanhamento e análise futura.

## 4. Funcionalidades

### Funcionalidades principais:

1. **Cálculo do preço-teto (modelo Bazin)**:
   - O sistema calcula o preço-teto dos ativos com base nos dividendos e na taxa de retorno desejada.
   - Exibe uma recomendação (comprar ou não) comparando o preço de mercado com o preço-teto.

2. **Gestão de Carteira de Investimentos**:
   - Criação de carteiras personalizadas de investimentos.
   - Adição e remoção de ativos na carteira.
   - Visualização do desempenho da carteira.

3. **Autenticação e Controle de Acesso**:
   - Login e criação de contas para que os usuários possam acessar suas carteiras.
   - Se o usuário não estiver logado, o sistema solicitará login ao tentar realizar uma ação, como adicionar ativos à carteira.

### Fluxo detalhado do sistema:

#### Tela Inicial:

- Campo de busca para que o usuário consulte ativos (ações e FIIs).
- Botões:
  - `<BUSCAR OUTRO ATIVO>`: Inicia uma nova busca.
  - `<VOLTAR>`: Retorna para a tela inicial, sem exigir login ou carteira.

#### Consulta de Ativo:

- Exibe informações detalhadas sobre o ativo consultado (preço atual, preço-teto, P/L, ROE, etc.).
- Botões:
  - `<BUSCAR OUTRO ATIVO>`: Para realizar uma nova busca.
  - `<VOLTAR>`: Para retornar à tela inicial.
  - `<ADICIONAR ATIVO EM CARTEIRA>`: Caso o usuário deseje adicionar o ativo à sua carteira.

#### Adição de Ativo à Carteira:

1. **Verificação de Login**:
   - Se o usuário não estiver logado, o sistema solicita o login.
   - Caso o usuário já esteja logado, o sistema verifica se ele já possui uma carteira.
   
2. **Caso o usuário tenha carteira(s)**:
   - Exibe uma lista de carteiras existentes para que o usuário selecione a carteira onde deseja adicionar o ativo.
   - Após a seleção, o ativo é adicionado à carteira e uma confirmação é exibida.

3. **Caso o usuário não tenha carteira(s)**:
   - Solicita que o usuário crie uma nova carteira antes de adicionar o ativo.
   - Após a criação da carteira, o ativo é automaticamente inserido.

#### Exemplo de Retorno para Ativo:

```
- Data: 16/08/2024
- Ativo: PETR4
- Nome: Petróleo Brasileiro S.A
- Tipo: Ação
- Dividend Yield (DY): 8.5%
- Preço Atual: R$ 28,50
- Preço-Teto (Bazin): R$ 30,00
- Indicação: 🟢 Comprar
```

## 5. Arquitetura do Sistema

O sistema é dividido em dois componentes principais:

1. **MyInvestAPI**: Desenvolvida em **C# | ASP.NET**, essa API é responsável por processar e expor dados financeiros para o cliente web, calculando o preço-teto e outras métricas necessárias.
   
2. **MyInvestClient**: Desenvolvido em **Angular**, este componente é a interface do usuário, permitindo que o investidor consulte ativos, gerencie carteiras e receba sugestões de compra.

## 6. Fórmula do Preço-Teto

A fórmula usada para calcular o preço-teto, de acordo com o modelo de Décio Bazin, é:

$$
\text{Preço Teto} = \frac{\text{Dividendo Médio por Ação}}{\text{Taxa de Retorno Desejada}}
$$

Essa fórmula é utilizada para determinar o valor máximo que o investidor deve pagar por um ativo, baseado nos dividendos recebidos e na taxa mínima de retorno que ele espera obter.

## 7. Diagrama de Sistema

O diagrama do sistema ilustra a interação entre o cliente web e a API. A API recebe dados de ativos, calcula o preço-teto com base no modelo Bazin e retorna esses dados ao cliente Angular, que os exibe em uma interface amigável e responsiva para o usuário.

## 8. Conclusão

O **MyInvest** é uma solução prática para ajudar investidores a gerenciar suas carteiras de forma inteligente e eficiente, fornecendo informações objetivas baseadas em análises de dados. A arquitetura moderna e a integração entre API e frontend garantem uma experiência fluida, com foco em usabilidade e desempenho.