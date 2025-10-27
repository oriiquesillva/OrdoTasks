using Microsoft.AspNetCore.Mvc;
using OrdoTasksApplication.DTOs;
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
    public class UpdateTaskUseCase
    {
        private readonly IOrdoTasksTaskRepository _tarefaRepository;
        public UpdateTaskUseCase(IOrdoTasksTaskRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task Run(int id, UpdateTaskDTO tarefaDTO)
        {
            var verificaTarefa = await _tarefaRepository.GetByIdAsync(id);

            if (verificaTarefa == null)
                throw new TarefaNaoEncontradaException();

            var tarefa = new Tarefa
            {
                Id = id,
                Titulo = tarefaDTO.Titulo,
                Descricao = tarefaDTO.Descricao,
                Status = tarefaDTO.Status,
                Prioridade = tarefaDTO.Prioridade,
                ProjetoId = tarefaDTO.ProjetoId,
                ResponsavelId = tarefaDTO.ResponsavelId,
                DataPrazo = tarefaDTO.DataPrazo,
                DataCriacao = verificaTarefa.DataCriacao,
                DataConclusao = tarefaDTO.Status == StatusTarefa.Concluida
                    ? DateTime.UtcNow
                    : verificaTarefa.DataConclusao
            };

            await _tarefaRepository.UpdateAsync(tarefa);
        }
    }
}
