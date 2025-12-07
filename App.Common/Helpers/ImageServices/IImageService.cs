using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Common.Helpers.ImageServices
{
    public interface IImageService
    {
        Task<(bool ok, string? error)> ValidateAsync(IFormFile image); 

        Task<(bool isUploaded, string? errorMessage)> UploadAsync(IFormFile image, string imageName, string folderPath);
        Task<(bool isUploaded, string? errorMessage)> UploadFileAPIAsync(IFormFile image, string imageName, string folderPath, string rootPath);
        Task<IActionResult> DownloadAsync(string fileName); 
        void Delete(string imagePath, string? imageThumbnailPath = null);
    }
}
