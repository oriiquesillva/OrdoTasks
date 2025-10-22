using Microsoft.AspNetCore.Mvc;
using OrdoTasksApplication.Exceptions.Tasks;
using OrdoTasksDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.UseCases
{
    public class UpdateTaskUseCase
    {
        private readonly ITarefaRepository _tarefaRepository;
        public UpdateTaskUseCase(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task Run(int id, Tarefa tarefa) 
        {
            var verificaTarefa = await _tarefaRepository.GetByIdAsync(id);

            if (verificaTarefa == null)
            {
                throw new TarefaNaoEncontradaException();
            }

            tarefa.Id = id;

            await _tarefaRepository.UpdateAsync(tarefa);
        }
    }
}
