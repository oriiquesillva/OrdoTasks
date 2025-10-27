using OrdoTasksApplication.DTOs;
using OrdoTasksDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.Interfaces
{
    public interface IOrdoTasksProjectRepository
    {
        Task<IEnumerable<Projeto>> GetAllAsync();
        Task<Projeto?> GetByIdAsync(int id);
        Task<int> CreateAsync(Projeto projeto);
        Task UpdateAsync(Projeto projeto);
        Task DeleteAsync(int id);
        Task<bool> HasTarefasAsync(int projetoId);
    }
}
