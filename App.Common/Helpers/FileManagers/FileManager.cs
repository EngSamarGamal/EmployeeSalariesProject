using App.Common.Enums;
using App.Common.Helpers.ImageServices;
using App.Common.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace App.Common.Helpers.FileManagers
{
	public class FileManager : IFileManager
	{
		private readonly IWebHostEnvironment _env;
		private readonly IConfiguration _config;

		public FileManager(IWebHostEnvironment env, IConfiguration config)
		{
			_env = env;
			_config = config;
		}






	

		
		public async Task<FileModel> SaveToApiWwwrootAsync(IFormFile file, string tagName, string userId, CancellationToken ct = default)
		{
			if (file == null || file.Length == 0) return new FileModel();

			var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
			var isImage = file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)
						  || new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" }.Contains(ext);
			var isVideo = file.ContentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase)
						  || new[] { ".mp4", ".mov", ".avi", ".mkv" }.Contains(ext);

			// validation example: image service or size checks
			if (!isImage && !isVideo) return new FileModel();

			// example: limit video size (optional)
			if (isVideo)
			{
				const long maxVideo = 50L * 1024 * 1024; // 50MB (tweak if needed)
				if (file.Length > maxVideo) return new FileModel();
			}

			// build folder: wwwroot/uploads/{tag}/{yyyy}/{MM}
			tagName = string.IsNullOrWhiteSpace(tagName) ? "general" : tagName.Trim();
			var webroot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
			var folder = Path.Combine(webroot, "uploads", tagName, DateTime.UtcNow.ToString("yyyy"), DateTime.UtcNow.ToString("MM"));

			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

			var fileName = $"{Guid.NewGuid():N}{ext}";
			var filePath = Path.Combine(folder, fileName);

			// write file
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream, ct);
			}

			// relative path for DB / front-end usage
			var relativePath = Path.Combine("/uploads", tagName, DateTime.UtcNow.ToString("yyyy"), DateTime.UtcNow.ToString("MM"), fileName).Replace("\\", "/");

			// optional absolute URL using config BaseUrl (if configured)
			var baseUrl = _config["BaseUrl"]?.TrimEnd('/');
			var url = string.IsNullOrWhiteSpace(baseUrl) ? relativePath : $"{baseUrl}{relativePath}";

			var result = new FileModel
			{
				FileName = fileName,
				RelativePath = relativePath,
				Url = url,
				Size = file.Length,
				IsImage = isImage,
				IsVideo = isVideo,
				UploadedBy = userId ?? "",
				Tag = tagName
			};

			return result;
		}

		public Task<bool> DeleteFileAsync(string relativePath)
		{
			if (string.IsNullOrWhiteSpace(relativePath)) return Task.FromResult(false);

			var path = relativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
			var full = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), path);

			if (File.Exists(full))
			{
				File.Delete(full);
				return Task.FromResult(true);
			}
			return Task.FromResult(false);
		}
	}
}