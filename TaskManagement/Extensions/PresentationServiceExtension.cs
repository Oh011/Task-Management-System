using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using TaskManagement.ResponseFactories;

namespace TaskManagement.Extensions
{
    public static class PresentationServiceExtension
    {


        public static IServiceCollection AddPresentationServices(this IServiceCollection Services)
        {



            Services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
                .AddJsonOptions(options =>
                {
                    //This tells ASP.NET Core to accept and serialize enum values as strings.

                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

                });





            Services.Configure<ApiBehaviorOptions>(options =>
            {


                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationResponse;
            });



            Services.AddEndpointsApiExplorer();

            Services.AddSwaggerGen();

            return Services;
        }
    }
}
