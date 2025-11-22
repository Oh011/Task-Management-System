using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.HelperMethods;
using Services.Abstractions;
using Shared.Dtos;
using Shared.Dtos.Invitations;
using Shared.ParameterTypes;

namespace Presentation
{

    /// <summary>
    /// Controller for managing products.
    /// </summary>

    [Authorize]

    [Route("api/invitations")]
    public class InvitationController(IServiceManager serviceManager) : ApiController
    {


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProjectInvitationsDto>>> GetUserInvitations()
        {


            var result = await serviceManager.ProjectInvitationService.GetUserProjectInvitations(
                AuthHelpers.GetUserId(HttpContext)
                );


            return Ok(result);
        }


        [HttpGet("projects/{projectId}")]

        public async Task<ActionResult<PaginatedResult<ProjectInvitationsDto>>> GetProjectInvitations([FromRoute] int projectId, [FromQuery] ProjectInvitationParameters parameters)
        {


            var result = await serviceManager.ProjectInvitationService.GetProjectInvitations(projectId,
                AuthHelpers.GetUserId(HttpContext), parameters);



            return Ok(result);

        }




        [HttpGet("{id}")]
        public async Task<ActionResult<UserProjectInvitationsDto>> GetInvitationById(int id)
        {
            var invitation = await serviceManager.ProjectInvitationService.GetInvitationsById(id);

            return Ok(invitation);
        }

        [HttpPost]

        public async Task<IActionResult> SendInvitation(SendInvitationDto dto)
        {

            await serviceManager.ProjectInvitationService.SendInvitationAsync(dto, AuthHelpers.GetUserId(HttpContext));

            return Ok(new { Message = "Invitation sent successfully." });
        }


        [HttpPatch("{id}/accept")]

        public async Task<IActionResult> AcceptInvitation(int id)
        {

            await serviceManager.ProjectInvitationService.AcceptInvitation(id, AuthHelpers.GetUserId(HttpContext));
            return Ok(new { Message = "joined the project successfully." });
        }


        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> RejectInvitation(int id)
        {

            await serviceManager.ProjectInvitationService.RejectInvitationInvitation(id, AuthHelpers.GetUserId(HttpContext));
            return Ok(new { Message = "joined the project successfully." });

        }







    }
}
