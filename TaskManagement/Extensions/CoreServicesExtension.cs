using Services;
using Services.Abstractions;
using Services.Abstractions.Factories;
using Services.Factories;
using Services.Mapping;
using Shared.Options;
using Utilities;

namespace TaskManagement.Extensions
{
    public static class CoreServicesExtension
    {



        public static IServiceCollection AddCoresServices(this IServiceCollection Services, IConfiguration configuration)
        {



            Services.AddScoped<IServiceManager, ServiceManager>();

            Services.AddTransient<IEmailService, EmailService>();

            Services.AddScoped<IProjectCleanupService, ProjectCleanupService>();



            Services.AddScoped<IAuthorizationRequirementFactory, AuthorizationRequirementFactory>();


            Services.AddAutoMapper(typeof(TaskProfile).Assembly);


            Services.AddScoped<IRevokedTokenCleanupService, RevokedTokenCleanupService>();
            Services.AddHostedService<RevokedTokenCleanupWorker>();






            Services.Configure<JwtOptions>(
             configuration.GetSection("JwtOptions"));


            Services.Configure<ImageOptions>(


                configuration.GetSection("ImageSettings"));




            Services.Configure<SmtpOptions>(

                configuration.GetSection("SmtpSettings")

                );



            return Services;
        }



    }
}
