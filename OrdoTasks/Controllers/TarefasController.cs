using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrdoTasks.Hubs;
using OrdoTasksApplication.Interfaces;
using OrdoTasksDomain.Entities;
using OrdoTasksDomain.Enums;

namespace OrdoTasks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefasController : ControllerBase
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IProjetoRepository _projetoRepository;
        private readonly IHubContext<NotificationHub> _hub;

        public TarefasController(ITarefaRepository tarefaRepository, IProjetoRepository projetoRepository, IHubContext<NotificationHub> hub)
        {
            _tarefaRepository = tarefaRepository;
            _projetoRepository = projetoRepository;
            _hub = hub;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks([FromQuery] int? projetoId, [FromQuery] StatusTarefa? status, [FromQuery] string? responsavel, [FromQuery] DateTime? prazo)
        {
            var tarefas = await _tarefaRepository.GetAllAsync(projetoId, status, responsavel, prazo);

            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(id);

            if (tarefa == null)
            {
                return NotFound(new { message = "Ooops! Não foi possível localizar essa tarefa" });
            }

            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] Tarefa tarefa)
        {
            var projeto = await _projetoRepository.GetByIdAsync(tarefa.ProjetoId);

            if (projeto == null)
            {
                return BadRequest(new { message = "Ooops! Não foi possível localizer esse projeto." });
            }

            tarefa.Status = StatusTarefa.Pendente;
            tarefa.DataCriacao = DateTime.UtcNow;

            var result = await _tarefaRepository.CreateAsync(tarefa);

            await _hub.Clients.All.SendAsync("Uma tarefa foi criada", tarefa);

            return CreatedAtAction(nameof(GetTaskById), new { id = result }, tarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Tarefa tarefa)
        {
            var verificaTarefa = await _tarefaRepository.GetByIdAsync(id);

            if (verificaTarefa == null)
            {
                return NotFound(new { message = "Ooops! Não foi possível localizar essa tarefa." });
            }

            tarefa.Id = id;

            await _tarefaRepository.UpdateAsync(tarefa);

            await _hub.Clients.All.SendAsync("Uma Tarefa foi atualizada", tarefa);

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] StatusTarefa novoStatus)
        {
            var verificaTarefa = await _tarefaRepository.GetByIdAsync(id);

            if (verificaTarefa == null)
            {
                return NotFound(new { message = "Ooops! Não foi possível localizar essa tarefa." });
            }

            if (novoStatus == StatusTarefa.EmAndamento && verificaTarefa.Status != StatusTarefa.Pendente)
            {
                return BadRequest(new {message = "Ooops! O status da tarefa só pode alterado para 'Em Andamento' se estiver com o status 'Pendente'." });
            }

            if (novoStatus == StatusTarefa.Concluida && verificaTarefa.Status != StatusTarefa.EmAndamento)
            {
                return BadRequest(new { message = "Ooops! O status da tarefa só pode alterado para 'Concluída' se estiver com o status 'Em Andamento'." });
            }

            await _tarefaRepository.UpdateStatusAsync(id, novoStatus);

            await _hub.Clients.All.SendAsync("O status de uma tarefa foi alterado", new { id, novoStatus });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var verificaTarefa = await _tarefaRepository.GetByIdAsync(id);

            if (verificaTarefa == null)
            {
                return NotFound(new { message = "Ooops! Não foi possível localizar essa tarefa." });
            }

            if (verificaTarefa.Status == StatusTarefa.EmAndamento)
            {
                return BadRequest(new { message = "Ooops! Não é possível excluir uma tarefa que está em andamento" });
            }

            await _tarefaRepository.DeleteAsync(id);

            await _hub.Clients.All.SendAsync("Uma tarefa foi removida", id);

            return NoContent();
        }

        [HttpGet("atrasadas")]
        public async Task<IActionResult> GetDelayedTasks(int id)
        {
            var tarefas = await _tarefaRepository.GetByIdAsync(id);

            return Ok(tarefas);
        }
    }
}
