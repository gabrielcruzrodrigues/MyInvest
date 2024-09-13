# Documento de Fluxo - **MyInvest**

Este documento descreve o fluxo das telas e interações do sistema **MyInvest**, abordando os principais pontos da experiência do usuário desde a tela inicial até as telas de gerenciamento de carteira e consulta de ativos.

## 1. Tela Inicial

### Descrição:
A **Tela Inicial** é o ponto de entrada do sistema, onde o usuário pode buscar por ativos na bolsa de valores (ações e FIIs). Essa tela é acessível a todos os usuários, mesmo que não estejam logados.

### Elementos Principais:
- **Campo de Busca**: Permite a pesquisa de um ativo específico pelo código (ex: PETR4) ou nome.
- **Botões**:
  - `<BUSCAR OUTRO ATIVO>`: Reseta a busca atual e permite ao usuário realizar uma nova consulta.
  - `<VOLTAR>`: Retorna o usuário para a tela inicial sem depender de login ou da existência de uma carteira.

### Fluxo de Interação:
1. O usuário acessa a **Tela Inicial**.
2. O usuário digita o código ou nome de um ativo no **Campo de Busca** e clica no botão de busca.
3. Após a busca, o sistema exibe a tela de **Detalhes do Ativo**.

---

## 2. Tela de Detalhes do Ativo

### Descrição:
Essa tela exibe as informações detalhadas sobre o ativo que o usuário buscou. Aqui, o usuário pode ver se o ativo é recomendado para compra ou não com base no cálculo do preço-teto (modelo Bazin).

### Informações Exibidas:
- Nome do ativo.
- Tipo (ação ou FII).
- Preço atual do ativo.
- Preço-teto calculado.
- Indicador de compra (🟢 Comprar ou 🔴 Não-comprar).
- Outros dados financeiros (Dividend Yield, P/L, ROE, etc.).

### Botões Disponíveis:
- `<BUSCAR OUTRO ATIVO>`: Permite ao usuário retornar à tela de busca para consultar um novo ativo.
- `<VOLTAR>`: Retorna à tela inicial.
- `<ADICIONAR ATIVO EM CARTEIRA>`: Permite adicionar o ativo à carteira do usuário.

### Fluxo de Interação:
1. O usuário visualiza os detalhes do ativo buscado.
2. O usuário tem as seguintes opções:
   - Buscar outro ativo.
   - Voltar à tela inicial.
   - Adicionar o ativo à sua carteira clicando no botão **<ADICIONAR ATIVO EM CARTEIRA>**.

---

## 3. Fluxo de Adição de Ativo à Carteira

### Descrição:
Ao clicar em **<ADICIONAR ATIVO EM CARTEIRA>**, o sistema verifica se o usuário está logado e se ele já possui uma carteira de investimentos.

### Passos do Fluxo:

#### 3.1 Verificação de Login:
- **Usuário não logado**:
  - O sistema exibe uma tela de login, pedindo para o usuário entrar com suas credenciais ou criar uma conta.
  - Após o login, o sistema redireciona o usuário de volta para a ação de adicionar o ativo à carteira.
  
- **Usuário logado**:
  - O sistema verifica se o usuário já possui carteiras.

#### 3.2 Verificação de Carteira:
- **Caso o usuário tenha carteira(s)**:
  - O sistema exibe uma lista com as carteiras do usuário.
  - O usuário escolhe em qual carteira deseja adicionar o ativo.
  - Uma mensagem de confirmação é exibida: "Ativo adicionado à carteira com sucesso".
  
- **Caso o usuário não tenha carteira(s)**:
  - O sistema exibe uma mensagem: "Você ainda não possui uma carteira."
  - O usuário é solicitado a criar uma nova carteira.
  - Após a criação da carteira, o ativo é adicionado automaticamente.

### Botões Disponíveis:
- `<BUSCAR OUTRO ATIVO>`: Retorna à tela inicial para realizar uma nova consulta.
- `<VOLTAR>`: Volta para a tela de detalhes do ativo.

---

## 4. Tela de Login

### Descrição:
Se o usuário não estiver logado, ele será redirecionado para a tela de login ao tentar adicionar um ativo à sua carteira. Esta tela também pode ser acessada diretamente, caso o usuário deseje entrar no sistema para gerenciar suas carteiras.

### Elementos Principais:
- **Campos de Entrada**:
  - Email.
  - Senha.
- **Botões**:
  - `<LOGIN>`: Realiza a autenticação do usuário.
  - `<CRIAR CONTA>`: Redireciona para a tela de criação de conta.

### Fluxo de Interação:
1. O usuário insere suas credenciais e clica em `<LOGIN>`.
2. Se as credenciais estiverem corretas, o sistema redireciona o usuário de volta à ação anterior (adicionar ativo à carteira).
3. Caso contrário, uma mensagem de erro é exibida.

---

## 5. Tela de Criação de Carteira

### Descrição:
Se o usuário não possuir nenhuma carteira, ele será solicitado a criar uma. Esta tela permite ao usuário nomear e criar uma carteira de investimentos, que será usada para armazenar ativos.

### Elementos Principais:
- **Campo de Entrada**:
  - Nome da carteira.
- **Botão**:
  - `<CRIAR CARTEIRA>`: Cria a carteira e adiciona o ativo.

### Fluxo de Interação:
1. O usuário insere o nome da nova carteira.
2. O sistema cria a carteira e adiciona automaticamente o ativo selecionado.
3. Uma mensagem de confirmação é exibida: "Carteira criada e ativo adicionado com sucesso."

---

## 6. Tela de Gestão de Carteiras

### Descrição:
Após o login, o usuário pode acessar suas carteiras para visualização e gerenciamento. Essa tela exibe todas as carteiras existentes e permite adicionar ou remover ativos.

### Elementos Principais:
- **Lista de Carteiras**: Exibe todas as carteiras do usuário.
- **Ações Disponíveis**:
  - Visualizar ativos dentro da carteira.
  - Adicionar novos ativos.
  - Remover ativos existentes.

### Fluxo de Interação:
1. O usuário acessa a tela de gestão de carteiras.
2. Ele pode selecionar uma carteira para visualizar seus ativos.
3. Opções de adicionar ou remover ativos estão disponíveis diretamente dentro de cada carteira.

---

## 7. Fluxo de Exceções e Erros

- **Erro de Login**:
  - Se o usuário inserir credenciais incorretas na tela de login, uma mensagem de erro é exibida: "Credenciais inválidas, tente novamente."
  
- **Erro ao Criar Carteira**:
  - Se ocorrer um erro ao criar uma carteira, uma mensagem de erro é exibida: "Erro ao criar carteira. Tente novamente mais tarde."

- **Erro ao Adicionar Ativo**:
  - Se o sistema falhar ao adicionar um ativo à carteira, uma mensagem de erro é exibida: "Não foi possível adicionar o ativo à carteira. Tente novamente."

---

## 8. Conclusão

O fluxo do sistema **MyInvest** é projetado para ser simples e intuitivo, com foco em facilitar a navegação entre a consulta de ativos, a gestão de carteiras e as ações de login. O usuário pode consultar ativos sem precisar de uma conta, mas as funcionalidades de gerenciamento de carteiras exigem autenticação. O sistema também oferece uma experiência amigável para criar e gerenciar carteiras, proporcionando um ambiente organizado e eficiente para o investidor.