using OrdoTasksApplication.DTOs;
using OrdoTasksApplication.Exceptions.Projects;
using OrdoTasksApplication.Interfaces;
using OrdoTasksDomain.Entities;
using OrdoTasksDomain.Enums;

namespace OrdoTasksApplication.UseCases.TasksUseCases
{
    public class CreateTaskUseCase
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IProjetoRepository _projetoRepository;

        public CreateTaskUseCase(ITarefaRepository tarefaRepository, IProjetoRepository projetoRepository)
        {
            _tarefaRepository = tarefaRepository;
            _projetoRepository = projetoRepository;
        }

        public async Task<CreateTaskResult> Run(Tarefa tarefa)
        {
            var projeto = await _projetoRepository.GetByIdAsync(tarefa.ProjetoId);

            if (projeto == null)
                throw new ProjectNotFoundException();

            tarefa.Status = StatusTarefa.Pendente;
            tarefa.DataCriacao = DateTime.UtcNow;

            var id = await _tarefaRepository.CreateAsync(tarefa);
            tarefa.Id = id; 

            return new CreateTaskResult
            {
                Id = id,
                Tarefa = tarefa
            };
        }
    }
}
