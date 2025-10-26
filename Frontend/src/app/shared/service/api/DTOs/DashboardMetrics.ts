export interface DashboardMetrics {
  pendingTasks: number;
  runningTasks: number;
  finishedTasks: number;
  finishedOnTimeTasks: number;
  canceledTasks: number;
  delayedTasks: number;
  completionRate: number;
}
