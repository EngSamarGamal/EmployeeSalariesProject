using App.Common.Helpers;
using App.Domain.Models.Identity;
using Azure.Core;
using Google.Api.Gax.ResourceNames;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;

namespace FileManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileActionsController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        //private readonly UserManager<ApplicationUser> _userManager;
        public FileActionsController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            //_userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> TestBaseUrl()
        {
            string schema = "https";
            var request = _httpContextAccessor.HttpContext!.Request;
            if (!request.IsHttps)
            {
                schema = "http";
            }

            //if (request.Host.Host.Contains("www") || request.Host.Host.Contains("https") || request.Host.Host.Contains(".com"))
            //{
            //    schema = "https";
            //}

            var host = request.Host.Host;
            var port = request.Host.Port; // This can be null if the port is the standard port for the scheme

            // Construct the base URL to include the port if it's available and not a standard port
            string baseUrlTestPort;
            if (port.HasValue && !((schema == "http" && port == 80) || (schema == "https" && port == 443)))
            {
                baseUrlTestPort = $"{schema}://{host}:{port}";
            }
            else
            {
                baseUrlTestPort = $"{schema}://{host}";
            }

            var baseUrl = $"{schema}://{request.Host}";
            string baseUrlWithPort = $"{schema}://{request.Host}";

            if ((request.IsHttps && request.Host.Port != 443) || (!request.IsHttps && request.Host.Port != 80))
            {
                baseUrlWithPort += $":{request.Host.Port}";
            }
            return Ok(new { BaserUrlWithDefault = baseUrl, BaseUrlwithPort = baseUrlWithPort, BaseUrlTestPort = baseUrlTestPort });
        }


        [HttpPost("UploadFile")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = Int32.MaxValue, ValueLengthLimit = Int32.MaxValue)]

        public async Task<IActionResult> UploadFile(IFormFile? file, string tagName, string userId)
        {
            try
            {
               
                var configurationBuilder = new ConfigurationBuilder();
                var appSettingPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                configurationBuilder.AddJsonFile(appSettingPath, false);
                var root = configurationBuilder.Build();
                string baseUrl = root.GetSection("BaseUrl").Value;
              
                string path = "Uploads/" + tagName + "/";
                string PhysicalfilePath = Path.Combine(_hostingEnvironment.WebRootPath, path);
                if (!Directory.Exists(PhysicalfilePath))
                {
                    Directory.CreateDirectory(PhysicalfilePath);
                }

                string fileName = Guid.NewGuid() + "." + file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                using (var stream = new FileStream(PhysicalfilePath + fileName, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                FileModel fileModel = new FileModel();
                fileModel.Url = baseUrl + "/" + path + fileName;
                fileModel.FileName = file.FileName;
                return Ok(fileModel);
            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }
        [HttpPost("UploadMultipleFile")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = Int32.MaxValue, ValueLengthLimit = Int32.MaxValue)]
        public async Task<IActionResult> UploadMultipleFile(List<IFormFile> lstFiles, string tagName, string userId)
        {
            try
            {
                List<FileModel> fileModelList = new List<FileModel>();
                //var user = await _userManager.FindByIdAsync(userId);
                //if (user == null)
                //{
                //    return BadRequest("");
                //}

                var configurationBuilder = new ConfigurationBuilder();
                var appSettingPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                configurationBuilder.AddJsonFile(appSettingPath, false);
                var root = configurationBuilder.Build();
                string baseUrl = root.GetSection("BaseUrl").Value;
               
                string path = "Uploads/" + tagName + "/";
                string PhysicalfilePath = Path.Combine(_hostingEnvironment.WebRootPath, path);
                if (!Directory.Exists(PhysicalfilePath))
                {
                    Directory.CreateDirectory(PhysicalfilePath);
                }
                foreach (IFormFile currentFile in lstFiles)
                {
                    FileModel fileModel = new FileModel();
                    string fileName = Guid.NewGuid() + "." + currentFile.FileName.Substring(currentFile.FileName.LastIndexOf(".") + 1);
                    using (var stream = new FileStream(PhysicalfilePath + fileName, FileMode.Create))
                    {
                        await currentFile.CopyToAsync(stream);
                    }

                    //imageModelList.Add(baseUrl + "/" + path + fileName);

                    fileModel.Url = baseUrl + "/" + path + fileName;
                    fileModel.FileName = currentFile.FileName;
                    fileModelList.Add(fileModel);
                }
                return Ok(fileModelList);
            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }
        [HttpPost("DeleteFile")]
        public async Task<IActionResult> DeleteFile(string fileUrl)
        {
            try
            {
                // Decode the URL to handle special characters
                string decodedFileUrl = WebUtility.UrlDecode(fileUrl);

                // Ensure that the URL is valid
                if (!Uri.TryCreate(decodedFileUrl, UriKind.Absolute, out var fileUri))
                {
                    return BadRequest("Invalid file URL");
                }

                // Extract the local path from the URL
                string localPath = fileUri.LocalPath;

                // Combine with the web root path to get the full file path
                string webRootPath = _hostingEnvironment.WebRootPath;
                string fullPath = Path.Combine(webRootPath, localPath.TrimStart('/'));

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    return Ok($"File {fullPath} deleted successfully.");
                }
                else
                {
                    return BadRequest($"File not found at {fullPath}");
                }
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                return BadRequest($"Error deleting file: {ex.Message}");
            }
           
        }
        [HttpPost("DeleteMultipleFiles")]
        public async Task<IActionResult> DeleteMultipleFiles(List<string> filesUrl)
        {
            try
            {
                foreach (string fileUrl in filesUrl)
                {
                    string decodedFileUrl = WebUtility.UrlDecode(fileUrl);

                    // Ensure that the URL is valid
                    if (!Uri.TryCreate(decodedFileUrl, UriKind.Absolute, out var fileUri))
                    {
                        return BadRequest("Invalid file URL");
                    }

                    // Extract the local path from the URL
                    string localPath = fileUri.LocalPath;

                    // Combine with the web root path to get the full file path
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string fullPath = Path.Combine(webRootPath, localPath.TrimStart('/'));

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);

                    }
                }
                return Ok($"Files {filesUrl} deleted successfully.");
                // Decode the URL to handle special characters

            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                return BadRequest($"Error deleting file: {ex.Message}");
            }
           
        }
    }
}