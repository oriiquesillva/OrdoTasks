using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OrdoTasksDomain.Entities;
using OrdoTasksDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksInfrastructure.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly string _conn;

        public TarefaRepository(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Tarefa>> GetAllAsync(
            int? projetoId = null,
            StatusTarefa? status = null,
            string? responsavel = null,
            DateTime? prazo = null)
        {
            var sql = "SELECT * FROM Tarefas WHERE 1=1";
            var parametros = new DynamicParameters();

            if (projetoId.HasValue)
            {
                sql += " AND ProjetoId = @ProjetoId";
                parametros.Add("ProjetoId", projetoId);
            }

            if (status.HasValue)
            {
                sql += " AND Status = @Status";
                parametros.Add("Status", status);
            }

            if (!string.IsNullOrEmpty(responsavel))
            {
                sql += " AND ResponsavelId LIKE @Responsavel";
                parametros.Add("Responsavel", $"%{responsavel}%");
            }

            if (prazo.HasValue)
            {
                sql += " AND DataPrazo <= @Prazo";
                parametros.Add("Prazo", prazo);
            }

            sql += " ORDER BY DataCriacao DESC";

            await using var conn = new SqlConnection(_conn);
            return await conn.QueryAsync<Tarefa>(sql, parametros);
        }

        public async Task<Tarefa?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM Tarefas WHERE Id = @Id";
            await using var conn = new SqlConnection(_conn);
            return await conn.QueryFirstOrDefaultAsync<Tarefa>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(Tarefa tarefa)
        {
            const string sql = @"
                INSERT INTO Tarefas 
                (Titulo, Descricao, Status, Prioridade, ProjetoId, ResponsavelId, DataPrazo)
                VALUES (@Titulo, @Descricao, @Status, @Prioridade, @ProjetoId, @ResponsavelId, @DataPrazo);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            await using var conn = new SqlConnection(_conn);
            return await conn.QuerySingleAsync<int>(sql, tarefa);
        }

        public async Task UpdateAsync(Tarefa tarefa)
        {
            const string sql = @"
                UPDATE Tarefas 
                SET Titulo = @Titulo, 
                    Descricao = @Descricao, 
                    Status = @Status, 
                    Prioridade = @Prioridade, 
                    ProjetoId = @ProjetoId, 
                    ResponsavelId = @ResponsavelId, 
                    DataPrazo = @DataPrazo,
                    DataConclusao = @DataConclusao
                WHERE Id = @Id;";

            await using var conn = new SqlConnection(_conn);
            await conn.ExecuteAsync(sql, tarefa);
        }

        public async Task UpdateStatusAsync(int id, StatusTarefa novoStatus)
        {
            string sql = "UPDATE Tarefas SET Status = @Status";

            // Define DataConclusao automaticamente quando concluída
            if (novoStatus == StatusTarefa.Concluida)
                sql += ", DataConclusao = GETUTCDATE()";

            sql += " WHERE Id = @Id";

            await using var conn = new SqlConnection(_conn);
            await conn.ExecuteAsync(sql, new { Id = id, Status = novoStatus });
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Tarefas WHERE Id = @Id";
            await using var conn = new SqlConnection(_conn);
            await conn.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Tarefa>> GetAtrasadasAsync()
        {
            const string sql = @"
                SELECT * FROM Tarefas 
                WHERE DataPrazo < GETUTCDATE() 
                AND Status <> @Concluida
                AND Status <> @Cancelada";

            await using var conn = new SqlConnection(_conn);
            return await conn.QueryAsync<Tarefa>(sql, new
            {
                Concluida = StatusTarefa.Concluida,
                Cancelada = StatusTarefa.Cancelada
            });
        }

        public async Task<int> CountByStatusAsync(StatusTarefa status)
        {
            const string sql = "SELECT COUNT(1) FROM Tarefas WHERE Status = @Status";
            await using var conn = new SqlConnection(_conn);
            return await conn.ExecuteScalarAsync<int>(sql, new { Status = status });
        }
    }
}
