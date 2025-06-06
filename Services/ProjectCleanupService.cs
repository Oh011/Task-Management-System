using Domain.Contracts;
using Domain.Entities.TaskModels;
using Microsoft.Extensions.Logging;
using Services.Abstractions;

namespace Services
{
    public class ProjectCleanupService : IProjectCleanupService
    {


        private readonly ILogger<ProjectCleanupService> _logger;

        private readonly IUnitOfWork _unitOfWork;


        public ProjectCleanupService(ILogger<ProjectCleanupService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

        }
        public async Task CleanupProjectTasksAsync(List<int> taskIds)
        {
            var taskRepo = _unitOfWork.GetRepository<TaskItem, int>();

            var tasks = await taskRepo.FindAllAsync(t => taskIds.Contains(t.Id));

            taskRepo.DeleteRange(tasks);

            await _unitOfWork.SaveChanges();
        }
    }
}
