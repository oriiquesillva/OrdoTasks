using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using OrdoTasksApplication.Interfaces;
using OrdoTasksDomain.Entities;

namespace OrdoTasks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetosController : ControllerBase
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

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Projeto projeto)
        {
            if (string.IsNullOrWhiteSpace(projeto.Nome))
            {
                return BadRequest(new { message = "Ooops! Para criar um projeto e obrigatório informar um nome" });
            }

            projeto.DataCriacao = DateTime.UtcNow;

            var result = await _projetoRepository.CreateAsync(projeto);

            projeto.Id = result;

            return Ok(projeto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Projeto projeto)
        {
            var verificaProjeto = await _projetoRepository.GetByIdAsync(id);

            if (verificaProjeto == null)
            {
                return NotFound(new { message = "Ooops! Não foi possível encontrar o projeto" });
            }

            projeto.Id = id;

            await _projetoRepository.UpdateAsync(projeto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var verificaProjeto = await _projetoRepository.GetByIdAsync(id);

            if (verificaProjeto == null)
            {
                return NotFound(new { message = "Ooops! Não foi possível encontrar o projeto" });
            }

            bool verificaTarefas = await _projetoRepository.HasTarefasAsync(id);

            if (verificaTarefas)
            {
                return BadRequest(new { message = "Não é possível excluir um projeto que possui tarefas vinculadas" });
            }

            await _projetoRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
