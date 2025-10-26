using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.Exceptions.Projects
{
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException() : base("Ooops! Não foi possível localizar esse Projeto.") { }

        public ProjectNotFoundException(string mensagem) : base(mensagem) { }
    }
}
