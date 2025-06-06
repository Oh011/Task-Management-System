using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.HelperMethods;
using Services.Abstractions;
using Shared.Dtos.Project;
using Shared.ParameterTypes;
using System.Security.Claims;

namespace Presentation
{


    [Authorize]
    [Route("api/projects")]
    public class ProjectController(IServiceManager serviceManager, IAuthorizationService authorizationService) : ApiController
    {


        [HttpPost]

        public async Task<ActionResult<ProjectSummaryDto>> CreateProject(CreateProjectDto dto)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await serviceManager.ProjectService.CreateProjectAsync(dto, userId);


            return CreatedAtAction(nameof(GetProjectById), new { id = result.Id }, result);
        }


        [HttpGet("{id}")]

        public async Task<ActionResult<ProjectResultDto?>> GetProjectById(int id)
        {


            var result = await serviceManager.ProjectService.GetProjectByIdAsync(id, AuthHelpers.GetUserId(HttpContext));


            return Ok(result);
        }

        [HttpPatch("{id}")]

        public async Task<ActionResult<ProjectResultDto>> UpdateProject(int id, ProjectUpdateDto dto)
        {


            var result = await serviceManager.ProjectService.UpdateProjectAsync(id, dto, AuthHelpers.GetUserId(HttpContext));


            return Ok(result);
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ProjectSummaryDto>>> GetUserProjects([FromQuery] ProjectParameters parameters)
        {


            var result = await serviceManager.ProjectService.GetUserProjectsAsync(parameters, AuthHelpers.GetUserId(HttpContext));

            return Ok(result);
        }


        [HttpGet("{projectId}/dashboard")]

        public async Task<ActionResult<ProjectDashboardDto>> ProjectDashboard(int projectId)
        {


            var result = await serviceManager.ProjectDashboardService.GetDashboardDtoAsync(projectId, AuthHelpers.GetUserId(HttpContext));



            return Ok(result);
        }



        [HttpDelete("{id}")]


        public async Task<IActionResult> DeleteProject(int id)
        {




            await serviceManager.ProjectService.DeleteProjectAsync(id, AuthHelpers.GetUserId(HttpContext));


            return NoContent();
        }


        [HttpPatch("{id}/status")]
        public async Task<ActionResult<ProjectResultDto>> ChangeStatus([FromRoute] int id, [FromBody] UpdateProjectStatus dto)
        {


            var result = await serviceManager.ProjectService.UpdateProjectStatus(id, dto, AuthHelpers.GetUserId(HttpContext));


            return Ok(result);
        }
    }
}
