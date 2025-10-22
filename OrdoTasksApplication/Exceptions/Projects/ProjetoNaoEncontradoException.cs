using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.Exceptions.Projects
{
    public class ProjetoNaoEncontradoException : Exception
    {
        public ProjetoNaoEncontradoException() : base("Ooops! Não foi possível localizar esse Projeto.") { }

        public ProjetoNaoEncontradoException(string mensagem) : base(mensagem) { }
    }
}
