using Domain.Exceptions;
using Shared.Errors;
using System.Net;

namespace TaskManagement.MiddleWares
{
    public class GlobalErrorHandlingMiddleware
    {

        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        private readonly RequestDelegate? _next;

        //delegate that represents the next middleware in the HTTP pipeline


        public GlobalErrorHandlingMiddleware(ILogger<GlobalErrorHandlingMiddleware> logger, RequestDelegate next)
        {

            _logger = logger;
            _next = next;

        }


        public async Task InvokeAsync(HttpContext context)
        {


            try
            {


                await _next(context);

                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {

                    await HandleNotFoundApiError(context);
                }
            }


            catch (Exception e)
            {



                _logger.LogError(e, e.Message);



                await HandleException(context, e);
            }


        }

        private async Task HandleException(HttpContext context, Exception e)
        {


            context.Response.ContentType = "application/json";

            _logger.LogError(e, "An error occurred while processing the request.");


            int statusCode = context.Response.StatusCode;

            var Response = new ErrorDetails
            {

                ErrorMessage =
                (statusCode == (int)HttpStatusCode.InternalServerError ?
                "An unexpected error occurred. Please try again later." :
                e.Message),
            };


            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;



            if (e is ValidationException C)
            {
                await context.Response.WriteAsJsonAsync(this.HandleValidationException(C));

                return;

            }



            context.Response.StatusCode = e switch
            {

                NotFoundException => (int)HttpStatusCode.NotFound,
                ForbiddenException => (int)HttpStatusCode.Forbidden,
                AccountLockedException => (int)HttpStatusCode.Locked,
                UnAuthorizedException => (int)HttpStatusCode.Unauthorized,
                BadRequestException => (int)HttpStatusCode.BadRequest,
                ResourceExists => (int)HttpStatusCode.Conflict,

                _ => (int)HttpStatusCode.InternalServerError,
            };



            Response.StatusCode = context.Response.StatusCode;


            Response.ErrorMessage =
                 (context.Response.StatusCode == (int)HttpStatusCode.InternalServerError ?
                 "An unexpected error occurred. Please try again later." :
                 e.Message);


            await context.Response.WriteAsJsonAsync(Response);



        }



        private ValidationErrorResponse HandleValidationException(ValidationException validationException)
        {


            var ValidationErrors = validationException.Errors.Select(X => new ValidationError
            {

                Field = X.Key,
                Errors = X.Value
            });

            var Response = new ValidationErrorResponse
            {

                StatusCode = (int)HttpStatusCode.BadRequest,

                Errors = ValidationErrors,

                Message = "Validation Fields"

            };

            return Response;

        }

        private async Task HandleNotFoundApiError(HttpContext context)
        {

            context.Response.ContentType = "application/json";

            var Response = new ErrorDetails
            {

                StatusCode = context.Response.StatusCode,

                ErrorMessage = $"The EndPoint {context.Request.Path} is not found"
            };

            //writes a JSON object to the HTTP response body.

            await context.Response.WriteAsJsonAsync(Response);
        }
    }
}
