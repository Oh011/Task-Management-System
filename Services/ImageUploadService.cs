using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using Shared.Options;

namespace Services
{
    internal class ImageUploadService : IImageUploadService
    {



        private readonly ImageOptions _options;
        private readonly List<string> AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
        private int _MaxSize = 2_097_152;



        public ImageUploadService(IOptions<ImageOptions> options)
        {

            _options = options.Value;
        }

        public void DeleteFile(string filePath)
        {

            var existingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.Replace("/", "\\"));
            if (File.Exists(existingFilePath))
            {
                File.Delete(existingFilePath);
            }


        }

        public async Task<string?> UploadFileAsync(IFormFile file)
        {



            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!IsImage(extension))
            {

                throw new BadRequestException("Only image files are allowed.");
            }


            if (file.Length > _MaxSize)
                throw new BadRequestException("Image is too large.");



            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _options.Folder);



            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }


            var FileName = string.Empty;


            FileName = $"{Guid.NewGuid()}{extension}";


            var FilePath = Path.Combine(folderPath, FileName);


            using var FileStream = new FileStream(FilePath, FileMode.Create);

            await file.CopyToAsync(FileStream);


            return Path.Combine(_options.Folder, FileName).Replace("\\", "/");


        }


        private bool IsImage(string extension)
        {



            return AllowedExtensions.Contains(extension);
        }
    }
}
