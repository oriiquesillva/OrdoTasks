using OrdoTasksDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.DTOs
{
    public class CreateTaskResult
    {
        public int Id { get; set; }
        public Tarefa Tarefa { get; set; }
    }
}
