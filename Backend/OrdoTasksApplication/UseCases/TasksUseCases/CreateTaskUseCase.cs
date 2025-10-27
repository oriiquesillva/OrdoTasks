using OrdoTasksApplication.DTOs;
using OrdoTasksApplication.Exceptions.Projects;
using OrdoTasksApplication.Interfaces;
using OrdoTasksDomain.Entities;
using OrdoTasksDomain.Enums;

namespace OrdoTasksApplication.UseCases.TasksUseCases
{
    public class CreateTaskUseCase
    {
        private readonly IOrdoTasksTaskRepository _tarefaRepository;
        private readonly IOrdoTasksProjectRepository _projetoRepository;

        public CreateTaskUseCase(IOrdoTasksTaskRepository tarefaRepository, IOrdoTasksProjectRepository projetoRepository)
        {
            _tarefaRepository = tarefaRepository;
            _projetoRepository = projetoRepository;
        }

        public async Task<CreateTaskResult> Run(CreateTaskDTO tarefaDto)
        {
            var projeto = await _projetoRepository.GetByIdAsync(tarefaDto.ProjetoId);

            if (projeto == null)
                throw new ProjectNotFoundException();

            var tarefa = new Tarefa
            {
                Titulo = tarefaDto.Titulo,
                Descricao = tarefaDto.Descricao,
                Prioridade = tarefaDto.Prioridade,
                ProjetoId = tarefaDto.ProjetoId,
                ResponsavelId = tarefaDto.ResponsavelId,
                Status = StatusTarefa.Pendente,
                DataPrazo = tarefaDto.DataPrazo,
                DataCriacao = DateTime.UtcNow 
            };

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
