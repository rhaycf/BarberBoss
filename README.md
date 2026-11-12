# BarberBoss

## Sobre o projeto

Esta API foi desenvolvida com .NET 8 e organiza as responsabilidades e as regras de negócio da aplicação em camadas inspiradas nos princípios do Domain-Driven Design (DDD). O objetivo do projeto é permitir o gerenciamento do faturamento de uma barbearia, com informações como data e hora do atendimento, nome do barbeiro, nome do cliente, serviço realizado, valor, forma de pagamento, status e observações, armazenadas em um banco de dados MySQL.

A API segue o padrão REST, disponibilizando endpoints para as operações da aplicação. Para facilitar a exploração e os testes das funcionalidades, o projeto conta com documentação interativa por meio do Swagger.

Dentre os pacotes NuGet utilizados, o AutoMapper facilita o mapeamento entre objetos da aplicação, reduzindo a necessidade de código repetitivo. O FluentValidation é utilizado para definir as regras de validação das requisições, contribuindo para um código mais organizado e de fácil manutenção. Para o acesso ao banco de dados, o Entity Framework Core, integrado ao MySQL por meio do provedor Pomelo, permite manipular os dados utilizando objetos .NET, simplificando as operações de persistência. Os relatórios são gerados com PDFsharp-MigraDoc (PDF) e ClosedXML (Excel).

<img width="1510" height="1221" alt="relatorio-faturamento-barberboss" src="https://github.com/user-attachments/assets/91f42b8f-4a16-4803-90ec-58e956af34d5" />

## Features

- **Domain-Driven Design (DDD):** organização da aplicação em camadas (API, Application, Communication, Domain, Exception e Infrastructure), buscando separar as responsabilidades e facilitar a manutenção e a evolução do código.
- **Autenticação e autorização:** login com token JWT e perfis de acesso (Admin e Barbeiro). Cada faturamento pertence a um usuário, e os relatórios são restritos ao perfil Admin.
- **Gerenciamento de usuários:** cadastro, consulta e atualização do perfil, troca de senha e exclusão da conta, com a senha armazenada criptografada.
- **Regras de negócio validadas:** por exemplo, um faturamento cancelado deve ter valor zero e as datas não podem estar no futuro.
- **Mensagens em vários idiomas:** as mensagens de erro são devolvidas em inglês ou português (pt-BR), de acordo com o header `Accept-Language` da requisição.
- **Testes automatizados:** testes unitários (use cases e validators) e testes de integração da API com `WebApplicationFactory` e banco em memória, usando xUnit, FluentAssertions, Moq e Bogus.
- **Geração de relatórios:** geração de arquivos Excel e documentos PDF do faturamento por período, desconsiderando os atendimentos cancelados.
- **API RESTful com Swagger:** disponibilização de endpoints seguindo os princípios REST, com documentação interativa para visualizar e testar as funcionalidades da API.

## Construído com

![.NET Badge](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=white&style=for-the-badge)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=black&style=for-the-badge)
![Visual Studio](https://img.shields.io/badge/Visual_Studio-5C2D91?logo=visualstudio&logoColor=white&style=for-the-badge)

## Getting Started

Siga as etapas abaixo para configurar e executar o projeto localmente.

### Pré-requisitos

Antes de começar, você precisará ter instalado:

- Visual Studio 2022+ ou Visual Studio Code
- Windows 10+ ou Linux/MacOS com [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) instalado
- MySQL Server

### Instalação

1. Clone o repositório:

```
git clone https://github.com/rhaycf/BarberBoss.git
```

2. Configure a conexão com o banco de dados no arquivo `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "Connection": "Server=localhost;Database=barberbossdb;Uid=SEU_USUARIO;Pwd=SUA_SENHA;"
  },
  "Settings": {
    "Jwt": {
      "SigningKey": "SUA_CHAVE_SECRETA_COM_NO_MINIMO_32_CARACTERES",
      "ExpiresMinutes": 60
    }
  }
}
```

3. Execute a API:

```
dotnet run --project src/BarberBoss.API
```

O Swagger fica disponível em `http://localhost:5010/swagger`.

### Executando os testes

Para rodar todos os testes (unitários, validators e integração):

```bash
dotnet test
```

### Relatório de cobertura de testes

O projeto usa o `coverlet` para medir a cobertura e o `ReportGenerator` para gerar o relatório em HTML.

1. Instale o gerador de relatório (uma vez só no computador):

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

2. Rode os testes coletando a cobertura:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

3. Gere o relatório:

```bash
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
```

4. Abra o arquivo `coverage-report/index.html` no navegador.

> As pastas `coverage-report/` e `TestResults/` são geradas localmente e não são versionadas.

## Autenticação

1. Cadastre um usuário em `POST /api/user`.
2. Faça login em `POST /api/login` e copie o token retornado.
3. Envie o token no header das demais requisições: `Authorization: Bearer SEU_TOKEN`.

## Endpoints

| Método | Rota | Descrição | Acesso |
|--------|------|-----------|--------|
| POST | `/api/user` | Registra um usuário | Público |
| POST | `/api/login` | Autentica e retorna o token | Público |
| GET | `/api/user` | Retorna o perfil do usuário logado | Autenticado |
| PUT | `/api/user` | Atualiza o perfil do usuário logado | Autenticado |
| PUT | `/api/user/change-password` | Altera a senha | Autenticado |
| DELETE | `/api/user` | Exclui a conta do usuário logado | Autenticado |
| POST | `/api/billings` | Registra um faturamento | Autenticado |
| GET | `/api/billings` | Lista os faturamentos do usuário logado | Autenticado |
| GET | `/api/billings/{id}` | Busca um faturamento pelo id | Autenticado |
| PUT | `/api/billings/{id}` | Atualiza um faturamento | Autenticado |
| DELETE | `/api/billings/{id}` | Remove um faturamento | Autenticado |
| GET | `/api/report/pdf?startDate=&endDate=` | Gera o relatório do período em PDF | Admin |
| GET | `/api/report/excel?startDate=&endDate=` | Gera o relatório do período em Excel | Admin |

As datas dos relatórios seguem o formato `yyyy-MM-dd`. Quando não há faturamentos no período, os endpoints de relatório e a listagem de faturamentos retornam `204 No Content`.
