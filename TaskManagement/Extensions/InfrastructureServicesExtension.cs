using Domain.Contracts;
using Domain.Entities.IdentityModels;
using Domain.Exceptions;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Persistence.Data.DataSeeding;
using Persistence.Repositories;
using Shared.Options;
using System.Text;
using System.Threading.RateLimiting;
using Utilities;

namespace TaskManagement.Extensions
{
    public static class InfrastructureServicesExtension
    {



        public static IServiceCollection AddInfrastructureServices(this IServiceCollection Services, IConfiguration configuration)
        {

            Services.AddScoped<IDbInitializer, DbInitializer>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();

            Services.AddScoped<IUserRepository, UserRepository>();

            Services.AddScoped<ILinkBuilder, LinkBuilder>();




            Services.AddMemoryCache();
            Services.AddScoped<ICacheRepository, CacheRepository>();




            Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;

                options.Tokens.EmailConfirmationTokenProvider = "Default";



                // Lockout settings (optional for security)
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;

                // Sign-in settings




            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            Services.AddDbContext<ApplicationDbContext>(Options =>
            {


                Options.UseSqlServer(configuration.GetSection("ConnectionStrings")["DefaultConnectionString"]);
            });



            Services.ConfigureJwtOptions(configuration);

            Services.ConfigureRateLimiter();


            Services.ConfigureHangFire(configuration);






            return Services;
        }



        public static IServiceCollection ConfigureHangFire(this IServiceCollection Services, IConfiguration configuration)
        {


            Services.AddHangfire(config =>
        config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(
              configuration.GetSection("ConnectionStrings")["DefaultConnectionString"],
              new SqlServerStorageOptions
              {
                  CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                  SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                  QueuePollInterval = TimeSpan.Zero,
                  UseRecommendedIsolationLevel = true,
                  DisableGlobalLocks = true
              }));

            Services.AddHangfireServer();

            //Starts the Hangfire Background Job Server.

            //This is what executes the jobs(like sending emails


            return Services;
        }




        public static IServiceCollection ConfigureRateLimiter(this IServiceCollection Services)
        {



            Services.AddRateLimiter(options =>
            {
                // Global Rate Limiter that will apply to every request.
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5, // Allow 5 requests
                            Window = TimeSpan.FromSeconds(10), // per 10 seconds
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            //	If requests exceed limit, they can wait in a queue, processed in FIFO order.
                            QueueLimit = 2 // Optional: queue up to 2 extra requests
                        }));

                options.RejectionStatusCode = 429; // Optional, default is 503


                options.AddPolicy("LoginLimiter", context =>
                RateLimitPartition.GetFixedWindowLimiter(
               partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
               factory: _ => new FixedWindowRateLimiterOptions
               {
                   PermitLimit = 5, // Allow 5 login attempts
                   Window = TimeSpan.FromSeconds(10), // Every 10 seconds
                                                      //QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                                      //QueueLimit = 1 // Optional: queue 2 extra
               }));


            });

            return Services;

        }

        public static IServiceCollection ConfigureJwtOptions(this IServiceCollection Services, IConfiguration configuration)
        {


            var JwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();



            Services.AddAuthentication(options =>
            {
                //{How to check if a user is logged in}
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                //{How to respond to unauthenticated requests}
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {


                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {


                        context.HandleResponse(); // Prevent default response

                        // Throw a custom exception so your global middleware catches it
                        throw new UnAuthorizedException("Your session has expired or the token is invalid. Please log in again.");



                    },



                };

                //TokenValidationParameters: These are the parameters that control how the token is validated.


                options.TokenValidationParameters = new TokenValidationParameters
                {


                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = JwtOptions.Issuer,
                    ValidAudience = JwtOptions.Audiance,


                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey)),
                    ClockSkew = TimeSpan.Zero // No extra time allowed for expiry skew
                };


            });


            Services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = configuration.GetValue<string>("Google:client_id");
                options.ClientSecret = configuration.GetValue<string>("Google:client_secret");
                options.CallbackPath = new PathString("/google-response");  // Make sure this matches the route in your controller
            });









            Services.AddAuthorization();



            return Services;

        }
    }
}


//✅ options.Tokens.ProviderMap.Add("Default", new TokenProviderDescriptor(typeof(DataProtectorTokenProvider<ApplicationUser>)))
//This line manually registers a token provider called "Default" that uses the built-in
//DataProtectorTokenProvider<TUser>.

//🔹 What does it do?
//It tells ASP.NET Identity:

//"If you need to generate a token (like for email confirmation), and no specific provider is mentioned,
//use this DataProtectorTokenProvider<ApplicationUser> as the default."

//This is helpful if you're seeing errors like:
//"No IUserTwoFactorTokenProvider<TUser> named 'Default' is registered"
//So, this line is a way to explicitly ensure a default token provider exists.


//✅ .AddDefaultTokenProviders()
//This is a shortcut method that automatically registers all the commonly used token providers, including:

//Email confirmation
//Password reset
//Change email
//Two-factor authentication (2FA)
//Phone number confirmation

//🔹 So why use it?
//To make life easier! When you call .AddDefaultTokenProviders() in AddIdentity, ASP.NET registers 
//all the built-in token providers for you automatically — including the "Default" provider — so 
//you usually don’t need to call ProviderMap.Add(...) manually.