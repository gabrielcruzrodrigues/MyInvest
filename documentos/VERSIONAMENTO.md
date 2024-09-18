# Padrão de Gerenciamento de Versionamento e Colaboração no GitLab

Este documento estabelece a estratégia de versionamento e colaboração a ser seguida para garantir eficiência no desenvolvimento e evitar problemas com *merges* no GitLab. A estrutura proposta visa manter o código limpo, organizado e minimizar conflitos durante o desenvolvimento.



## 1. **Estratégia de Branching**
Adotar uma abordagem baseada no GitFlow ou Feature Branching. A estrutura de *branches* deve seguir o seguinte padrão:

- **main**: Branch principal que contém o código pronto para produção.
- **develop**: Branch de desenvolvimento estável, onde as funcionalidades aprovadas são integradas antes de serem enviadas para produção.
- **feature/xxx**: *Branches* específicas para o desenvolvimento de novas funcionalidades (ex.: `feature/nome-da-feature`).
- **hotfix/xxx**: *Branches* para correções críticas e urgentes no código de produção.

**Nota:** Os desenvolvedores (Marcos Morais e Gabriel Rodrigues) devem criar *branches* nomeadas de forma clara e sugestiva para cada nova tarefa ou funcionalidade. Quando concluírem, devem abrir um *merge request* e solicitar ao Gerente de Projeto (Marcos Morais) a revisão e integração do código na branch de destino, como `develop` ou `main`.

**Adicione arquivos usando a linha de comando ou envie um repositório Git existente com o seguinte comando:**
```
cd myinvest.net
git init
git branch -a
git branch -M branchdetrabalho
git add .
git commit -m "Primeiro commit"
git remote add origin https://gitlab.com/marcosmoraisjr/myinvest.net.git
git push -u origin branchdetrabalho
```

**Nota:**  Para este projeto, a branchdetrabalho pode ser o nome do desenvolvedor.

## 2. **Atribuição e Gerenciamento de Tarefas**
- **Gerente de Projeto (Marcos Morais)**: Responsável pela gestão de tarefas e organização das sprints. Também revisa os *merge requests* e aprova ou solicita ajustes.
- **Desenvolvedores (Marcos Morais e Gabriel Rodrigues)**: Criam *branches* para cada tarefa, como novas funcionalidades ou correções de bugs, seguindo o padrão de nomeação estabelecido. Devem garantir que suas alterações estejam de acordo com as especificações e boas práticas.

## 3. **Realização de Merge Requests e Pull Requests**
- As mudanças devem ser pequenas e frequentes, submetidas por meio de *merge requests* regulares. Isso facilita a revisão de código, evita grandes *merges* complexos e reduz o risco de conflitos.
- Cada *merge request* deve ser revisado por outro membro da equipe antes de ser aprovado, garantindo uma segunda verificação e maior controle de qualidade.

## 4. **Automatização de Testes**
- Configurar pipelines no GitLab CI/CD para executar testes automatizados em cada *merge request*. Esses testes são cruciais para garantir que o código submetido não introduza novos erros ou quebre funcionalidades existentes.
- Todo *merge request* deve passar com sucesso nos testes automatizados antes de ser aprovado e integrado.

## 5. **Manutenção Regular de Branches (Rebase ou Merge)**
- As *feature branches* devem ser regularmente atualizadas com as últimas alterações da branch `develop` para evitar conflitos acumulados. Isso pode ser feito por meio de *rebase* ou *merge* da branch de desenvolvimento.
- Os desenvolvedores devem garantir que suas branches estejam sincronizadas com a branch de desenvolvimento antes de submeterem um *merge request*.

## 6. **Revisão de Código (Code Review)**
- A revisão de código deve ser um processo colaborativo. Marcos e Gabriel devem revisar o código um do outro para identificar melhorias, garantir conformidade com os padrões de codificação e minimizar a introdução de bugs.
- A revisão deve ser realizada antes da aprovação de qualquer *merge request* para garantir a qualidade do código.

## 7. **Gerenciamento de Conflitos**
Resolva conflitos de merge rapidamente: Use ferramentas de merge do GitLab para resolver conflitos de forma eficiente e mantenha a comunicação clara sobre as mudanças necessárias.