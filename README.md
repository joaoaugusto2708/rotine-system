# LifeQuest

Sistema pessoal de organização, evolução e gamificação da rotina.

O LifeQuest transforma objetivos pessoais em uma jornada estruturada por módulos, metas, temporadas, XP, níveis, títulos, conquistas, badges, recompensas e uma Roda da Vida. A proposta é permitir que cada pessoa acompanhe sua evolução de forma personalizada, com apoio de inteligência artificial para mensagens, feedbacks, títulos e retrospectivas.

## Status do projeto

O projeto está em fase de documentação e preparação arquitetural antes do início da implementação.

Documentação principal concluída:

1. Visão, Escopo e Roadmap
2. Glossário e Regras de Negócio
3. Requisitos do Sistema
4. Modelo de Domínio e DER
5. Arquitetura do Sistema
6. Contrato e Padrões da API
7. Estratégia de Testes
8. Architecture Decision Records (ADRs)

## Objetivos

- Organizar diferentes áreas da vida em módulos customizáveis.
- Criar metas fixas e recorrentes.
- Registrar progresso e atividades.
- Gamificar a evolução por XP, níveis, títulos, conquistas e badges.
- Acompanhar a evolução pela Roda da Vida.
- Trabalhar com temporadas anuais.
- Utilizar IA apenas para geração de conteúdo textual e feedbacks.
- Permitir futura evolução para um produto profissional multiusuário.

## Conceitos principais

### Módulo

Representa uma área, jornada ou tema que o usuário deseja desenvolver.

Exemplos:

- Comunicação
- Saúde
- Liderança
- Finanças
- Inglês
- Desenvolvimento profissional

Um módulo pode possuir categorias opcionais e deve conter pelo menos três metas obrigatórias.

### Meta

A meta é a unidade central do LifeQuest.

Tipos:

- Fixa
- Recorrente

Naturezas possíveis:

- Livro
- Curso
- Certificação
- Atividade física
- Dieta
- Estudo
- Financeiro
- Hábito
- Workshop
- Projeto
- Outro

### Temporada

Cada temporada ocorre de 1º de janeiro a 31 de dezembro.

O nível máximo por temporada é 100.

Ao iniciar uma nova temporada:

- nível volta para 1;
- XP atual volta para 0;
- histórico anterior é preservado;
- o usuário escolhe quais módulos e metas deseja levar;
- metas fixas podem continuar com o progresso atual ou ser reiniciadas;
- metas recorrentes têm o progresso da nova temporada zerado, preservando o histórico.

### Roda da Vida

Áreas oficiais:

1. Saúde e Disposição
2. Desenvolvimento Intelectual
3. Equilíbrio Emocional
4. Realização e Propósito
5. Saúde Financeira
6. Contribuição Social
7. Família
8. Relacionamento Amoroso
9. Vida Social e Amizades
10. Lazer, Hobbies e Diversão
11. Plenitude e Felicidade
12. Espiritualidade

Cada área utiliza escala de 0 a 100.

O usuário realiza uma autoavaliação inicial antes do uso.

Uma meta pode contribuir para várias áreas simultaneamente.

## Gamificação

### XP e níveis

O XP necessário para avançar aumenta progressivamente.

- Nível 1 → 2: 100 XP
- Nível 2 → 3: 200 XP
- Nível 3 → 4: 300 XP
- ...
- Nível 99 → 100: 9.900 XP

Ao subir de nível, o XP necessário é descontado e o excedente permanece na barra do próximo nível.

Exemplo:

```text
Nível 4
380 / 400 XP
+50 XP

Resultado:
Nível 5
30 / 500 XP
```

No nível 100, o XP continua sendo registrado no histórico da temporada, mas não gera novos níveis.

### Título

Concedido por grandes feitos, como:

- níveis 20, 40, 60, 80 e 100;
- área da Roda da Vida que iniciou abaixo de 80 e alcançou 80 ou mais;
- grandes marcos de constância;
- conclusão de todos os módulos da temporada;
- encerramento da temporada.

### Conquista

Gerada por:

- conclusão de meta fixa;
- conclusão de módulo.

### Badge

Gerado principalmente por:

- ciclos recorrentes;
- constância;
- marcos Bronze, Prata e Ouro.

Títulos, conquistas e badges nunca são removidos após conquistados.

## Pontuação

### Tempo e dificuldade

```text
Pontos = horas investidas × multiplicador de dificuldade
```

Multiplicadores:

- dificuldade 1 = 1,1
- dificuldade 2 = 1,2
- dificuldade 3 = 1,3
- dificuldade 4 = 1,4
- dificuldade 5 = 1,5

Minutos são convertidos proporcionalmente em horas.

### Livros

```text
Pontos = páginas lidas / 50
```

Frações de ponto são permitidas.

### Certificações

Complexidade:

1. Introdutória
2. Básica
3. Intermediária
4. Avançada
5. Especialista

XP:

```text
XP = 10 × complexidade
```

### Atividade física

```text
Pontos = (horas × fator de intensidade) + fator da sequência de dias ativos
```

A intensidade varia de 1 a 10 e é convertida para fator de 0,1 a 1,0.

A sequência também varia de 0,1 a 1,0, limitada para cálculo a 10 dias consecutivos.

Se houver um dia completo sem atividade, a sequência atual volta para zero.

O histórico, XP, pontos, badges e marcos já obtidos permanecem.

### Constância

Regras:

- ciclo semanal cumprido gera pontuação-base;
- semanas consecutivas aplicam multiplicador;
- marcos de constância geram XP bônus e badge.

Marcos:

- Bronze
- Prata
- Ouro

## Inatividade

Cada área da Roda da Vida perde 1 ponto por semana completa sem atividade vinculada à sua área principal.

Regras:

- contagem começa após 7 dias completos;
- perda ocorre mesmo sem login;
- pontuação mínima é 0;
- casas decimais são permitidas;
- somente atividade da área principal reinicia o contador de inatividade;
- pontos acima de 100 são descartados, mas o XP continua sendo concedido.

## Recompensas

Cada meta pode possuir no máximo uma recompensa.

Status:

- Bloqueada
- Disponível
- Resgatada
- Expirada

Metas recorrentes podem liberar a mesma recompensa em cada ciclo concluído em 100%.

Recompensas de marco podem ser alteradas apenas enquanto o progresso do marco estiver abaixo de 70%.

## Perfis e acesso

Perfis conceituais:

- Admin
- Psychologist
- Coach
- User

A autenticação e a gestão de roles serão implementadas com recursos consolidados do ecossistema .NET.

Psicólogos e Coaches só podem acessar usuários com vínculo explícito e autorização.

O acesso considera:

```text
Role
+
ProfessionalLink
+
Permission
+
Ownership
```

### Psicólogo

Pode:

- criar templates;
- disponibilizar templates para pacientes vinculados;
- visualizar apenas dados autorizados.

### Coach

Pode:

- visualizar templates autorizados;
- registrar dicas e orientações;
- não pode alterar diretamente XP, metas, pontuação ou progresso.

## Inteligência artificial

A IA será usada apenas para criação de textos.

Casos de uso:

- mensagens de entrada;
- parabéns por conclusão;
- títulos;
- feedbacks;
- alertas de falta de foco;
- análise de equilíbrio da Roda da Vida;
- retrospectiva anual.

A IA nunca pode:

- alterar XP;
- alterar níveis;
- concluir metas;
- alterar pontuação;
- liberar recompensa;
- desbloquear conquista;
- alterar regras de negócio.

Regra principal:

```text
A IA cria textos.
O LifeQuest toma decisões.
```

Conteúdos gerados com sucesso são salvos e não podem ser regenerados.

## Arquitetura

Arquitetura inicial:

- Modular Monolith
- Clean Architecture
- DDD pragmático
- CQRS simplificado
- Domain Events
- Integration Events
- Workers separados quando necessário

Evolução prevista:

- extração gradual de microserviços;
- comunicação assíncrona;
- múltiplos bancos;
- gateway;
- observabilidade distribuída.

## Stack técnica

### Backend

- .NET 10
- ASP.NET Core
- Clean Architecture
- DDD
- CQRS simplificado
- Entity Framework Core

### API

- GraphQL
- Hot Chocolate
- Nitro para exploração do schema
- REST apenas para endpoints auxiliares
- OpenAPI + Scalar para REST auxiliar

### Gateway

- YARP

### Persistência

- MySQL como banco relacional principal
- MongoDB para conteúdo documental e IA

### Mensageria

- RabbitMQ
- Outbox Pattern
- Inbox / idempotência

### Identidade e segurança

- ASP.NET Core Identity
- Roles
- Claims
- Policies
- autorização por recurso e vínculo profissional

### Ambiente e observabilidade

- .NET Aspire
- Docker
- OpenTelemetry
- health checks
- logging estruturado

### Frontend

Previsto:

- React

A estrutura definitiva do frontend será detalhada durante a implementação.

## Estrutura prevista da solução

```text
LifeQuest/
├── src/
│   ├── LifeQuest.Api/
│   ├── LifeQuest.Application/
│   ├── LifeQuest.Domain/
│   ├── LifeQuest.Infrastructure/
│   ├── LifeQuest.Gateway/
│   ├── LifeQuest.Workers/
│   └── LifeQuest.Contracts/
│
├── tests/
│   ├── LifeQuest.Domain.Tests/
│   ├── LifeQuest.Application.Tests/
│   ├── LifeQuest.IntegrationTests/
│   ├── LifeQuest.ArchitectureTests/
│   └── LifeQuest.EndToEndTests/
│
├── docs/
│   └── adr/
│
├── README.md
└── LifeQuest.sln
```

A estrutura pode evoluir conforme os Bounded Contexts forem definidos fisicamente.

## GraphQL

Endpoint principal previsto:

```text
/graphql
```

Não será utilizado versionamento como `/graphql/v1`.

A evolução ocorrerá por:

- mudanças aditivas no schema;
- depreciação de campos;
- novas queries e mutations;
- preservação de compatibilidade quando possível.

Exemplos de mutations:

```text
createModule
updateModule
archiveModule
deleteModule

createGoal
updateGoal
completeGoal
archiveGoal

registerGoalProgress

redeemReward

closeSeason
transitionSeason
```

O cliente envia fatos. O backend calcula as consequências.

O frontend não envia valores autoritativos de:

- XP;
- nível;
- pontos;
- badges;
- conquistas;
- recompensas liberadas.

## REST auxiliar

Endpoints REST serão usados apenas onde fizer mais sentido operacionalmente, por exemplo:

- autenticação;
- health checks;
- integrações específicas;
- endpoints técnicos.

Esses endpoints poderão utilizar:

```text
/api/v1/...
```

Documentação:

- OpenAPI
- Scalar

## Estratégia de testes

Stack:

- xUnit
- NSubstitute
- Testcontainers

Testes previstos:

- unitários;
- domínio;
- aplicação;
- integração;
- GraphQL;
- autenticação e autorização;
- filas;
- workers;
- arquitetura;
- end-to-end.

Cobertura:

- 80% como referência geral;
- regras críticas com cobertura significativamente maior.

Regras críticas:

- XP;
- níveis;
- Roda da Vida;
- recorrência;
- temporadas;
- recompensas;
- autorização.

## Execução local

A configuração final será definida durante a implementação.

A expectativa é utilizar .NET Aspire para orquestrar localmente componentes como:

- API;
- Gateway;
- Workers;
- MySQL;
- MongoDB;
- RabbitMQ.

Docker Compose poderá ser mantido como alternativa ou suporte adicional.

## Configurações previstas

Exemplos de configurações futuras:

```text
ConnectionStrings__MySql
MongoDb__ConnectionString
RabbitMq__Host
Authentication__*
OpenAI__*
OpenTelemetry__*
```

Credenciais e secrets não devem ser armazenados no repositório.

## Documentação

Documentos do projeto:

```text
docs/
├── 01-visao-escopo-roadmap.docx
├── 02-glossario-regras-negocio.docx
├── 03-requisitos-sistema.docx
├── 04-modelo-dominio-der.docx
├── 05-arquitetura-sistema.docx
├── 06-contrato-api.docx
├── 07-estrategia-testes.docx
└── 08-adrs.docx
```

ADRs também poderão ser mantidos individualmente em Markdown:

```text
docs/adr/
├── ADR-001-dotnet-10.md
├── ADR-002-clean-architecture-ddd.md
├── ADR-003-modular-monolith.md
└── ...
```

## Principais ADRs

Decisões arquiteturais já consolidadas:

- .NET 10
- Clean Architecture + DDD pragmático
- Modular Monolith como arquitetura inicial
- GraphQL + Hot Chocolate
- REST auxiliar
- YARP
- MySQL
- MongoDB
- RabbitMQ
- Outbox/Inbox
- ASP.NET Core Identity
- Role + vínculo + permission
- Strategy Pattern
- ledger de XP e Roda da Vida
- Domain Events + Integration Events
- IA desacoplada do domínio
- Workers
- GUID
- xUnit + NSubstitute + Testcontainers
- .NET Aspire
- OpenTelemetry

## Próximas etapas

Antes do início da implementação ainda poderão ser produzidos artefatos adicionais de preparação.

Após essa etapa:

1. criar estrutura do repositório;
2. criar solution .NET 10;
3. configurar projetos;
4. configurar arquitetura base;
5. configurar ambiente local;
6. implementar autenticação;
7. implementar primeiros agregados;
8. configurar persistência;
9. criar schema GraphQL;
10. iniciar testes automatizados.

## Observações

Este README representa o estado do projeto antes do início da implementação.

Ele deverá evoluir junto com o código e passar a incluir:

- comandos reais;
- requisitos de ambiente;
- migrations;
- configuração local;
- execução com Aspire;
- execução com Docker;
- testes;
- exemplos GraphQL;
- troubleshooting;
- pipeline de CI/CD;
- deploy.

---

**LifeQuest**  
Transformando evolução pessoal em uma jornada mensurável, gamificada e consciente.
