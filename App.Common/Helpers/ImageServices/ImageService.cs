using App.Common.Const;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Common.Helpers.ImageServices
{
    public class ImageService : ControllerBase, IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png", ".pdf", ".svg" };
        private int _maxAllowedSize = 5242880; // 5 MegaByte

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public Task<(bool ok, string? error)> ValidateAsync(IFormFile image)
        {
            var extension = Path.GetExtension(image.FileName);
            if (!_allowedExtensions.Contains(extension))
                return Task.FromResult((false, Errors.NotAllowedExtension));

            if (image.Length > _maxAllowedSize)
                return Task.FromResult((false, Errors.MaxSize));

            return Task.FromResult<(bool ok, string? error)>((true, null));
        }
        public async Task<(bool isUploaded, string? errorMessage)> UploadAsync(IFormFile image, string imageName, string folderPath) //, bool hasThumbnail
        {
            var extension = Path.GetExtension(image.FileName);

            if (!_allowedExtensions.Contains(extension))
                return (isUploaded: false, errorMessage: Errors.NotAllowedExtension);

            if (image.Length > _maxAllowedSize)
                return (isUploaded: false, errorMessage: Errors.MaxSize);

            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}{folderPath}", imageName);

            using var stream = System.IO.File.Create(path);
            await image.CopyToAsync(stream);
            stream.Dispose();


            return (isUploaded: true, errorMessage: null);
        }

        public async Task<(bool isUploaded, string? errorMessage)> UploadFileAPIAsync(IFormFile image, string imageName, string folderName, string rootPath) //, bool hasThumbnail
        {
            var extension = Path.GetExtension(image.FileName);

            if (!_allowedExtensions.Contains(extension))
                return (isUploaded: false, errorMessage: Errors.NotAllowedExtension);

            if (image.Length > _maxAllowedSize)
                return (isUploaded: false, errorMessage: Errors.MaxSize);

            rootPath += @"wwwroot\";
            var path = Path.Combine($"{rootPath}{folderName}", imageName);

            using var stream = System.IO.File.Create(path);
            await image.CopyToAsync(stream);
            stream.Dispose();

            return (isUploaded: true, errorMessage: null);
        }

        public void Delete(string imagePath, string? imageThumbnailPath = null)
        {
            var oldImagePath = $"{_webHostEnvironment.WebRootPath}{imagePath}";

            if (System.IO.File.Exists(oldImagePath))
                System.IO.File.Delete(oldImagePath);

            if (!string.IsNullOrEmpty(imageThumbnailPath))
            {
                var oldThumbPath = $"{_webHostEnvironment.WebRootPath}{imageThumbnailPath}";

                if (System.IO.File.Exists(oldThumbPath))
                    System.IO.File.Delete(oldThumbPath);
            }
        }


        public async Task<IActionResult> DownloadAsync(string pdfName)
        {
            var file = _webHostEnvironment.WebRootPath + pdfName;

            string contentType = "application/octet-stream";
            byte[] fileBytes;
            if (System.IO.File.Exists(file))
            {
                fileBytes = System.IO.File.ReadAllBytes(file);
                return File(fileBytes, contentType, pdfName);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
