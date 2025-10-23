using OrdoTasksDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.DTOs
{
    public class DashboardMetricsUseCaseDTO
    {
        public int PendingTasks { get; set; }
        public int RunningTasks { get; set; }
        public int FinishedTasks { get; set; }
        public int FinishedOnTimeTasks { get; set; }
        public int CanceledTasks { get; set; }
        public int DelayedTasks { get; set; }
        public double CompletionRate { get; set; }
    }
}
