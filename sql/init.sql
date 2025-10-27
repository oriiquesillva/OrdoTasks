-- Criação das tabelas básicas
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
    Status INT NOT NULL, -- mapear para enum
    Prioridade INT NOT NULL,
    ProjetoId INT NOT NULL,
    ResponsavelId NVARCHAR(100) NULL,
    DataCriacao DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    DataPrazo DATETIME2 NULL,
    DataConclusao DATETIME2 NULL,
    CONSTRAINT FK_Tarefas_Projetos FOREIGN KEY (ProjetoId) REFERENCES Projetos(Id)
);

-- Índices úteis
CREATE INDEX IX_Tarefas_Status ON Tarefas(Status);
CREATE INDEX IX_Tarefas_ProjetoId ON Tarefas(ProjetoId);
CREATE INDEX IX_Tarefas_DataPrazo ON Tarefas(DataPrazo);
