# Projeto Investimento API

Uma API construída com .NET 9, seguindo os princípios SOLID e Clean Architecture, com logs detalhados, testes automatizados e suporte a Docker.

## 🏗️ Arquitetura

Este projeto implementa boas práticas de **Clean Architecture** e **S.O.L.I.D** com a seguinte estrutura:

- **ProjInv.Domain**: Entidades, interfaces e regras de negócio
- **ProjInv.Application**: UseCases, DTOs e lógica de aplicação
- **ProjInv.Infrastructure**: Repositórios, acesso a dados e serviços externos
- **ProjInv.API**: Endpoints da API, middleware e apresentação
- **ProjInv.Tests**: Testes unitários e de integração

## 🚀 Características

- Clean Architecture
- SOLID
- API .NET 9
- Entity Framework Core (SQL Server)
- Logging estruturado com Serilog
- Validação de entrada com FluentValidation
- Middleware de tratamento global de exceções
- Testes com xUnit
- Docker e docker-compose
- Documentação Swagger

## 📋 Endpoints da API

### Investidores
- `POST /api/investidores` - Cria um novo investidor
- `GET /api/investidores` - Lista todos os investidores
- `GET /api/investidores/{id}` - Busca investidor por ID
- `PUT /api/investidores/{id}` - Atualiza investidor
- `DELETE /api/investidores/{id}` - Remove investidor

### Investimentos
- `POST /api/investimentos` - Cria um novo investimento
- `GET /api/investimentos` - Lista todos os investimentos
- `GET /api/investimentos/{id}` - Busca investimento por ID
- `PUT /api/investimentos/{id}` - Atualiza investimento
- `DELETE /api/investimentos/{id}` - Remove investimento

## 🛠️ Pré-Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) ou [Docker](https://www.docker.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)

## 🚀 Iniciando

### Usando Docker

1. Clone o repositório
   ```bash
   git clone <repository-url>
   cd backend-test/src
   ```
2. Rode com Docker Compose
   ```bash
   docker-compose up -d
   ```
3. Rode as migrations
   ```bash
   dotnet ef migrations add InitialCreate --project ProjInv.Infrastructure --startup-project ProjInv.API
   dotnet ef database update --project ProjInv.Infrastructure --startup-project ProjInv.API
   ```
4. Acesse a API
   - Health: http://localhost:8080/health
   - Swagger: http://localhost:8080/swagger

### Local

1. Clone o repositório
   ```bash
   git clone <repository-url>
   cd backend-test/src
   ```
2. Restaure dependências
   ```bash
   dotnet restore
   ```
3. Rode as migrations
   ```bash
   dotnet ef migrations add InitialCreate --project ProjInv.Infrastructure --startup-project ProjInv.API
   dotnet ef database update --project ProjInv.Infrastructure --startup-project ProjInv.API
   ```
4. Rode a aplicação
   ```bash
   dotnet run --project ProjInv.API
   ```
5. Acesse a API
   - Health: http://localhost:7057/health
   - Swagger: https://localhost:7057/swagger

## 📊 Database Schema

### Investidor
- `Id` (uniqueidentifier, PK)
- `Nome` (nvarchar)
- ...

### Investimento
- `Id` (uniqueidentifier, PK)
- `InvestidorId` (uniqueidentifier)
- ...

## 🧪 Testes

- Rode todos os testes:
  ```bash
  dotnet test
  ```
- Cobertura:
  ```bash
  powershell -ExecutionPolicy Bypass -File run_coverage.ps1
  ```

## 📁 Estrutura do Projeto

```
backend-test/
├── ProjInv.API/
├── ProjInv.Application/
├── ProjInv.Domain/
├── ProjInv.Infrastructure/
├── ProjInv.Tests/
├── docker-compose.yml
├── README.md
```

## 🔧 Configuração

- `ASPNETCORE_ENVIRONMENT`: Ambiente
- `ConnectionStrings__DefaultConnection`: String de conexão
- `ASPNETCORE_URLS`: URLs da aplicação

## Logs

- Serilog: console, arquivos rotativos, formato JSON
- Diretório: `logs/`

## Docker

- API: `8080:8080`
- SQL Server: `1433:1433`
- Volumes: `sqlserver_data`, `./logs`

## Segurança

- Validação com FluentValidation
