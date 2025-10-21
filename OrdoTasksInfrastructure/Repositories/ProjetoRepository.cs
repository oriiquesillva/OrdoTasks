using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OrdoTasksApplication.Interfaces;
using OrdoTasksDomain.Entities;


public class ProjetoRepository : IProjetoRepository
{
    private readonly string _conn;
    public ProjetoRepository(IConfiguration config) => _conn = config.GetConnectionString("DefaultConnection");

    public async Task<int> CreateAsync(Projeto projeto)
    {
        const string sql = @"INSERT INTO Projetos (Nome, Descricao) VALUES (@Nome, @Descricao); SELECT CAST(SCOPE_IDENTITY() as int);";
        await using var conn = new SqlConnection(_conn);
        return await conn.QuerySingleAsync<int>(sql, projeto);
    }

    public async Task DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Projetos WHERE Id = @Id;";
        await using var conn = new SqlConnection(_conn);
        await conn.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<IEnumerable<Projeto>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Projetos ORDER BY DataCriacao DESC;";
        await using var conn = new SqlConnection(_conn);
        return await conn.QueryAsync<Projeto>(sql);
    }

    public async Task<Projeto?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM Projetos WHERE Id = @Id;";
        await using var conn = new SqlConnection(_conn);
        return await conn.QueryFirstOrDefaultAsync<Projeto>(sql, new { Id = id });
    }

    public async Task UpdateAsync(Projeto projeto)
    {
        const string sql = "UPDATE Projetos SET Nome = @Nome, Descricao = @Descricao WHERE Id = @Id;";
        await using var conn = new SqlConnection(_conn);
        await conn.ExecuteAsync(sql, projeto);
    }

    public async Task<bool> HasTarefasAsync(int projetoId)
    {
        const string sql = "SELECT COUNT(1) FROM Tarefas WHERE ProjetoId = @ProjetoId;";
        await using var conn = new SqlConnection(_conn);
        var count = await conn.ExecuteScalarAsync<int>(sql, new { ProjetoId = projetoId });
        return count > 0;
    }
}
