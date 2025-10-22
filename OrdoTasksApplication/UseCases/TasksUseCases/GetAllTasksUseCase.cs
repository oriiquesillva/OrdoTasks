using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    public class GetAllTasksUseCase
    {
        private readonly ITarefaRepository _repository;

        public GetAllTasksUseCase(ITarefaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Tarefa>> Run(int? projetoId, [FromQuery] StatusTarefa? status, [FromQuery] string? responsavel, [FromQuery] DateTime? prazo)
        {
           var result =  await _repository.GetAllAsync(projetoId, status, responsavel, prazo);

            return result.ToList();
        }
    }
}
