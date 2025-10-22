using OrdoTasksApplication.Exceptions.Tasks;
using OrdoTasksDomain.Entities;
using OrdoTasksDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.UseCases.TasksUseCases
{
    public class GetTaskByIdUseCase
    {
        private readonly ITarefaRepository _tarefaRepository;
        public GetTaskByIdUseCase(ITarefaRepository repository)
        {
            _tarefaRepository = repository;
        }

        public async Task<Tarefa> Run(int id)
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(id);

            if (tarefa == null)
                throw new TarefaNaoEncontradaException();

            return tarefa;
        }
    }
}
