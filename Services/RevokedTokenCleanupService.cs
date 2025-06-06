using Domain.Contracts;
using Domain.Entities.IdentityModels;
using Microsoft.Extensions.Logging;
using Services.Abstractions;

namespace Services
{
    public class RevokedTokenCleanupService : IRevokedTokenCleanupService
    {


        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<RevokedTokenCleanupService> _logger;



        public RevokedTokenCleanupService(IUnitOfWork unitOfWork, ILogger<RevokedTokenCleanupService> logger)
        {

            _unitOfWork = unitOfWork;
            _logger = logger;

        }

        public async Task CleanUpAsync()
        {


            await _unitOfWork.GetRepository<RefreshToken, int>().DeleteRange(

                   t => t.Revoked == true || t.Expiration < DateTime.UtcNow

                   );


            await _unitOfWork.SaveChanges();

            _logger.LogInformation("Deleted {Count} expired revoked tokens.", true);
        }
    }
}
