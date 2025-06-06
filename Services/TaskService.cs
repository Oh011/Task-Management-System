using AutoMapper;
using Domain.Contracts;
using Domain.Entities.TaskModels;
using Domain.Exceptions;
using Services.Abstractions;
using Services.Specifications.Tasks;
using Shared.Dtos;
using Shared.Dtos.Tasks;
using Shared.ParameterTypes;
using System.Linq.Expressions;



namespace Services
{
    public class TaskService : ITaskService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStatusTransitionService _statusTransitionService;
        private readonly ITaskAuthorizationService _taskAuthorizationService;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper, IStatusTransitionService statusTransitionService, ITaskAuthorizationService taskAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _taskAuthorizationService = taskAuthorizationService;

            _statusTransitionService = statusTransitionService;

        }
        public async Task<TaskResultDto> CreateTask(CreateTaskDto task, string? userId, TaskType taskType = TaskType.Personal)
        {



            var Result = _mapper.Map<TaskItem>(task);



            Result.CreatedByUserId = userId;
            Result.TaskType = taskType;

            if (taskType == TaskType.Project)
            {
                Result.AssignedToUserId = null;
            }
            else if (taskType == TaskType.Personal)
            {
                Result.AssignedToUserId = userId;
            }



            await _unitOfWork.GetRepository<TaskItem, int>().AddAsync(Result);

            await _unitOfWork.SaveChanges();


            var CreatedTask = _mapper.Map<TaskResultDto>(Result); ;

            var allowedTransitions = _statusTransitionService.GetTaskTransitions()[Result.Status].Select(t => t.ToString());



            return CreatedTask;


        }



        public async Task<TaskDetailsDto?> GetTaskById(int taskId, string userId, bool bypassAuthorizationChecks = false)
        {


            var task = await GetTaskOrThrow(taskId);


            if (!bypassAuthorizationChecks)
            {
                _taskAuthorizationService.CheckViewPermission(task, userId);

            }


            var allowedTransitions = _statusTransitionService.GetTaskTransitions()[task.Status].Select(t => t.ToString());
            var result = _mapper.Map<TaskDetailsDto>(task);
            result.AllowedStatus = allowedTransitions;

            return result;
        }

        public async Task<ProjectTaskDetailsDto> GetProjectTaskByIdAsync(int taskId, int projectId, string userId)
        {


            var repo = _unitOfWork.GetRepository<TaskItem, int>();

            var task = await repo.GetWithIdProjectionSpecifications
                (new ProjectTaskDetailsSpecifications(taskId));


            var allowedTransitions = _statusTransitionService.GetTaskTransitions()[
               Enum.Parse<TasksStatus>(task.Status)].Select(t => t.ToString());

            task.AllowedStatus = allowedTransitions;


            return task;


        }




        private async Task<TaskItem> GetTaskOrThrow(int id)
        {
            var task = await _unitOfWork.GetRepository<TaskItem, int>().GetByIdAsync(id);
            return task ?? throw new TaskNotFoundException(id);

        }

        public async Task<TaskResultDto?> UpdateTask(int id, UpdateTaskDto task, string? userId, bool bypassAuthorizationChecks = false)
        {


            var repo = _unitOfWork.GetRepository<TaskItem, int>();
            var result = await GetTaskOrThrow(id);



            if (!bypassAuthorizationChecks)
            {

                _taskAuthorizationService.CheckModifyOrDeletePermission(result, userId);
            }

            if (task.Title != null)
                result.Title = task.Title;

            if (task.Description != null)
                result.Description = task.Description;

            if (task.Priority.HasValue)
                result.Priority = task.Priority.Value;

            if (task.DueDate.HasValue)
                result.DueDate = task.DueDate.Value;


            repo.Update(result);

            await _unitOfWork.SaveChanges();

            return _mapper.Map<TaskResultDto>(result);

        }

        public async Task DeleteTask(int id, string? userId, bool bypassAuthorizationChecks = false)
        {



            var repo = _unitOfWork.GetRepository<TaskItem, int>();
            var result = await GetTaskOrThrow(id);



            if (!bypassAuthorizationChecks)
            {

                _taskAuthorizationService.CheckModifyOrDeletePermission(result, userId);
            }

            repo.Delete(result);


            await _unitOfWork.SaveChanges();
        }

        public async Task<TaskResultDto?> ChangeTaskStatus(int taskId, string? userId, UpdateTaskStatus Dto, bool bypassAuthorizationChecks = false)
        {

            var repo = _unitOfWork.GetRepository<TaskItem, int>();
            var result = await GetTaskOrThrow(taskId);



            if (!bypassAuthorizationChecks)
            {

                _taskAuthorizationService.CheckModifyOrDeletePermission(result, userId);
            }



            _statusTransitionService.CheckAllowedTaskTransitions(result.Status, Dto.Status);


            result.Status = Dto.Status;


            repo.Update(result);

            await _unitOfWork.SaveChanges();

            return _mapper.Map<TaskResultDto?>(result);

        }


        public async Task<PaginatedResult<TaskResultDto>> GetUserTasksAsync(TaskParameters parameters, string userId)
        {


            var specifications = new UserTasksSpecifications(userId, parameters);
            var result = await GetTasksAsync<TaskResultDto>(specifications, userId, parameters);

            return result;


        }



        public async Task<PaginatedResult<ProjectTaskResultDto>> GetProjectTasksAsync(int projectId, string? userId, TaskParameters parameters)
        {

            var specifications = new ProjectTasksSpecifications(projectId, parameters);
            var Tasks = await GetTasksAsync<ProjectTaskResultDto>(specifications, userId, parameters);

            return Tasks;

        }


        private async Task<PaginatedResult<Dto>> GetTasksAsync<Dto>(ProjectionSpecifications<TaskItem, Dto> specifications, string? userId, TaskParameters parameters)
        {


            var repo = _unitOfWork.GetRepository<TaskItem, int>();


            var Tasks = await repo.GetAllWithProjectionSpecifications<Dto>(

               specifications);



            var TotalCount = await repo.CountAsync(specifications);



            var result = _mapper.Map<IEnumerable<Dto>>(Tasks);




            return new PaginatedResult<Dto>(

              parameters.PageIndex,
              parameters.PageSize,
              TotalCount,
              result
              );


        }



        public async Task<PaginatedResult<TaskResultDto>> GetAssignedProjectTasksAsync(int projectId, string userId, TaskParameters parameters)
        {
            var specification = new AssignedProjectTasksSpecification(projectId, userId, parameters);
            return await GetTasksAsync<TaskResultDto>(specification, userId, parameters);
        }



        public async Task<bool> IsTaskAssignedToAsync(int taskId, string? userId)
        {

            var repository = _unitOfWork.GetRepository<TaskItem, int>();

            var taskExistsAndAssigned = await repository.ExistsAsync(
       t => t.Id == taskId && t.AssignedToUserId == userId);

            if (!taskExistsAndAssigned)
            {

                var taskExists = await repository.ExistsAsync(
                    t => t.Id == taskId);

                if (!taskExists)
                    throw new TaskNotFoundException(taskId);


            }

            return taskExistsAndAssigned;
        }


        public async Task<Dictionary<string, int>> CountTasksByStatusAsync(Expression<Func<TaskItem, bool>>? filter)
        {
            var tasks = await _unitOfWork.GetRepository<TaskItem, int>()
                .GetAllWithGrouping(new TaskCountSpecifications(filter));

            var allTaskStatus = Enum.GetValues(typeof(TasksStatus)).Cast<TasksStatus>();

            var tasksGroupedDict = tasks.ToDictionary(t => t.Status, t => t.count);

            return allTaskStatus.ToDictionary(
                status => status.ToString(),
                status => tasksGroupedDict.TryGetValue(status, out var count) ? count : 0
            );
        }



        public async Task<TaskResultDto> AssignTaskAsync(AssignTaskDto dto)
        {

            var task = await GetTaskOrThrow(dto.TaskId);


            task.AssignedToUserId = dto.TragetUserId;

            await _unitOfWork.SaveChanges();

            return _mapper.Map<TaskResultDto>(task);
        }
    }
}

//| Action | Who Can Perform It                   |
//| ------------- | ------------------------------------ |
//| Update/Delete | `Owner` or `Admin` of the project    |
//| Change Status | `Assigned User` **OR** `Owner/Admin` |
