using OrdoTasksApplication.Exceptions.Tasks;
using OrdoTasksApplication.Interfaces;
using OrdoTasksDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.UseCases.TasksUseCases
{
    public class GetDelayedTaskUseCase
    {
        private readonly IOrdoTasksTaskRepository _tarefaRepository;

        public GetDelayedTaskUseCase(IOrdoTasksTaskRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;

        }

        public async Task<List<Tarefa>> Run()
        {
            var tarefas = await _tarefaRepository.GetAtrasadasAsync();

            if (tarefas == null)
                throw new TarefaNaoEncontradaException();

            return tarefas.ToList();
        }
    }
}
