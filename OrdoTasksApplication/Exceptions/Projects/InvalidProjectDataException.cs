using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.Exceptions.Projects
{
    public class InvalidProjectDataException : Exception
    {
        public InvalidProjectDataException() : base("Ooops! Para criar um projeto e obrigatório informar um nome") { }

        public InvalidProjectDataException(string mensagem) : base(mensagem) { }
    }
}
