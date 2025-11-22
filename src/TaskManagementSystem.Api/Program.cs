using Hangfire;
using Microsoft.OpenApi.Models;
using TaskManagement.Extensions;

namespace TaskManagement
{
    public class Program
    {


        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddCoresServices(builder.Configuration);


            builder.Services.AddPresentationServices();

            var controllersXmlFile = Path.Combine(AppContext.BaseDirectory, "Presentation.xml");

            builder.Services.AddSwaggerGen(c =>
            {
                // Include XML comments from controllers project
                c.IncludeXmlComments(controllersXmlFile);

                // JWT setup
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token in the format **Bearer YOUR_TOKEN**"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });



            var app = builder.Build();




            app.UseCustomMiddleware();

            await app.SeedDbAsync();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();



            }


            app.UseStaticFiles();

            app.UseHangfireDashboard();

            app.UseHttpsRedirection();
            app.UseRouting();


            app.UseCors("AllowAll");

            app.UseRateLimiter();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
