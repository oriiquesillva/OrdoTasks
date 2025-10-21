using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.DTOs
{
    public record ProjetoDTO(int Id, string Nome, string? Descricao, DateTime DataCriacao);

}
