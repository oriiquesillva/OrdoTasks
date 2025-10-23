using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrdoTasks.Hubs;
using OrdoTasksApplication.Exceptions.Projects;
using OrdoTasksApplication.Exceptions.Tasks;
using OrdoTasksApplication.Interfaces;
using OrdoTasksApplication.UseCases.TasksUseCases;
using OrdoTasksDomain.Entities;
using OrdoTasksDomain.Enums;

namespace OrdoTasks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefasController : ControllerBase
    {
        private readonly IOrdoTasksTaskRepository _tasksrepository;
        private readonly IProjetoRepository _projetoRepository;
        private readonly GetAllTasksUseCase _getAllTasksUseCase;
        private readonly GetTaskByIdUseCase _getTaskByIdUseCase;
        private readonly CreateTaskUseCase _createTaskUseCase;
        private readonly UpdateTaskUseCase _updateTaskUseCase;
        private readonly UpdateTaskStatusUseCase _updateTaskStatusUseCase;
        private readonly DeleteTaskUseCase _deleteTaskUseCase;
        private readonly GetDelayedTaskUseCase _getDelayedTaskUseCase;
        private readonly IHubContext<NotificationHub> _hub;

        public TarefasController(IOrdoTasksTaskRepository tasksrepository,
            IProjetoRepository projetoRepository,
            GetAllTasksUseCase getAllTasksUseCase,
            GetTaskByIdUseCase getTaskByIdUseCase,
            CreateTaskUseCase createTaskUseCase,
            UpdateTaskUseCase updateTaskUseCase,
            UpdateTaskStatusUseCase updateTaskStatusUseCase,
            DeleteTaskUseCase deleteTaskUseCase,
            GetDelayedTaskUseCase getDelayedTaskUseCase,
            IHubContext<NotificationHub> hub)
        {
            _tasksrepository = tasksrepository;
            _projetoRepository = projetoRepository;
            _getAllTasksUseCase = getAllTasksUseCase;
            _getTaskByIdUseCase = getTaskByIdUseCase;
            _createTaskUseCase = createTaskUseCase;
            _updateTaskUseCase = updateTaskUseCase;
            _updateTaskStatusUseCase = updateTaskStatusUseCase;
            _deleteTaskUseCase = deleteTaskUseCase;
            _getDelayedTaskUseCase = getDelayedTaskUseCase;
            _hub = hub;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks([FromQuery] int? projetoId, [FromQuery] StatusTarefa? status, [FromQuery] string? responsavel, [FromQuery] DateTime? prazo)
        {
            try
            {
                var result = await _getAllTasksUseCase.Run(projetoId, status, responsavel, prazo);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var tarefa = await _getTaskByIdUseCase.Run(id);
                return Ok(tarefa);
            }
            catch (TarefaNaoEncontradaException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] Tarefa tarefa)
        {
            try
            {
                var result = await _createTaskUseCase.Run(tarefa);
                await _hub.Clients.All.SendAsync("Uma tarefa foi criada", tarefa);

                return CreatedAtAction(nameof(GetTaskById), new { id = result.Id }, result.Tarefa);
            }
            catch (ProjectNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro interno no servidor." });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Tarefa tarefa)
        {
            try
            {
                await _updateTaskUseCase.Run(id, tarefa);

                await _hub.Clients.All.SendAsync("Uma Tarefa foi atualizada", tarefa);

                return NoContent();
            }
            catch (TarefaNaoEncontradaException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro interno no servidor." });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] StatusTarefa novoStatus)
        {
            try
            {
                await _updateTaskStatusUseCase.Run(id, novoStatus);

                return NoContent();
            }
            catch (TarefaNaoEncontradaException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (StatusInvalidoException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _deleteTaskUseCase.Run(id);

                await _hub.Clients.All.SendAsync("Uma tarefa foi removida", id);

                return NoContent();
            }
            catch (StatusInvalidoException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro interno no servidor." });
            }
        }

        [HttpGet("atrasadas")]
        public async Task<IActionResult> GetDelayedTasks()
        {
            try
            {
                var result = await _getDelayedTaskUseCase.Run();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor." });
            }
        }
    }
}
