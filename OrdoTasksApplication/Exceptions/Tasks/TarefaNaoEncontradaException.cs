using System;

namespace OrdoTasksApplication.Exceptions.Tasks
{
    public class TarefaNaoEncontradaException : Exception
    {
        public TarefaNaoEncontradaException() : base("Ooops! Não foi possível localizar essa tarefa.") { }

        public TarefaNaoEncontradaException(string mensagem) : base(mensagem) {}
    }
}
