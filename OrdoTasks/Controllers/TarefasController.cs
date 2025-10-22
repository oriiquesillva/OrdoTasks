using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrdoTasks.Hubs;
using OrdoTasksApplication.Interfaces;
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
    }
}
