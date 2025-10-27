using OrdoTasksDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.DTOs
{
    public class CreateTaskDTO
    {
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public Prioridade Prioridade { get; set; }
        public int ProjetoId { get; set; }
        public string? ResponsavelId { get; set; }
        public DateTime DataPrazo { get; set; }
    }
}
