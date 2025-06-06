using Hangfire;
using System.Reflection;
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


            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
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
