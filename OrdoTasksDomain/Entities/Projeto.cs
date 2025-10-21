using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksDomain.Entities
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = default!;
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
