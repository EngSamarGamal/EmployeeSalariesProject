using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace App.Common.Localization
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IDistributedCache _cache;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JsonStringLocalizerFactory(IDistributedCache cache, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer(_cache, _env, _httpContextAccessor);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer(_cache, _env, _httpContextAccessor);
        }
    }
}
