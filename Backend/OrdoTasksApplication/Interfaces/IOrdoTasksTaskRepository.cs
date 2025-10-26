using OrdoTasksDomain.Entities;
using OrdoTasksDomain.Enums;

public interface IOrdoTasksTaskRepository
{
    Task<IEnumerable<Tarefa>> GetAllAsync(int? projetoId = null, StatusTarefa? status = null, string? responsavel = null, DateTime? prazo = null);
    Task<Tarefa?> GetByIdAsync(int id);
    Task<int> CreateAsync(Tarefa tarefa);
    Task UpdateAsync(Tarefa tarefa);
    Task UpdateStatusAsync(int id, StatusTarefa novoStatus);
    Task DeleteAsync(int id);
    Task<IEnumerable<Tarefa>> GetAtrasadasAsync();
    Task<int> CountByStatusAsync(StatusTarefa status);
}
