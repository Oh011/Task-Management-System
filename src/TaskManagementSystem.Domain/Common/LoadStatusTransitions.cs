using Domain.Entities.ProjectModels;
using TaskStatus = Domain.Entities.TaskModels.TasksStatus;

namespace Domain.Common
{
    public class LoadStatusTransitions
    {


        public static readonly HashSet<TaskStatus> AllowedCreationStatuses = new()
    {
        TaskStatus.ToDo,
        TaskStatus.InProgress
    };

        private static readonly IReadOnlyDictionary<ProjectStatus, HashSet<ProjectStatus>> _projectTransitions = new Dictionary<ProjectStatus, HashSet<ProjectStatus>>
    {
        { ProjectStatus.NotStarted, new HashSet<ProjectStatus> { ProjectStatus.InProgress } },
        { ProjectStatus.InProgress, new  HashSet<ProjectStatus> { ProjectStatus.Completed, ProjectStatus.OnHold } },
        { ProjectStatus.OnHold, new HashSet<ProjectStatus> { ProjectStatus.InProgress } },
        { ProjectStatus.Completed, new HashSet<ProjectStatus>() }
    };

        private static readonly IReadOnlyDictionary<TaskStatus, HashSet<TaskStatus>> _taskTransitions = new Dictionary<TaskStatus, HashSet<TaskStatus>>
    {
        { TaskStatus.ToDo, new HashSet<TaskStatus> { TaskStatus.InProgress } },
        { TaskStatus.InProgress, new HashSet<TaskStatus> { TaskStatus.Done, TaskStatus.Blocked } },
        { TaskStatus.Blocked, new HashSet<TaskStatus> { TaskStatus.InProgress } },
        { TaskStatus.Done, new HashSet<TaskStatus>() }
    };

        /// <summary>
        /// Gets allowed project status transitions.
        /// </summary>
        public static IReadOnlyDictionary<ProjectStatus, HashSet<ProjectStatus>> GetProjectTransitions() => _projectTransitions;

        /// <summary>
        /// Gets allowed task status transitions.
        /// </summary>
        public static IReadOnlyDictionary<TaskStatus, HashSet<TaskStatus>> GetTaskTransitions() => _taskTransitions;


    }
}


