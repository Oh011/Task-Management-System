using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.HelperMethods;
using Services.Abstractions;
using Shared.Dtos.Tasks;
using Shared.ParameterTypes;

namespace Presentation
{

    [Authorize]

    [Route("api/tasks")]
    public class TaskController(IServiceManager serviceManager) : ApiController
    {




        [HttpPost]

        public async Task<ActionResult<TaskResultDto>> CreateTask(CreateTaskDto task)
        {



            var result = await serviceManager.TaskService.CreateTask(task, AuthHelpers.GetUserId(HttpContext));


            return CreatedAtAction(nameof(GetTaskById), new { id = result.Id }, result);


        }

        [HttpGet("{taskId}")]

        public async Task<ActionResult<TaskDetailsDto?>> GetTaskById([FromRoute] int taskId)
        {


            var result = await serviceManager.TaskService.GetTaskById(taskId, AuthHelpers.GetUserId(HttpContext));
            return Ok(result);
        }



        [HttpPatch("{id}")]
        public async Task<ActionResult<TaskResultDto>> UpdateTask(int id, [FromBody] UpdateTaskDto dto)
        {

            var result = await serviceManager.TaskService.UpdateTask(id, dto, AuthHelpers.GetUserId(HttpContext));

            return Ok(result);

        }

        [HttpPatch("{id}/status")]

        public async Task<ActionResult<TaskResultDto>> ChangeTaskStatus(int id, [FromBody] UpdateTaskStatus status)
        {

            var result = await serviceManager.TaskService.ChangeTaskStatus(id, AuthHelpers.GetUserId(HttpContext), status);


            return Ok(result);

        }


        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteTask(int id)
        {

            await serviceManager.TaskService.DeleteTask(id, AuthHelpers.GetUserId(HttpContext));

            return NoContent();
        }



        [HttpGet]

        public async Task<ActionResult<IEnumerable<TaskResultDto>>> GetUserTasks([FromQuery] TaskParameters parameters)
        {



            var result = await serviceManager.TaskService.GetUserTasksAsync(parameters, AuthHelpers.GetUserId(HttpContext));

            return Ok(result);
        }









    }
}


//🧠 Why is Id in the DTO optional or even better to remove?
//👉 Because in good API design,
//👉 the resource you are updating is identified by the URL, not by the body (DTO).