using System;

namespace OrdoTasksApplication.Exceptions.Tasks
{
    public class StatusInvalidoException : Exception
    {
        public StatusInvalidoException(string mensagem) : base(mensagem) { }
    }
}