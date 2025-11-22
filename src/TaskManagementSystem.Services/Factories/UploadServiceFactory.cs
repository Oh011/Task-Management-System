using Services.Abstractions;
using Services.Abstractions.Common;

namespace Services.Factories
{
    internal class UploadServiceFactory(IServiceManager _serviceManager)
    {


        public IUploadService GetUploadService(UploadType type)
        {
            return type switch
            {
                UploadType.Image => _serviceManager.ImageUploadService,

                _ => throw new ArgumentException("Invalid upload type", nameof(type)),
            };


        }
    }
}
