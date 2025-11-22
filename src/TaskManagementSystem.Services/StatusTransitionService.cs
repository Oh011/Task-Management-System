using Domain.Common;
using Domain.Contracts;
using Domain.Entities.ProjectModels;
using Domain.Exceptions;
using Services.Abstractions;
using Shared.Dtos.Tasks;
using TaskStatus = Domain.Entities.TaskModels.TasksStatus;

namespace Services
{
    class StatusTransitionService : IStatusTransitionService
    {

        private readonly ICacheRepository _cacheRepository;
        private const string TaskStatusCacheKey = "TaskStatus";
        private const string ProjectStatusCacheKey = "ProjectStatus";
        private readonly TimeSpan CacheExpiration = TimeSpan.FromHours(1);


        public StatusTransitionService(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public IReadOnlyDictionary<TaskStatus, HashSet<TaskStatus>> GetTaskTransitions()
        {
            return _cacheRepository.GetOrAdd(
                TaskStatusCacheKey,
                () => LoadStatusTransitions.GetTaskTransitions(),
                CacheExpiration);
        }

        public IReadOnlyDictionary<ProjectStatus, HashSet<ProjectStatus>> GetProjectTransitions()
        {
            return _cacheRepository.GetOrAdd(
                ProjectStatusCacheKey,
                () => LoadStatusTransitions.GetProjectTransitions(),
                CacheExpiration);
        }


        public void AddAllowedTaskTransitions(IEnumerable<TaskDetailsDto> tasks)
        {
            var allowedTransitions = GetTaskTransitions();

            foreach (var task in tasks)
            {
                if (Enum.TryParse<TaskStatus>(task.Status, out var currentStatus))
                {
                    var transitions = allowedTransitions[currentStatus];
                    task.AllowedStatus = transitions.Select(t => t.ToString()).ToList();
                }
                else
                {
                    task.AllowedStatus = new List<string>();
                }
            }
        }

        public void CheckAllowedTaskTransitions(TaskStatus oldStatus, TaskStatus newStatus)
        {

            var AllowedTransitions = GetTaskTransitions();


            if (!AllowedTransitions[oldStatus].Contains(newStatus))
            {

                throw new StatusChangeNotAllowedException($"Cannot change status from {oldStatus} to {newStatus}");

            }
        }
    }
}
