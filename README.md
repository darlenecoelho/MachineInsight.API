# Machine.Insight API

## 📖 Descrição

MachineInsight é uma aplicação backend em .NET 9 que simula parte do funcionamento de uma plataforma de monitoramento de máquinas pesadas utilizando telemetria em tempo real via SignalR.  

A arquitetura segue **DDD**, **Clean Code** e **SOLID**, com camadas separadas:
- **Domain**: entidades, VOs, enums, interfaces
- **Application**: serviços, DTOs, mapeamentos, validações
- **Infrastructure**: EF Core, configurações, repositórios
- **API**: controllers, hubs, startup

---

## 🚀 Funcionalidades

| Categoria                 | Endpoints / Descrição                                                                                             |
|---------------------------|-------------------------------------------------------------------------------------------------------------------|
| **CRUD de Máquinas**      | `POST /api/machines`<br>`GET /api/machines`<br>`GET /api/machines/{id}`<br>`PUT /api/machines/{id}`<br>`DELETE /api/machines/{id}` |
| **Consulta por status**   | `GET /api/machines/status/{status}`                                                                               |
| **Telemetria**            | `PUT /api/machines/{id}/telemetry` (status + RPM)<br>Simulação automática configurável<br>Broadcast via SignalR (`/hubs/telemetry`) |
| **Timestamps**            | `CreatedAt` (default `now()` no Postgres)<br>`UpdatedAt` (atualizado em UpdateDetails)                             |
| **Validação & Docs**      | DTO validators com FluentValidation<br>Documentação Swagger com XML comments                                       |

### Extras implementados

- **UpdateDetails**: `PUT /api/machines/{id}` para editar nome e localização  
- **ServiceCollectionExtensions** para startup enxuto  
- **TestConfig<T>**: base de testes com xUnit, Moq, Bogus, FluentAssertions  
- **Testes de unidade** cobrindo domínio  
- **Docker multi‐stage build** para imagem leve  

---

## 🛠️ Pré-requisitos

- .NET 9 SDK  
- Docker & Docker Compose  
- (Opcional) DBeaver  

---

## ⚙️ Configuração

Edite `appsettings.{Environment}.json` ou variáveis de ambiente:

```jsonc
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=machineinsight;Username=postgres;Password=postgres"
},
"TelemetrySimulator": {
  "IntervalSeconds": 5
}
```

Ou via shell:

```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=machineinsight;Username=postgres;Password=postgres"
export TelemetrySimulator__IntervalSeconds=10
```

---

## ▶️ Como executar

> **Importante**: todos os comandos abaixo devem ser executados no diretório **que contém** o arquivo `docker-compose.yml`.  
> Ex.: se seu repositório foi clonado em `C:\Development\MachineInsight.Api`, abra o terminal dentro dessa pasta.

### 1. Subir apenas o banco (teste local)

```bash
cd <caminho/para>/MachineInsight.Api
docker-compose up -d db
```

- Isso iniciará apenas o serviço **Postgres** em background.
- Banco disponível em `localhost:5432` (usuário `postgres` / senha `postgres`).

### 2. Rodar a API localmente

```bash
cd <caminho/para>/MachineInsight.Api/src/MachineInsight.API
dotnet run
```

- A API vai rodar em `http://localhost:5135` e `https://localhost:7157` (conforme `launchSettings.json`).

### 3. Subir tudo via Docker Compose

```bash
cd <caminho/para>/MachineInsight.Api
docker-compose up --build
```

- Sobe os serviços **db** (Postgres) e **api** (.NET) juntos.
- API disponível em `http://localhost:5000` (HTTP) e `https://localhost:5001` (HTTPS).

---

## 📄 Documentação Swagger

Após iniciar a API, acesse:

```
https://localhost:7157/swagger
```

ou

```
http://localhost:5000/swagger
```
![image](https://github.com/user-attachments/assets/e71b6dfa-e777-4e7d-9b4d-3c3d6d7ac2ac)

---

## ✅ Testes de Unidade

```bash
cd MachineInsight.Tests
dotnet test
```

- **xUnit**, **FluentAssertions**, **Moq**, **Bogus**, **AutoMapper**  
- Cobertura de domínio

---

## 📂 Estrutura do Projeto

```
src/
├── MachineInsight.API/           # Controllers, hubs, Program.cs, Extensions
├── MachineInsight.Application/   # DTOs, serviços, validações, mapeamentos
├── MachineInsight.Domain/        # Entidades, VOs, enums, interfaces
└── MachineInsight.Infrastructure# EF Core, configurações, repositórios

tests/
└── MachineInsight.Tests/         # TestConfig, MachineTests
```
