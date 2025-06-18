# APS2HashTable - Sistema Clínico com Tabela Hash

Este projeto implementa um sistema de gerenciamento de pacientes para uma clínica, utilizando uma tabela hash em C# para armazenamento eficiente dos dados. O sistema permite o cadastro, atualização, busca, remoção e atendimento de pacientes, controlando também a fila de triagem e o histórico de atendimentos.

## Funcionalidades

- **Cadastro de Pacientes:** Cada paciente é identificado pelo CPF e possui informações como nome, pressão arterial, temperatura e oxigenação.
- **Definição de Prioridade:** O sistema classifica automaticamente a prioridade do paciente (Verde, Amarela ou Vermelha) com base em seus dados clínicos.
- **Tabela Hash:** Os pacientes são armazenados em uma tabela hash, garantindo buscas, inserções e remoções eficientes.
- **Fila de Triagem:** Os pacientes aguardam atendimento em uma fila de triagem após o cadastro.
- **Histórico de Atendimentos:** O sistema mantém um histórico dos pacientes atendidos.
- **Menu Interativo:** Todas as operações são acessíveis por um menu interativo no terminal.

## Como funciona

- O paciente é cadastrado informando CPF, nome e dados clínicos.
- A prioridade é calculada automaticamente:
  - **Vermelha:** Situação crítica (ex: pressão > 18, temperatura > 39, oxigenação < 90).
  - **Amarela:** Situação de alerta (ex: pressão fora de 12-14, temperatura fora de 36-37.5, oxigenação < 95).
  - **Verde:** Situação estável.
- O CPF é usado como chave na tabela hash, que utiliza encadeamento para tratar colisões.
- A fila de triagem regula a ordem de atendimento.
- O histórico permite revisar pacientes já atendidos.

## Estrutura do Projeto

- `Paciente`: Classe que representa um paciente e calcula sua prioridade.
- `TabelaHash`: Estrutura de dados para armazenar e gerenciar pacientes por CPF.
- `SistemaClinico`: Controla o fluxo do sistema, fila de triagem e histórico.
- `Program`: Inicializa o sistema e exibe o menu.

## Como usar

1. Compile o projeto com um compilador C# compatível (.NET).
2. Execute o programa.
3. Use o menu para cadastrar, buscar, atualizar ou remover pacientes, bem como controlar a fila e o histórico de atendimentos.

## Exemplo de uso

```
--- MENU ---
1. Cadastrar paciente
2. Buscar paciente por CPF
3. Atualizar dados clínicos
4. Remover paciente
5. Exibir tabela hash
6. Exibir fila de triagem
7. Atender próximo paciente (simular atendimento)
8. Exibir histórico de atendimentos
0. Sair
Escolha:
```

---

**Observação:** Este projeto é uma aplicação didática para fins acadêmicos, demonstrando o uso de tabela hash, filas e pilhas em C# para gerenciamento de dados em sistemas clínicos.
