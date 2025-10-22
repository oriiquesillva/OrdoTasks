using OrdoTasksApplication.Exceptions.Tasks;
using OrdoTasksDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.UseCases
{
    public class DeleteTaskUseCase
    {
        private readonly ITarefaRepository _tarefaRepository;

        public DeleteTaskUseCase(ITarefaRepository repository)
        {
            _tarefaRepository = repository;
        }

        public async Task Run(int id)
        {
            var verificaTarefa = await _tarefaRepository.GetByIdAsync(id);

            if (verificaTarefa == null)
            {
                throw new TarefaNaoEncontradaException();
            }

            if (verificaTarefa.Status == StatusTarefa.EmAndamento)
            {
                throw new StatusInvalidoException("Ooops! Tarefas com o status 'Em Andamento' não podem ser removidas.");
            }

            await _tarefaRepository.DeleteAsync(id);
        }

    }
}
