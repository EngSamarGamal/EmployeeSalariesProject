using App.Dashboard.api.Repository;
using Microsoft.AspNetCore.Authentication;
namespace LucaStay.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string? AccessToken => _httpContextAccessor.HttpContext?.GetTokenAsync("access_token").Result;
        public string? UserId => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type.Equals("uid", StringComparison.OrdinalIgnoreCase))?.Value;
    }
}
