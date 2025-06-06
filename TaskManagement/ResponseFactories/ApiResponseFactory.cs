using Microsoft.AspNetCore.Mvc;
using Shared.Errors;
using System.Net;

namespace TaskManagement.ResponseFactories
{
    public class ApiResponseFactory
    {


        public static IActionResult CustomValidationResponse(ActionContext context)
        {


            var errors = context.ModelState.Where(Pair => Pair.Value.Errors.Any()).Select(



                pair => new ValidationError
                {

                    Field = NormalizeFieldName(pair.Key),
                    Errors = pair.Value.Errors.Select(e => e.ErrorMessage)
                }

                );


            // KeyValuePair<string, ModelStateEntry>
            //So, error is a key - value pair where:
            //error.Key is the name of the field(e.g., "Email", "Password")
            //error.Value is the ModelStateEntry for that field, which contains the validation errors.


            var Response = new ValidationErrorResponse
            {

                StatusCode = (int)HttpStatusCode.BadRequest,
                Errors = errors,
                Message = "Validation Fields"

            };

            return new BadRequestObjectResult(Response);
        }

        private static string NormalizeFieldName(string key)
        {
            // Removes "$." prefix (from JSONPath), and "dto." or "model." if present
            return key.Replace("$.", "")
                      .Replace("dto.", "")
                      .Replace("model.", "");
        }
    }
}
