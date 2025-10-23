using OrdoTasksDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.DTOs
{
    public class CreateProjectResult
    {
        public int Id { get; set; }
        public Projeto Projeto { get; set; }
    }
}
