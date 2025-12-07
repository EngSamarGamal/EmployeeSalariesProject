using App.Common.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Application.Interfaces.Authentication
{
    //public interface IAuthService
    //{
    //    Task<AuthModel> RegisterAsync(RegisterationRequest request);
    //    Task<AuthModel> LoginAsync(LoginRequest request);
    //    Task<AuthModel> LoginOwnerAsync(LoginRequest request);
    //    Task<IResponseModel> LogoutAsync(string userId);
    //    Task<AuthModel> RefreshTokenAsync(string token);
    //    Task<bool> RevokeTokenAsync(string token);
    //    Task<AuthModel> LoginMvcAsync(LoginRequest request, HttpContext httpContext);
    //    Task<bool> LogoutMvcAsync(HttpContext httpContext);

    //    Task<bool> SendPasswordResetAsync(string email, string? returnUrl = null);
    //    Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword);
    //    Task<List<AdminItemDto>> GetAllAdminsAsync();
    //    Task<AuthModel> CreateAdminAsync(CreateAdminRequest request);
    //    Task<string> GenerateActivationTokenAsync(string userId);
    //    Task<bool> ValidateActivationTokenAsync(string userId, string token);
    //    Task<bool> ResetPasswordAsyncForAdmin(string userId, string token, string newPassword);
    //    Task<bool> ChangePassword(string userId,string currentPassword);
    //    Task<List<AdminItemDto>> GetAllOwnersAsync();
    //    Task<AuthModel> CreateOwnerAsync(CreateAdminRequest request);

    //}
}
