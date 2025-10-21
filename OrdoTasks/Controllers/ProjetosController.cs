using Microsoft.AspNetCore.Mvc;
using OrdoTasksApplication.Interfaces;

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
    }
}
