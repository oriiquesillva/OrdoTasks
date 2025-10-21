using Microsoft.AspNetCore.Mvc;
using OrdoTasksApplication.Interfaces;
using OrdoTasksDomain.Entities;

namespace OrdoTasks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetosController : Controller
    {
        private readonly IProjetoRepository _projetoRepository;

        public ProjetosController(IProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projetos = await _projetoRepository.GetAllAsync();

            return Ok(projetos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var projeto = await _projetoRepository.GetByIdAsync(id);

            if (projeto == null)
            {
                return NotFound(new { message = "Ooops! Não foi possível localizar esse projeto" });
            }

            return Ok(projeto);
        }
    }
}
