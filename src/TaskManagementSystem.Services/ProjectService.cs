using AutoMapper;
using Domain.Contracts;
using Domain.Entities.ProjectModels;
using Domain.Entities.TaskModels;
using Domain.Exceptions;
using Hangfire;
using Services.Abstractions;
using Services.Abstractions.Common;
using Services.Specifications;
using Shared.Dtos;
using Shared.Dtos.Project;
using Shared.ParameterTypes;

namespace Services
{
    public class ProjectService(IUnitOfWork unitOfWork, IMapper mapper, IProjectAuthorizationService projectAuthorizationService, IStatusTransitionService statusTransitionService, IUserProjectService userProjectService
        , IBackgroundJobClient backgroundJobClient, IProjectCleanupService projectCleanupService) : IProjectService
    {



        private async Task<Project> GetOrThrow(int projectId)
        {
            var project = await unitOfWork.GetRepository<Project, int>().GetByIdAsync(projectId);


            return project ?? throw new ProjectNotFoundException(projectId);
        }



        public async Task<ProjectSummaryDto> CreateProjectAsync(CreateProjectDto dto, string userId)
        {



            var result = mapper.Map<Project>(dto);

            result.CreatedByUserId = userId;


            await unitOfWork.GetRepository<Project, int>().AddAsync(result);

            await unitOfWork.SaveChanges();

            await userProjectService.CreateProjectUserAsync(userId, result.Id, ProjectRole.Owner);

            return mapper.Map<ProjectSummaryDto>(result);


        }

        public async Task DeleteProjectAsync(int projectId, string userId)
        {


            var project = await GetOrThrow(projectId);

            var tasks = (await unitOfWork.GetRepository<TaskItem, int>()
                .FindAllAsync(t => t.ProjectTask.ProjectId == projectId))
                .Select(t => t.Id).ToList();



            await projectAuthorizationService.AuthorizeProjectAction(userId, project.Id, ProjectAction.DeleteProject);

            var repo = unitOfWork.GetRepository<Project, int>();


            repo.Delete(project);

            backgroundJobClient.Enqueue<IProjectCleanupService>(s => projectCleanupService.CleanupProjectTasksAsync(tasks));


            await unitOfWork.SaveChanges();
        }



        public async Task<PaginatedResult<ProjectSummaryDto>> GetUserProjectsAsync(ProjectParameters parameters, string userId)
        {




            var repo = unitOfWork.GetRepository<Project, int>();

            var specifications = new UserProjectsSpecifications(userId, parameters);

            var projects = await repo.GetAllProjectedAsync<ProjectSummaryDto>(specifications);




            var TotalCount = (await repo.CountAsync(specifications));





            return new PaginatedResult<ProjectSummaryDto>(

                parameters.PageIndex,
                parameters.PageSize,
                TotalCount,
                projects
                );


        }

        public async Task<ProjectResultDto?> GetProjectByIdAsync(int projectId, string userId)
        {


            var repo = unitOfWork.GetRepository<Project, int>();
            var project = await repo.GetWithIdProjectedAsync<ProjectResultDto>(projectId);

            if (project == null)
                throw new ProjectNotFoundException(projectId);


            var user = await userProjectService.GetProjectUserEntityAsync(projectId, userId);



            if (user == null)
            {

                throw new ForbiddenException($"User {userId} is not a member of project {projectId}.");
            }




            var allowedActions = projectAuthorizationService.GetAllowedActions(user.Role);

            project.UserRole = user.Role.ToString();

            project.AllowedActions = allowedActions;

            var AllowedStatus = statusTransitionService.GetProjectTransitions();


            project.AllowedStatus = AllowedStatus[project.Status].Select(t => t.ToString());

            return project;


        }


        public async Task<ProjectSummaryDto> GetProjectSummaryAsync(int projectId)
        {
            var repo = unitOfWork.GetRepository<Project, int>();

            var project = await repo.GetWithIdProjectedAsync<ProjectSummaryDto>(projectId);

            if (project == null)
                throw new ProjectNotFoundException(projectId);

            return project;
        }


        public async Task<ProjectResultDto?> UpdateProjectAsync(int projectId, ProjectUpdateDto dto, string userId)
        {



            var project = await GetOrThrow(projectId);


            await projectAuthorizationService.AuthorizeProjectAction(userId, project.Id, ProjectAction.UpdateProject);


            mapper.Map(dto, project);


            unitOfWork.GetRepository<Project, int>().Update(project);

            await unitOfWork.SaveChanges();


            return mapper.Map<ProjectResultDto>(project);
        }

        public async Task<ProjectResultDto> UpdateProjectStatus(int projectId, UpdateProjectStatus dto, string userId)
        {


            var project = await GetOrThrow(projectId);


            await projectAuthorizationService.AuthorizeProjectAction(userId, project.Id, ProjectAction.UpdateProject);


            var repo = unitOfWork.GetRepository<Project, int>();

            var AllowedStatus = statusTransitionService.GetProjectTransitions();


            if (!AllowedStatus.TryGetValue(project.Status, out var validNextStatuses) ||
        !validNextStatuses.Contains(dto.ProjectStatus))
            {
                throw new StatusChangeNotAllowedException($"Cannot change status from {project.Status} to {dto.ProjectStatus}");
            };


            project.Status = dto.ProjectStatus;

            repo.Update(project);

            await unitOfWork.SaveChanges();


            var result = mapper.Map<ProjectResultDto>(project);

            result.AllowedStatus = AllowedStatus[dto.ProjectStatus].Select(t => t.ToString());

            return result;

        }

        public async Task<bool> ProjectExistsAsync(int projectId)
        {
            var repo = unitOfWork.GetRepository<Project, int>();
            var projectExists = await repo.ExistsAsync(p => p.Id == projectId);
            return projectExists == true ? projectExists : throw new ProjectNotFoundException(projectId);
        }


    }
}
