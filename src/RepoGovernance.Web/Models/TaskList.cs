using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class TaskList
    {
        public List<TaskItem> Tasks { get; set; }
        public bool IsContributor { get; set; }

        public TaskList()
        {
            Tasks = new List<TaskItem>();
            IsContributor = false;
        }
    }
}