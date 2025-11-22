using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Abstractions;

namespace Utilities
{
    public class RevokedTokenCleanupWorker : BackgroundService
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RevokedTokenCleanupWorker> _logger;


        public RevokedTokenCleanupWorker(IServiceScopeFactory scopeFactory, ILogger<RevokedTokenCleanupWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation("worker started : RevokedToken");


            while (!stoppingToken.IsCancellationRequested)
            {

                using var scope = _scopeFactory.CreateScope();

                var cleanUpService = scope.ServiceProvider.GetRequiredService<IRevokedTokenCleanupService>();


                //Because this background service itself is Singleton, and you cannot inject scoped
                //services directly into singletons.

                try
                {

                    await cleanUpService.CleanUpAsync();
                }

                catch (Exception ex)
                {

                    _logger.LogError(ex, "Error while cleaning up revoked tokens.");


                }

                await Task.Delay(TimeSpan.FromMinutes(17), stoppingToken);

            }

            _logger.LogInformation("RevokedTokenCleanupWorker stopped.");
        }
    }
}
