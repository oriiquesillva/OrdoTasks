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

            await _hub.Clients.All.SendAsync("Uma nova tarefa foi criada", tarefa);

            return CreatedAtAction(nameof(GetTaskById), new { id = result }, tarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Tarefa tarefa)
        {
            var verificaTarefa = await _projetoRepository.GetByIdAsync(id);

            if (verificaTarefa == null)
            {
                return BadRequest(new { message = "Ooops! Não foi possível localizer essa tarefa." });
            }

            tarefa.Id = id;

            await _tarefaRepository.UpdateAsync(tarefa);

            await _hub.Clients.All.SendAsync("Uma Tarefa foi atualizada", tarefa);

            return NoContent();
        }
    }
}
