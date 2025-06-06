using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.HelperMethods;
using Services.Abstractions;
using Shared.Dtos;
using Shared.Dtos.Project;
using Shared.ParameterTypes;

namespace Presentation
{

    [Authorize]
    [Route("api/projects/{projectId}/users")]
    public class ProjectUsersController(IServiceManager serviceManager) : ApiController
    {


        [HttpGet("")]

        public async Task<ActionResult<PaginatedResult<ProjectUserDto>>> GetProjectUsers(int projectId, [FromQuery] ProjectUserParameters parameters)
        {

            var result = await serviceManager.UserProjectService.GetProjectUsersAsync(projectId, AuthHelpers.GetUserId(HttpContext), parameters);


            return Ok(result);

        }

        [HttpGet("search-available")]
        public async Task<ActionResult<PaginatedResult<UserInfoDto>>> GetUsersNotInProjectAsync(
        int projectId,
       [FromQuery] UserSearchParameters parameters)
        {
            var result = await serviceManager.UserProjectService.GetUsersNotInProjectAsync(AuthHelpers.GetUserId(HttpContext), projectId, parameters);
            return Ok(result);
        }


        [HttpPatch("assign-role")]
        public async Task<ActionResult<ProjectUserDto>> AssignRole(int projectId, AssignRoleDto dto)
        {

            var result = await serviceManager.UserProjectService.AssignRoleToUserAsync(AuthHelpers.GetUserId(HttpContext), projectId, dto);
            return Ok(result);
        }


        [HttpDelete("{targetUserId}")]
        public async Task<IActionResult> RemoveUserFromProject(int projectId, string targetUserId)
        {

            await serviceManager.UserProjectService.RemoveProjectUserAsync(AuthHelpers.GetUserId(HttpContext), targetUserId, projectId);
            return NoContent();
        }


    }
}
