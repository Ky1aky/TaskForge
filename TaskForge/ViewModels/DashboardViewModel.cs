using TaskForge.ViewModels;

namespace TaskForge.ViewModels
{
    public class DashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;

        public int TotalTasks { get; set; }

        public int PendingTasks { get; set; }

        public int InProgressTasks { get; set; }

        public int CompletedTasks { get; set; }

        public List<TaskListViewModel> RecentTasks { get; set; } = new();
    }
}