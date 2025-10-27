
-- Criação do Banco de Dados

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'OrdoTasksDB')
BEGIN
    CREATE DATABASE OrdoTasksDB;
END
GO

USE OrdoTasksDB;
GO

-- Criação das Tabelas (se não existirem)

IF OBJECT_ID('dbo.Projetos', 'U') IS NULL
BEGIN
    CREATE TABLE Projetos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nome NVARCHAR(200) NOT NULL,
        Descricao NVARCHAR(MAX) NULL,
        DataCriacao DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END

IF OBJECT_ID('dbo.Tarefas', 'U') IS NULL
BEGIN
    CREATE TABLE Tarefas (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Titulo NVARCHAR(300) NOT NULL,
        Descricao NVARCHAR(MAX) NULL,
        Status INT NOT NULL, -- mapear para enum
        Prioridade INT NOT NULL,
        ProjetoId INT NOT NULL,
        ResponsavelId NVARCHAR(100) NULL,
        DataCriacao DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        DataPrazo DATETIME2 NULL,
        DataConclusao DATETIME2 NULL,
        CONSTRAINT FK_Tarefas_Projetos FOREIGN KEY (ProjetoId) REFERENCES Projetos(Id)
    );

    CREATE INDEX IX_Tarefas_Status ON Tarefas(Status);
    CREATE INDEX IX_Tarefas_ProjetoId ON Tarefas(ProjetoId);
    CREATE INDEX IX_Tarefas_DataPrazo ON Tarefas(DataPrazo);
END
GO

-- Inserção de Dados (somente se vazio)

IF NOT EXISTS (SELECT 1 FROM Projetos)
BEGIN
    PRINT('Inserindo projetos iniciais...');
    
    INSERT INTO Projetos (Nome, Descricao)
    VALUES 
    ('Projeto A - Gestão Interna', 'Sistema de gestão de tarefas e processos internos da empresa.'),
    ('Projeto B - Plataforma OrdoTasks', 'Aplicação principal da plataforma OrdoTasks.'),
    ('Projeto C - Integrações e APIs', 'Criação e manutenção de APIs e integrações externas.');
END
GO

IF NOT EXISTS (SELECT 1 FROM Tarefas)
BEGIN
    PRINT('⏳ Inserindo tarefas iniciais...');
    DECLARE @ProjetoId INT;
    DECLARE @i INT = 1;

    WHILE @i <= 3
    BEGIN
        SET @ProjetoId = @i;

        -- 3 Pendentes (1 atrasada, 2 dentro do prazo)
        INSERT INTO Tarefas (Titulo, Descricao, Status, Prioridade, ProjetoId, ResponsavelId, DataPrazo, DataCriacao)
        VALUES
        ('Definir requisitos detalhados', 'Reunião com stakeholders para definir escopo detalhado.', 0, 2, @ProjetoId, 'user01', DATEADD(DAY, -3, GETUTCDATE()), DATEADD(DAY, -10, GETUTCDATE())), -- atrasada
        ('Documentar arquitetura do sistema', 'Especificar camadas e dependências do projeto.', 0, 1, @ProjetoId, 'user02', DATEADD(DAY, 2, GETUTCDATE()), DATEADD(DAY, -7, GETUTCDATE())),
        ('Organizar backlog inicial', 'Criar a lista inicial de tarefas para o sprint 1.', 0, 0, @ProjetoId, 'user03', DATEADD(DAY, 5, GETUTCDATE()), DATEADD(DAY, -3, GETUTCDATE()));

        -- 3 Em Andamento
        INSERT INTO Tarefas (Titulo, Descricao, Status, Prioridade, ProjetoId, ResponsavelId, DataPrazo, DataCriacao)
        VALUES
        ('Desenvolver tela de login', 'Implementar autenticação e validação de credenciais.', 1, 3, @ProjetoId, 'user01', DATEADD(DAY, 2, GETUTCDATE()), DATEADD(DAY, -5, GETUTCDATE())),
        ('Criar dashboard principal', 'Montar layout e componentes do painel de controle.', 1, 2, @ProjetoId, 'user02', DATEADD(DAY, 3, GETUTCDATE()), DATEADD(DAY, -4, GETUTCDATE())),
        ('Integrar API de usuários', 'Conectar o backend ao serviço de autenticação.', 1, 1, @ProjetoId, 'user03', DATEADD(DAY, 1, GETUTCDATE()), DATEADD(DAY, -6, GETUTCDATE()));

        -- 3 Concluídas
        INSERT INTO Tarefas (Titulo, Descricao, Status, Prioridade, ProjetoId, ResponsavelId, DataPrazo, DataConclusao, DataCriacao)
        VALUES
        ('Criar estrutura do banco de dados', 'Modelagem inicial de tabelas e relacionamentos.', 2, 1, @ProjetoId, 'user01', DATEADD(DAY, -7, GETUTCDATE()), DATEADD(DAY, -8, GETUTCDATE()), DATEADD(DAY, -10, GETUTCDATE())),
        ('Configurar ambiente de desenvolvimento', 'Setup de ferramentas e dependências.', 2, 0, @ProjetoId, 'user02', DATEADD(DAY, -5, GETUTCDATE()), DATEADD(DAY, -6, GETUTCDATE()), DATEADD(DAY, -9, GETUTCDATE())),
        ('Gerar protótipos iniciais', 'Criação de mockups no Figma.', 2, 2, @ProjetoId, 'user03', DATEADD(DAY, -4, GETUTCDATE()), DATEADD(DAY, -5, GETUTCDATE()), DATEADD(DAY, -7, GETUTCDATE()));

        -- 3 Canceladas
        INSERT INTO Tarefas (Titulo, Descricao, Status, Prioridade, ProjetoId, ResponsavelId, DataPrazo, DataCriacao)
        VALUES
        ('Reunião de revisão cancelada', 'Cancelada por indisponibilidade da equipe.', 3, 0, @ProjetoId, 'user01', DATEADD(DAY, -1, GETUTCDATE()), DATEADD(DAY, -3, GETUTCDATE())),
        ('Teste com serviço de terceiros', 'API descontinuada, tarefa cancelada.', 3, 1, @ProjetoId, 'user02', DATEADD(DAY, -6, GETUTCDATE()), DATEADD(DAY, -8, GETUTCDATE())),
        ('Migração de dados legados', 'Cancelada após mudança no escopo.', 3, 2, @ProjetoId, 'user03', DATEADD(DAY, -10, GETUTCDATE()), DATEADD(DAY, -11, GETUTCDATE()));

        SET @i = @i + 1;
    END
END
GO

PRINT('Banco de dados OrdoTasksDB configurado com sucesso!');
