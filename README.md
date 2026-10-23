# BarberBoss

## Sobre o projeto

Esta API foi desenvolvida com .NET 8 e organiza as responsabilidades e as regras de negócio da aplicação em camadas inspiradas nos princípios do Domain-Driven Design (DDD). O objetivo do projeto é permitir o gerenciamento do faturamento de uma barbearia, com informações como data e hora do atendimento, nome do barbeiro, nome do cliente, serviço realizado, valor, forma de pagamento, status e observações, armazenadas em um banco de dados MySQL.

A API segue o padrão REST, disponibilizando endpoints para as operações da aplicação. Para facilitar a exploração e os testes das funcionalidades, o projeto conta com documentação interativa por meio do Swagger.

Dentre os pacotes NuGet utilizados, o AutoMapper facilita o mapeamento entre objetos da aplicação, reduzindo a necessidade de código repetitivo. O FluentValidation é utilizado para definir as regras de validação das requisições, contribuindo para um código mais organizado e de fácil manutenção. Para o acesso ao banco de dados, o Entity Framework Core, integrado ao MySQL por meio do provedor Pomelo, permite manipular os dados utilizando objetos .NET, simplificando as operações de persistência. Os relatórios são gerados com PDFsharp-MigraDoc (PDF) e ClosedXML (Excel).

## Features

- **Domain-Driven Design (DDD):** organização da aplicação em camadas (API, Application, Communication, Domain, Exception e Infrastructure), buscando separar as responsabilidades e facilitar a manutenção e a evolução do código.
- **Regras de negócio validadas:** por exemplo, um faturamento cancelado deve ter valor zero e as datas não podem estar no futuro.
- **Testes unitários:** testes automatizados com xUnit, Shouldly e Bogus para verificar o comportamento da aplicação e auxiliar na identificação de possíveis erros.
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
  }
}
```

3. Execute a API:

```
dotnet run --project src/BarberBoss.API
```

O Swagger fica disponível em `http://localhost:5010/swagger`.

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/billings` | Registra um faturamento |
| `GET` | `/api/billings` | Lista todos os faturamentos |
| `GET` | `/api/billings/{id}` | Busca um faturamento pelo id |
| `PUT` | `/api/billings/{id}` | Atualiza um faturamento |
| `DELETE` | `/api/billings/{id}` | Remove um faturamento |
| `GET` | `/api/report/pdf?startDate=&endDate=` | Gera o relatório do período em PDF |
| `GET` | `/api/report/excel` | Gera o relatório do período em Excel (`startDate` e `endDate` nos headers) |
