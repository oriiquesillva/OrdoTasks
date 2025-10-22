using OrdoTasksDomain.Enums;
using OrdoTasksApplication.Exceptions;
using OrdoTasksApplication.Exceptions.Tasks;


namespace OrdoTasksApplication.UseCases
{
    public class UpdateTaskStatusUseCase
    {
        private readonly ITarefaRepository _repository;
        public UpdateTaskStatusUseCase(ITarefaRepository repository)
        {
            _repository = repository;
        }

        public async Task Run(int id, StatusTarefa novoStatus)
        {
            var verificaTarefa = await _repository.GetByIdAsync(id);

            if (verificaTarefa == null)
                throw new TarefaNaoEncontradaException();

            if (novoStatus == StatusTarefa.EmAndamento && verificaTarefa.Status != StatusTarefa.Pendente)
                throw new StatusInvalidoException("Ooops! O status da tarefa só pode ser alterado para 'Em Andamento' se estiver com o status 'Pendente'.");

            if (novoStatus == StatusTarefa.Concluida && verificaTarefa.Status != StatusTarefa.EmAndamento)
                throw new StatusInvalidoException("Ooops! O status da tarefa só pode ser alterado para 'Concluída' se estiver com o status 'Em Andamento'.");

            await _repository.UpdateStatusAsync(id, novoStatus);
        }
    }
}
