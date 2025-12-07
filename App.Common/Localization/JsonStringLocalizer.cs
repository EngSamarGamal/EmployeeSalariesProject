using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;



namespace App.Common.Localization
{
	public class JsonStringLocalizer : IStringLocalizer
	{
		private readonly IDistributedCache _cache;
		private readonly JsonSerializer _serializer = new();
        private readonly IWebHostEnvironment _env;
		private static readonly AsyncLocal<string> MthThreadLocal = new AsyncLocal<string>();
		private readonly IHttpContextAccessor _httpContextAccessor;
        private string userLanguage;


		public JsonStringLocalizer(IDistributedCache cache, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor = null)
		{
			_cache = cache;
			_env = env;
			_httpContextAccessor = httpContextAccessor;
		}

		public LocalizedString this[string name]
		{
			get
			{
				var value = GetString(name);
				return new LocalizedString(name, value);
			}
		}

		public LocalizedString this[string name, params object[] arguments]
		{
			get
			{
				var actualValue = this[name];
				return !actualValue.ResourceNotFound
					? new LocalizedString(name, string.Format(actualValue.Value, arguments))
					: actualValue;
			}
		}
        public void SetUserLanguage(string userLanguage)
        {
            this.userLanguage = userLanguage;
        }

        public string GetUserLanguage()
        {
            return userLanguage;
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
		{
            var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
			using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			using StreamReader streamReader = new(stream);
			using JsonTextReader reader = new(streamReader);

			while (reader.Read())
			{
				if (reader.TokenType != JsonToken.PropertyName)
					continue;

				var key = reader.Value as string;
				reader.Read();
				var value = _serializer.Deserialize<string>(reader);
				yield return new LocalizedString(key, value);
			}
		}

        //private string GetString(string key)
        //{
        //    //var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
        //    //var fullFilePath = Path.GetFullPath(filePath);
        //    //var fullFilePath2 = Path.Combine(_env.ContentRootPath, filePath);

        //    //fullFilePath = fullFilePath.Replace("Admin.MVC", "App.Common");
        //    //fullFilePath = fullFilePath.Replace("App.Api", "App.Common");
        //    var filePath = $"{Thread.CurrentThread.CurrentCulture.Name}.json";
        //    var fullFilePath = Path.Combine(_env.ContentRootPath, "Resources", filePath);
        //    //fullFilePath = fullFilePath.Replace("Admin.MVC", "App.Common");
        //    if (File.Exists(fullFilePath))
        //    {
        //        // Begin Cashe
        //        var cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
        //        var cacheValue = _cache.GetString(cacheKey);

        //        if (!string.IsNullOrEmpty(cacheValue))
        //            return cacheValue;
        //        // End Cashe

        //        // result
        //        var result = GetValueFromJSON(key, fullFilePath);

        //        // Add result to cashe file if it is not exist
        //        if (!string.IsNullOrEmpty(result))
        //            _cache.SetString(cacheKey, result);

        //        return result;
        //    }
        //    else
        //    {
        //        fullFilePath = fullFilePath.Replace("Admin.MVC", "App.Common");
        //        if (File.Exists(fullFilePath))
        //        {
        //            var cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
        //            var cacheValue = _cache.GetString(cacheKey);

        //            if (!string.IsNullOrEmpty(cacheValue))
        //                return cacheValue;
        //            // End Cashe

        //            // result
        //            var result = GetValueFromJSON(key, fullFilePath);

        //            // Add result to cashe file if it is not exist
        //            if (!string.IsNullOrEmpty(result))
        //                _cache.SetString(cacheKey, result);
        //            return result;
        //        }
        //        else
        //        {

        //            fullFilePath = fullFilePath.Replace("App.Api", "App.Common");
        //            if (File.Exists(fullFilePath))
        //            {
        //                var cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
        //                var cacheValue = _cache.GetString(cacheKey);

        //                if (!string.IsNullOrEmpty(cacheValue))
        //                    return cacheValue;
        //                // End Cashe

        //                // result
        //                var result = GetValueFromJSON(key, fullFilePath);

        //                // Add result to cashe file if it is not exist
        //                if (!string.IsNullOrEmpty(result))
        //                    _cache.SetString(cacheKey, result);
        //                return result;
        //            }
        //            return string.Empty;
        //        }

        //    }


        //}
        private string GetString(string key)
        {
            var culture = Thread.CurrentThread.CurrentCulture.Name;
            var fileName = $"{culture}.json";

            // ✅ استخدم AppContext.BaseDirectory بدل الـ ContentRootPath
            var resourcesPath = Path.Combine(AppContext.BaseDirectory, "Resources", fileName);

            // ✅ fallback في حال الملف غير موجود
            if (!File.Exists(resourcesPath))
            {
                return $"[{key}]"; // return key only if missing file
            }

            var cacheKey = $"locale_{culture}_{key}";
            var cachedValue = _cache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(cachedValue))
                return cachedValue;

            var value = GetValueFromJSON(key, resourcesPath);

            if (!string.IsNullOrEmpty(value))
                _cache.SetString(cacheKey, value);

            return value ?? $"[{key}]";
        }

        //private string GetString(string key)
        //{
        //    var culture = Thread.CurrentThread.CurrentCulture.Name;
        //    var fileName = $"{culture}.json";

        //    var commonResourcesPath = Path.Combine(
        //        Directory.GetParent(_env.ContentRootPath)?.FullName ?? _env.ContentRootPath,
        //        "App.Common",
        //        "Resources",
        //        fileName
        //    );

        //    if (!File.Exists(commonResourcesPath))
        //    {
        //        var backendRoot = Path.GetFullPath(Path.Combine(_env.ContentRootPath, @"..\..\App.Common\Resources", fileName));
        //        if (File.Exists(backendRoot))
        //            commonResourcesPath = backendRoot;
        //    }

        //    if (!File.Exists(commonResourcesPath))
        //        return $"[{key}]"; 

        //    var cacheKey = $"locale_{culture}_{key}";
        //    var cachedValue = _cache.GetString(cacheKey);
        //    if (!string.IsNullOrEmpty(cachedValue))
        //        return cachedValue;

        //    var value = GetValueFromJSON(key, commonResourcesPath);

        //    if (!string.IsNullOrEmpty(value))
        //        _cache.SetString(cacheKey, value);

        //    return value ?? $"[{key}]";
        //}

        private string GetValueFromJSON(string propertyName, string filePath)
		{
			if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(filePath))
				return string.Empty;

			using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			using StreamReader streamReader = new(stream);
			using JsonTextReader reader = new(streamReader);

			while (reader.Read())
			{
				if (reader.TokenType == JsonToken.PropertyName && reader.Value as string == propertyName)
				{
					reader.Read();
					return _serializer.Deserialize<string>(reader);
				}
			}

			return string.Empty;
		}
	}
}
