using Services.Abstractions.Common;

namespace Services.Abstractions.Factories
{
    internal interface IUploadServiceFactory
    {

        IUploadService GetUploadService(UploadType type);
    }
}
