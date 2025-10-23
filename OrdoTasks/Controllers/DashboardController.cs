using Microsoft.AspNetCore.Mvc;
using OrdoTasksApplication.UseCases.DashboardUseCases;

namespace OrdoTasks.Controllers
{
    public class DashboardController : ControllerBase
    {
        private readonly DashboardUseCase _dashboardUseCase;

        public DashboardController(DashboardUseCase dashboardUseCase)
        {
            _dashboardUseCase = dashboardUseCase;
        }

        [HttpGet("metricas")]
        public async Task<IActionResult> GetDashboardMetrics()
        {
            try
            {
                var result = await _dashboardUseCase.Run();

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro interno no servidor." });
            }

        }
    }
}
