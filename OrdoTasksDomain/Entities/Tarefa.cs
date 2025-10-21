using OrdoTasksDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksDomain.Entities
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = default!;
        public string? Descricao { get; set; }
        public StatusTarefa Status { get; set; }
        public Prioridade Prioridade { get; set; }
        public int ProjetoId { get; set; }
        public string? ResponsavelId { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataPrazo { get; set; }
        public DateTime? DataConclusao { get; set; }
    }
}
