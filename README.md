
# OrdoTasks

Projeto de gestão simples de tarefas e projetos com dashboard de métricas. Implementa API em .NET 8, frontend em Angular (v20) e persistência em SQL Server. Permite criar/editar/excluir projetos e tarefas, atualizar status, e consultar métricas do dashboard.

## 1. Descrição do Projeto
OrdoTasks é uma aplicação fullstack para organização de projetos e tarefas com:
- Backend REST em ASP.NET Core (.NET 8).
- Frontend SPA em Angular (v20) com serviço de API.
- Banco SQL Server com script de criação em [sql/init.sql](sql/init.sql).

## 2. Tecnologias Utilizadas
- Backend: .NET 8, ASP.NET Core Web API
  - Principais libs: Dapper, Microsoft.Data.SqlClient, Serilog
  - Estrutura: Domain / Application (Use Cases) / Infrastructure / API
- Frontend: Angular 20, TypeScript, RxJS, Angular Router
- Banco de dados: Microsoft SQL Server
- Infra/Deploy: Docker, Docker Compose, Nginx (frontend em produção)
- Ferramentas dev: dotnet CLI, Angular CLI, npm/yarn

## 3. Como Executar

Pré-requisitos
- Docker & Docker Compose (recomendado)
- Alternativa sem Docker: .NET 8 SDK, Node 20 (ou compatível), npm ou yarn, SQL Server local (ou em container)

Rodando com Docker (recomendado)
    **OrdoTasks — Docker Compose Setup**

Aplicação completa do **OrdoTasks**, composta por:

- **Frontend** — Angular + Nginx  
- **Backend** — ASP.NET Core (.NET 8) com Dapper  
- **Banco de Dados** — Microsoft SQL Server 2022  

O projeto foi configurado para ser executado inteiramente via **Docker Compose**, com os três serviços se comunicando automaticamente.

---

## Estrutura do projeto**

```
OrdoTasks.v1/
│
├── Backend/
│   └── Dockerfile
│
├── Frontend/
│   ├── Dockerfile
│   └── nginx.conf
│
├── sql/
│   └── init.sql
│
├── .env
└── docker-compose.yml
```

---

## 3.1. Configuração do ambiente**

Crie um arquivo **`.env`** na raiz do projeto com o seguinte conteúdo:

```env
# BACKEND
ASPNETCORE_ENVIRONMENT=Development
BACKEND_PORT=5257

# FRONTEND
FRONTEND_PORT=4200

# SQL SERVER CONFIG
DB_IMAGE=mcr.microsoft.com/mssql/server:2022-latest
DB_CONTAINER_NAME=ordotasks-db
DB_NAME=OrdoTasksDB
DB_USER=sa
DB_PASSWORD=YourStrong!Pass123
DB_PORT=1433

# NETWORK
NETWORK_NAME=ordonet
```

---

## 3.2. Build do projeto**

Construa todas as imagens (backend, frontend e banco):

```bash
docker-compose build
```

---

## 3.3 Subir a aplicação**

Para iniciar todos os containers:

```bash
docker-compose up
```

ou, para rodar em segundo plano:

```bash
docker-compose up -d
```

---

## 3.4. Verificar se está rodando**

Confira os containers ativos:

```bash
docker ps
```

Você deve ver algo como:

```
CONTAINER ID   IMAGE              PORTS                    NAMES
a1b2c3d4e5f6   ordotasks-frontend  0.0.0.0:4200->80/tcp     ordotasks-frontend
b2c3d4e5f6g7   ordotasks-backend   0.0.0.0:5257->8080/tcp   ordotasks-backend
c3d4e5f6g7h8   mcr.microsoft.com/mssql/server:2022-latest  0.0.0.0:1433->1433/tcp ordotasks-db
```

---

## 3.5 Acessar a aplicação**

| Serviço | URL | Descrição |
|----------|-----|-----------|
| 🖥️ **Frontend** | [http://localhost:4200](http://localhost:4200) | Interface Angular do OrdoTasks |
| ⚙️ **Backend (.NET)** | [http://localhost:5257/swagger](http://localhost:5257/swagger) | API e documentação via Swagger |
| 🗄️ **Banco de Dados** | `localhost,1433` | Conexão via SSMS, Azure Data Studio ou DBeaver |

---

## 3.6 Logs e manutenção**

### Ver logs em tempo real:
```bash
docker-compose logs -f
```

### Parar os containers:
```bash
docker-compose down
```

### Parar e remover volumes (zera o banco):
```bash
docker-compose down -v
```

### Rebuild completo do zero:
```bash
docker-compose down -v
docker-compose build --no-cache
docker-compose up 
```

---

## 3.7 Verificando o banco SQL**

Para acessar o SQL Server dentro do container:

```bash
docker exec -it ordotasks-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong!Pass123 -Q "SELECT name FROM sys.databases;"
```

Resultado esperado:
```
name
-------------------
master
tempdb
model
msdb
OrdoTasksDB
```

---

## 3.8 Estrutura do banco de dados**

O banco é criado automaticamente com base no script `sql/init.sql`:

```sql
CREATE DATABASE OrdoTasksDB;
GO
USE OrdoTasksDB;
GO

CREATE TABLE Projetos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(200) NOT NULL,
    Descricao NVARCHAR(MAX) NULL,
    DataCriacao DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE Tarefas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(300) NOT NULL,
    Descricao NVARCHAR(MAX) NULL,
    Status INT NOT NULL,
    Prioridade INT NOT NULL,
    ProjetoId INT NOT NULL,
    ResponsavelId NVARCHAR(100) NULL,
    DataCriacao DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    DataPrazo DATETIME2 NULL,
    DataConclusao DATETIME2 NULL,
    CONSTRAINT FK_Tarefas_Projetos FOREIGN KEY (ProjetoId) REFERENCES Projetos(Id)
);
```

---

##  3.9 Dicas adicionais**

# Ver arquivos dentro de um container
```bash
docker exec -it ordotasks-frontend sh
ls /usr/share/nginx/html
```

# Reiniciar somente um serviço
```bash
docker-compose restart frontend
```

# Atualizar apenas o frontend
```bash
docker-compose build frontend
docker-compose up  frontend
```

---

## 3.10 Resultado final**

Após subir tudo, você terá:

**Frontend (Angular)** rodando em: `http://localhost:4200`  
**Backend (.NET)** rodando em: `http://localhost:5257/swagger`  
**Banco SQL Server** rodando em: `localhost,1433`  

Tudo interligado na rede `ordonet`, totalmente automatizado com Docker Compose. 

## 4. Rodando sem Docker.

Requisitos
- .NET 8 SDK instalado (dotnet) — verificar: dotnet --version
- Node 20+ e npm/yarn — verificar: node -v && npm -v
- SQL Server acessível (instância local, Docker opcional apenas para o banco, ou Azure SQL)
- Ferramentas opcionais: SQL Server Management Studio / Azure Data Studio / sqlcmd

## 4.1 Preparar o banco de dados

- Opção A — SQL Server local (Windows / SSMS / Azure Data Studio)
  1. Criar uma base vazia ou executar o script `sql/init.sql` (ex.: via SSMS).
  2. Ajustar o nome do banco se necessário (padrão sugerido: OrdoTasksDB).

- Opção B — Usando sqlcmd (PowerShell)
  Abra PowerShell e execute:
  ```powershell
  sqlcmd -S localhost -U sa -P "SuaSenhaForte!" -i ".\sql\init.sql"
  ```
  (Substitua localhost/porta e senha conforme sua instância.)

Observação: se sua instância do SQL Server usa outra porta, ajuste `-S localhost,1433` ou `serverName\instance`.

## 4.2 Configurar conexão do Backend
- Atualize a connection string de desenvolvimento:
  - Arquivo: `Backend/OrdoTasks/appsettings.Development.json`
  - Ou via variáveis de ambiente (recomendado para evitar comitar segredos).

Exemplo de connection string:
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=OrdoTasksDB;User Id=sa;Password=SuaSenhaForte!;"
}

Alternativa via PowerShell (temporária para a sessão):
```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost,1433;Database=OrdoTasksDB;User Id=sa;Password=SuaSenhaForte!;"
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:ASPNETCORE_URLS = "http://localhost:5257"
```

## 4.3 Rodar o Backend (.NET)
Abra um terminal na pasta do backend e execute:
```powershell
cd Backend/OrdoTasks
dotnet restore
dotnet build
dotnet run
```
- Em ambiente Development o launchSettings normalmente expõe a API em http://localhost:5257
- Se usar variável ASPNETCORE_URLS, ajuste a URL conforme necessário.

Verificar: acesse http://localhost:5257/swagger para ver endpoints (se o Swagger estiver habilitado em Development).

## 4.4 Rodar o Frontend (Angular)
Abra outro terminal na pasta do frontend:
```powershell
cd Frontend
npm install
npm run start
```
ou, se usar Angular CLI diretamente:
```powershell
npx ng serve --host 0.0.0.0 --port 4200 --open
```
- O dev server padrão abre em http://localhost:4200
- Certifique-se que o environment de frontend aponte para a URL da API (arquivo: `Frontend/src/environments/environment.ts` ou `environment.dev.ts`)
  - Ajuste `apiUrl` para `http://localhost:5257` (ou porta utilizada).

## 4.5 Credenciais padrão (exemplo)
- SQL Server:
  - Usuário: sa
  - Senha: definida por você (ex.: SuaSenhaForte!)
- API / Frontend:
  - Se a aplicação tiver autenticação, use as credenciais de teste documentadas no projeto (caso não existam, crie um usuário via endpoint ou DB).


## 4.6 Variáveis úteis e dicas (Windows PowerShell)
- Definir variável temporária na sessão:
  ```powershell
  $env:ASPNETCORE_ENVIRONMENT="Development"
  $env:ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=OrdoTasksDB;User Id=sa;Password=SuaSenhaForte!;"
  ```
- Se houver problemas de conexão:
  - Verifique se o SQL Server aceita conexões TCP/IP (SQL Server Configuration Manager).
  - Confirme firewall e porta (1433).
  - Teste conexão com sqlcmd ou SSMS.

## 4.7 Problemas comuns e soluções
- Erro de autenticação ao conectar ao DB:
  - Confirme user/password e que a autenticação SQL está habilitada.
- Erro de timeout ao iniciar a API:
  - Verifique connection string e se o SQL Server está aceitando conexões externas.
- Frontend não consegue chamar API (CORS):
  - Ative CORS no backend ou rode frontend por proxy. Verifique configuração em Program.cs/Startup.

## 4.8 Exemplo de fluxo completo (PowerShell)

    ```powershell
    # 1. Criar DB (sqlcmd)
    sqlcmd -S localhost -U sa -P "SuaSenhaForte!" -i ".\sql\init.sql"

    # 2. Ajustar env vars para backend
    $env:ConnectionStrings__DefaultConnection = "Server=localhost,1433;Database=OrdoTasksDB;User Id=sa;Password=SuaSenhaForte!;"
    $env:ASPNETCORE_ENVIRONMENT = "Development"
    $env:ASPNETCORE_URLS = "http://localhost:5257"

    # 3. Rodar backend
    cd Backend/OrdoTasks
    dotnet run

    # 4. Em outro terminal, rodar frontend
    cd Frontend
    npm install
    npm run start
    ```

## 5. Decisões Técnicas

Arquitetura
- Separação em camadas (Domain / Application / Infrastructure / API) para isolar regras de negócio, facilitar testes e permitir troca de infra sem afetar regras.
- Clean/Hexagonal style: Use Cases na camada Application tratam regras, controllers apenas orquestram input/output.

Patterns aplicados
- Use Case / Interactor: mantém regras de negócio fora dos controllers.
- Repository / DAO: abstrai acesso a dados (implementação com Dapper).
- DTOs/Contracts: comunicação entre camadas e com o frontend para controle de versão e validação.
- Logging com Serilog para observabilidade.

Por que essas escolhas?
- Dapper escolhido por performance e controle direto sobre queries (trade-off: mais SQL manual vs. produtividade do EF).
- Arquitetura em camadas para escalabilidade e testabilidade — facilita testes unitários de regras sem dependências de infraestrutura.
- Docker para padronizar ambiente e facilitar CI/CD.

Trade-offs considerados
- Adoção de Dapper reduz boilerplate de ORM, mas exige cuidado com SQL e mapeamentos.
- Estrutura em camadas introduz boilerplate inicial, mas melhora manutenção em projetos médios/grandes.
- Não usar EF evita migrações automáticas; exige scripts SQL (mais controle).

## 6. Uso de IA no Desenvolvimento
- Ferramentas usadas: GitHub Copilot ChatGPT Claude.AI para:
  - Utilizei as ferramentas para boilerplate, controllers simples, DTOs, SCSS, ajuste em código, refatoração.
  - Revisão de HTML, retorno de erros e etc.
  - Revisão rápida de mensagens de commit e README inicial.
- Para que foi usado:
  - Acelerar criação da aplicação utilziando as melhoreas e otimizando o tempo de desenvolvimento.
- O que foi adaptado:
  - Todo código sugerido por IA foi revisado manualmente, ajustado ao estilo do projeto, testado e modificado quando necessário.

## 7. Melhorias Futuras
O que faria com mais tempo:
- Implementar autenticação e autorização (JWT + políticas/roles).
- Escrever suíte completa de testes (unitários e de integração) e integrar CI/CD.
- Adicionar cobertura de código e métricas (coverlet, SonarCloud).
- Adicionar WebSocket/SignalR para atualizações em tempo real do dashboard.

Funcionalidades desejadas:
- Compartilhamento de projetos entre usuários e permissões por projeto.
- Templates de projeto e import/export.
- Integração com calendários e notificações por e-mail.

## Arquivos importantes
- docker-compose.yml
- .env.example
- Backend/OrdoTasks (API)
- Backend/OrdoTasksApplication (Use Cases)
- Backend/OrdoTasksInfrastructure (Repos, DB)
- Frontend/ (Angular app)
- sql/init.sql
