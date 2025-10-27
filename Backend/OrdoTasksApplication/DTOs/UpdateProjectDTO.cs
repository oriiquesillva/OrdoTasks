using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.DTOs
{
    public class UpdateProjectDTO
    {
        public string Nome { get; set; } = default!;
        public string? Descricao { get; set; }
    }
}
