using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using OrdoTasksApplication.DTOs;
using OrdoTasksApplication.Exceptions.Projects;
using OrdoTasksApplication.Exceptions.Tasks;
using OrdoTasksApplication.Interfaces;
using OrdoTasksApplication.UseCases.Project_UseCases;
using OrdoTasksApplication.UseCases.TasksUseCases;
using OrdoTasksDomain.Entities;

namespace OrdoTasks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetosController : ControllerBase
    {
        private readonly IOrdoTasksProjectRepository _projectRepository;
        private readonly GetAllProjectsUseCase _getAllProjectsUseCase;
        private readonly GetProjetByIdUseCase _getProjetByIdUseCase;
        private readonly CreateProjectUseCase _createProjectUseCase;
        private readonly UpdateProjectUseCase _updateProjectUseCase;
        private readonly DeleteProjectUseCase _deleteProjectUseCase;

        public ProjetosController(IOrdoTasksProjectRepository projectRepository,
            GetAllProjectsUseCase getAllProjectsUseCase,
            GetProjetByIdUseCase getProjetByIdUseCase,
            CreateProjectUseCase createProjectUseCase,
            UpdateProjectUseCase updateProjectUseCase,
            DeleteProjectUseCase deleteProjectUseCase
        )
        {
            _projectRepository = projectRepository;
            _getAllProjectsUseCase = getAllProjectsUseCase;
            _getProjetByIdUseCase = getProjetByIdUseCase;
            _createProjectUseCase = createProjectUseCase;
            _updateProjectUseCase = updateProjectUseCase;
            _deleteProjectUseCase = deleteProjectUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projetos = await _getAllProjectsUseCase.Run();

            return Ok(projetos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            try
            {
                var projeto = await _getProjetByIdUseCase.Run(id);

                return Ok(projeto);
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

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDTO projeto)
        {
            try
            {
                var result = await _createProjectUseCase.Run(projeto);
                return CreatedAtAction(nameof(GetProjectById), new { id = result.Id }, result.Projeto);
            }
            catch (InvalidProjectDataException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro interno no servidor." });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectDTO projeto)
        {
            try
            {
                await _updateProjectUseCase.Run(id, projeto);

                return NoContent();
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                await _deleteProjectUseCase.Run(id);

                return NoContent();
            }
            catch (ProjectNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedDeleteProjectException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro interno no servidor." });
            }
        }
    }
}
