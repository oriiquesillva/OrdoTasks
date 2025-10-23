using OrdoTasksApplication.DTOs;
using OrdoTasksDomain.Enums;

namespace OrdoTasksApplication.UseCases.DashboardUseCases
{
    public class DashboardUseCase
    {
        private readonly IOrdoTasksTaskRepository _tarefaRepository;

        public DashboardUseCase(IOrdoTasksTaskRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository ?? throw new ArgumentNullException(nameof(tarefaRepository));
        }

        public async Task<DashboardMetricsUseCaseDTO> Run()
        {
            var allTasks = (await _tarefaRepository.GetAllAsync()).ToList();

            if (!allTasks.Any()) 
            {
                return new DashboardMetricsUseCaseDTO();
            }
  
            var now = DateTime.UtcNow;

            var pending = allTasks.Count(t => t.Status == StatusTarefa.Pendente);
            var running = allTasks.Count(t => t.Status == StatusTarefa.EmAndamento);
            var finished = allTasks.Count(t => t.Status == StatusTarefa.Concluida);
            var canceled = allTasks.Count(t => t.Status == StatusTarefa.Cancelada);
            var delayed = allTasks.Count(t => t.DataPrazo < now && t.Status != StatusTarefa.Concluida);
            var finishedOnTime = allTasks.Count(t => t.Status == StatusTarefa.Concluida && t.DataConclusao.HasValue && t.DataConclusao.Value <= t.DataPrazo);

            var completionRate = Math.Round((double)finished / allTasks.Count * 100, 2);

            return new DashboardMetricsUseCaseDTO
            {
                PendingTasks = pending,
                RunningTasks = running,
                FinishedTasks = finished,
                FinishedOnTimeTasks = finishedOnTime,
                CanceledTasks = canceled,
                DelayedTasks = delayed,
                CompletionRate = completionRate
            };
        }
    }
}




//using OrdoTasksApplication.DTOs;
//using OrdoTasksApplication.Exceptions.Tasks;
//using OrdoTasksDomain.Entities;
//using OrdoTasksDomain.Enums;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OrdoTasksApplication.UseCases.DashboardUseCases
//{

//    public class DashboardUseCase
//    {
//        private readonly ITarefaRepository _tarefaRepository;

//        private List<Tarefa> PendingTasks = new List<Tarefa>();
//        private List<Tarefa> RunningTasks = new List<Tarefa>();
//        private List<Tarefa> FinishedTasks = new List<Tarefa>();
//        private List<Tarefa> FinishedOnTimeTasks = new List<Tarefa>();
//        private List<Tarefa> CanceledTasks = new List<Tarefa>();
//        private List<Tarefa> DelayedTasks = new List<Tarefa>();
//        public DashboardUseCase(ITarefaRepository tarefaRepository)
//        {
//            _tarefaRepository = tarefaRepository;
//        }

//        public async Task<DashboardMetricsUseCaseDTO> Run()
//        {
//            var allTasks = await _tarefaRepository.GetAllAsync();

//            foreach (var task in allTasks)
//            {
//                this.SeparetedByStatus(task);
//                this.VerifyDelayedTask(task);
//                this.VerifyFinishedOnTimeTask(task);
//            }

//            var completionRate = FinishedTasks.Count() / allTasks.Count() * 100;

//            return new DashboardMetricsUseCaseDTO
//            {
//                PendingTasks = PendingTasks.Count(),
//                RunningTasks = RunningTasks.Count(),
//                FinishedTasks = FinishedTasks.Count(),
//                FinishedOnTimeTasks = FinishedOnTimeTasks.Count(),
//                CanceledTasks = CanceledTasks.Count(),
//                DelayedTasks = DelayedTasks.Count(),
//                CompletionRate = completionRate
//            };
//        }

//        private void SeparetedByStatus(Tarefa tarefa)
//        {
//            switch (tarefa.Status)
//            {
//                case StatusTarefa.Pendente:
//                    this.PendingTasks.Add(tarefa);
//                    break;
//                case StatusTarefa.EmAndamento:
//                    this.RunningTasks.Add(tarefa);
//                    break;
//                case StatusTarefa.Concluida:
//                    this.FinishedTasks.Add(tarefa);
//                    break;
//                case StatusTarefa.Cancelada:
//                    this.CanceledTasks.Add(tarefa);
//                    break;
//                default:
//                    break;
//            }
//        }

//        private void VerifyDelayedTask(Tarefa tarefa)
//        {
//            if (tarefa.DataPrazo < DateTime.UtcNow)
//            {
//                DelayedTasks.Add(tarefa);
//            }
//        }

//        private void VerifyFinishedOnTimeTask(Tarefa tarefa)
//        {
//            if (tarefa.DataConclusao <= tarefa.DataPrazo)
//            {
//                FinishedOnTimeTasks.Add(tarefa);
//            }
//        }

//    }
//}
