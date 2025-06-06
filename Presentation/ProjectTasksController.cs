using Microsoft.AspNetCore.Mvc;
using Presentation.HelperMethods;
using Services.Abstractions;
using Shared.Dtos;
using Shared.Dtos.Tasks;
using Shared.ParameterTypes;

namespace Presentation
{

    [Route("api/projects/{projectId}/tasks")]
    public class ProjectTasksController(IServiceManager serviceManager) : ApiController
    {



        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProjectTaskResultDto>>> GetProjectTasks([FromRoute] int projectId, [FromQuery] TaskParameters parameters)
        {


            var result = await serviceManager.ProjectTasksService.GetProjectTasksAsync(projectId, AuthHelpers.GetUserId(HttpContext), parameters);

            return Ok(result);
        }


        [HttpGet("{taskId}")]


        public async Task<ActionResult<ProjectTaskDetailsDto>> GetProjectTaskById([FromRoute] int taskId, [FromRoute] int projectId)
        {

            var result = await serviceManager.ProjectTasksService.GetProjectTaskByIdAsync(taskId, projectId, AuthHelpers.GetUserId(HttpContext));

            return Ok(result);
        }


        [HttpGet("assigned-to-me")]

        public async Task<ActionResult<PaginatedResult<TaskResultDto>>> GetUserAssignedTasks([FromRoute] int projectId, [FromQuery] TaskParameters parameters)
        {

            var result = await serviceManager.ProjectTasksService.GetAssignedTasksForUserAsync(projectId,
                AuthHelpers.GetUserId(HttpContext), parameters);


            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TaskResultDto>> AddTask([FromRoute] int projectId, [FromBody] CreateProjectTaskDto dto)
        {



            var result = await serviceManager.ProjectTasksService.AddTask(dto, projectId, AuthHelpers.GetUserId(HttpContext));

            return Ok(result);


        }


        [HttpPut("{taskId}")]

        public async Task<ActionResult<TaskResultDto>> UpdateTask([FromRoute] int projectId, [FromRoute] int taskId, [FromBody] UpdateTaskDto dto)
        {



            var result = await serviceManager.ProjectTasksService.UpdateTask(dto, taskId, projectId, AuthHelpers.GetUserId(HttpContext));

            return Ok(result);


        }



        [HttpPatch("{taskId}")]

        public async Task<ActionResult<TaskResultDto>> ChangeTaskStatus(int taskId, [FromRoute] int projectId, [FromBody] UpdateTaskStatus status)
        {

            var result = await serviceManager.ProjectTasksService.ChangeTaskStatus(taskId, AuthHelpers.GetUserId(HttpContext), projectId, status);


            return Ok(result);

        }



        [HttpPatch("assign")]

        public async Task<ActionResult<TaskResultDto>> AssignTask([FromRoute] int projectId, AssignTaskDto dto)
        {

            //put taskid in url

            var result = await serviceManager.ProjectTasksService.AssignTask(projectId, AuthHelpers.GetUserId(HttpContext), dto);

            return Ok(result);
        }



        [HttpDelete("{taskId}")]

        public async Task<ActionResult<TaskResultDto>> DeleteTask([FromRoute] int projectId, [FromRoute] int taskId)
        {



            await serviceManager.ProjectTasksService.DeleteTask(taskId, projectId, AuthHelpers.GetUserId(HttpContext));

            return NoContent();


        }



    }
}
