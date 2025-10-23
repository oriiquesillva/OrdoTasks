using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.Exceptions.Projects
{
    public class UnauthorizedDeleteProjectException : Exception
    {
        public UnauthorizedDeleteProjectException() : base("Não é possível excluir um projeto que possui tarefas vinculadas") { }

        public UnauthorizedDeleteProjectException(string mensagem) : base(mensagem) { }
    }
}
