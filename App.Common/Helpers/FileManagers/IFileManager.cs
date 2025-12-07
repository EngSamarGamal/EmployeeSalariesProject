using App.Common.Enums;
using App.Common.Response;
using Microsoft.AspNetCore.Http;

namespace App.Common.Helpers.FileManagers
{
    public interface IFileManager
    {
		Task<FileModel> SaveToApiWwwrootAsync(IFormFile file, string tagName, string userId, CancellationToken ct = default);
		Task<bool> DeleteFileAsync(string relativePath);
	}
}
