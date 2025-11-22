using Microsoft.AspNetCore.Http;

namespace Services.Abstractions
{
    public interface IUploadService
    {


        Task<string> UploadFileAsync(IFormFile file);
        void DeleteFile(string filePath);
    }
}
