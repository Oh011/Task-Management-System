using Microsoft.AspNetCore.Mvc;
using Shared.Errors;
using System.Net;

namespace Presentation
{


    [ApiController]
    [Route("api/[controller]")]


    [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ValidationErrorResponse), (int)HttpStatusCode.BadRequest)]
    public class ApiController : ControllerBase
    {
    }
}
