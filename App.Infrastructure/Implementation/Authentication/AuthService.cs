
using App.Common.Enums;
using App.Common.Response;
using App.Domain.Models.Identity;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SharePoint.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;

namespace App.Infrastructure.Services.Identity
{
    //    public class AuthService : IAuthService
    //    {
    //        private readonly UserManager<ApplicationUser> _userManager;
    //        private readonly RoleManager<IdentityRole> _roleManager;
    //        private readonly IStringLocalizer _localizer;
    //        private readonly IConfiguration _config;
    //        private readonly PasswordResetOptions _resetOptions;
    //        private readonly IEmailSenderCustom _emailSender;
    //        private readonly MailSettings _mailSettings;
    //        private readonly AppSettings _appSettings;
    //        private readonly ILogger<AuthService> _logger;



    //        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config,
    //            IStringLocalizer localizer, IOptions<PasswordResetOptions> resetOptions, IEmailSenderCustom emailSender, IOptions<MailSettings> mailSettings, IOptions<AppSettings> appSettings, ILogger<AuthService> logger)
    //        {
    //            _userManager = userManager;
    //            _roleManager = roleManager;
    //            _config = config;
    //            _localizer = localizer;
    //            _resetOptions = resetOptions.Value;
    //            _emailSender = emailSender;
    //            _mailSettings = mailSettings.Value;
    //            _appSettings = appSettings.Value;
    //            _logger = logger;
    //        }

    //        public async Task<bool> SendPasswordResetAsync(string email, string? returnUrl = null)
    //        {
    //            var user = await _userManager.FindByEmailAsync(email);
    //            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
    //                return true;

    //            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    //            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

    //            var baseUrl = string.IsNullOrWhiteSpace(returnUrl)
    //                ? _resetOptions.ResetLinkBaseUrl
    //                : returnUrl;

    //            var resetLink = QueryHelpers.AddQueryString(baseUrl, new Dictionary<string, string?>
    //            {
    //                ["email"] = email,
    //                ["token"] = encodedToken
    //            });

    //            var subject = _localizer["PasswordResetSubject"].Value;
    //            var body = BuildPasswordResetEmailBody(user.FirstName, resetLink);

    //            await _emailSender.SendEmailAsync(email, subject, body);
    //            return true;
    //        }

    //        private string BuildPasswordResetEmailBody(string? firstName, string resetLink)
    //        {
    //            var greetingName = string.IsNullOrWhiteSpace(firstName)
    //                ? _resetOptions.EmailGreeting
    //                : $"{_resetOptions.EmailGreeting} {firstName},";
    //            string imageUrl = "https://filemanager-lucastay-a4gjbpdqekd0chfg.westeurope-01.azurewebsites.net//Uploads/DividoImage/ee2370e8-0a48-45c7-895f-eaa56d2163e5.jpg";


    //            return $@"
    //        <div style='font-family:Segoe UI,Arial,sans-serif; background-color:#f9f9f9; padding:40px;'>
    //            <div style='max-width:600px;margin:auto;background:#ffffff;border-radius:8px;box-shadow:0 2px 6px rgba(0,0,0,0.1);padding:30px;'>
    //                        <div align='center' style='padding-bottom:20px;'>
    //                            <img src='{imageUrl}' alt='Divido Logo' style='height:55px;' />
    //                        </div>
    //                <h2 style='color:#272727; text-align:center; margin-bottom:24px;'>Reset Your Account Password</h2>

    //                <p style='font-size:15px;color:#333;'><strong>{greetingName}</strong></p>
    //                <p style='font-size:15px;color:#333; line-height:1.6;'>{_resetOptions.EmailBody}</p>

    //                <div style='text-align:center;margin:30px 0;'>
    //                    <a href='{HtmlEncoder.Default.Encode(resetLink)}'
    //                       style='display:inline-block; background-color:#5DBA47; color:#272727; padding:12px 28px; border-radius:8px; font-size:16px; font-weight:600; text-decoration:none;'>
    //                       {_resetOptions.ButtonText}
    //                    </a>
    //                </div>

    //                <p style='font-size:13px;color:#777;'>If you did not request a password reset, you can safely ignore this email.</p>

    //                <hr style='border:none;border-top:1px solid #eee;margin:24px 0;'/>
    //                <p style='font-size:12px;color:#999;text-align:center;'>&copy; {DateTime.UtcNow.Year} SB Technology. All rights reserved.</p>
    //            </div>
    //        </div>";
    //        }


    //        public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword)
    //        {
    //            var user = await _userManager.FindByEmailAsync(email);
    //            if (user is null)
    //                return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = "User not found." });

    //            string decodedToken;
    //            try
    //            {
    //                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
    //            }
    //            catch
    //            {
    //                return IdentityResult.Failed(new IdentityError { Code = "InvalidToken", Description = "Invalid reset token." });
    //            }

    //            var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);
    //            return result;
    //        }

    //        #region MVC Login Helpers

    //        public async Task<AuthModel> LoginMvcAsync(LoginRequest request, HttpContext httpContext)
    //        {
    //            var result = await LoginAsync(request);

    //            if (!result.IsAuthenticated)
    //                return result;

    //            httpContext.Session.SetString("Token", result.Token ?? "");
    //            httpContext.Session.SetString("RefreshToken", result.RefreshToken ?? "");
    //            httpContext.Session.SetString("UserEmail", result.Email ?? "");
    //            httpContext.Session.SetString("UserName", result.FullName ?? "");
    //            httpContext.Session.SetString("UserRole", result.Role ?? "");
    //            httpContext.Session.SetString("ExpiresOn", result.ExpiresOn?.ToString("O") ?? "");

    //            return result;
    //        }

    //        public async Task<bool> LogoutMvcAsync(HttpContext httpContext)
    //        {
    //            // Clear all session variables
    //            httpContext.Session.Clear();

    //            // Sign out cookie authentication
    //            //await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


    //            //Delete all cookies Except Culture
    //            foreach (var cookie in httpContext.Request.Cookies.Keys)
    //            {
    //                if (!cookie.StartsWith(".AspNetCore.Culture"))
    //                    httpContext.Response.Cookies.Delete(cookie);
    //            }
    //            return true;
    //        }

    //        #endregion


    //        #region Register
    //        public async Task<AuthModel> RegisterAsync(RegisterationRequest request)
    //        {
    //            if (await _userManager.FindByEmailAsync(request.Email) != null)
    //                return new AuthModel { Message = "Email is already registered." };

    //            var user = new ApplicationUser
    //            {
    //                UserName = request.Email,
    //                Email = request.Email,
    //                FirstName = request.FirstName,
    //                LastName = request.LastName,
    //                CreatedOn = DateTime.UtcNow,
    //                IsActive = true,
    //                AccountType = request.AccountType
    //            };

    //            var result = await _userManager.CreateAsync(user, request.Password);
    //            if (!result.Succeeded)
    //                return new AuthModel { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };

    //            await _userManager.AddToRoleAsync(user, "SuperAdmin");

    //            var jwt = await CreateJwtToken(user);
    //            var refreshToken = GenerateRefreshToken();
    //            user.RefreshTokens ??= new List<RefreshToken>();
    //            user.RefreshTokens.Add(refreshToken);
    //            await _userManager.UpdateAsync(user);

    //            return new AuthModel
    //            {
    //                Id = Guid.Parse(user.Id),
    //                Email = user.Email!,
    //                FullName = user.FullName!,
    //                Token = jwt.Token,
    //                ExpiresOn = jwt.ExpiresOn,
    //                RefreshToken = refreshToken.Token,
    //                RefreshTokenExpiration = refreshToken.RefreshTokenExpireOn,
    //                IsAuthenticated = true,
    //                Role = "SuperAdmin",
    //                Message = "Registration successful"
    //            };
    //        }
    //        #endregion

    //        #region Login
    //        public async Task<AuthModel> LoginAsync(LoginRequest request)
    //        {
    //            var user = await _userManager.Users
    //                .Include(u => u.RefreshTokens)
    //                .FirstOrDefaultAsync(u => u.Email == request.Email);

    //            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
    //            {
    //                return new AuthModel
    //                {
    //                    Message = _localizer["Login_InvalidCredentials"]
    //                };
    //            }

    //            var jwt = await CreateJwtToken(user);
    //            var refreshToken = GenerateRefreshToken();

    //            user.RefreshTokens ??= new List<RefreshToken>();
    //            user.RefreshTokens.Add(refreshToken);
    //            await _userManager.UpdateAsync(user);

    //            return new AuthModel
    //            {
    //                Id = Guid.Parse(user.Id),
    //                Email = user.Email!,
    //                FullName = user.FullName!,
    //                Token = jwt.Token,
    //                ExpiresOn = jwt.ExpiresOn,
    //                RefreshToken = refreshToken.Token,
    //                RefreshTokenExpiration = refreshToken.RefreshTokenExpireOn,
    //                IsAuthenticated = true,
    //                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
    //            };
    //        }
    //        public async Task<AuthModel> LoginOwnerAsync(LoginRequest request)
    //        {
    //            var authModel = new AuthModel()
    //            {
    //                IsSuccess = true,
    //                AccountType = 0,
    //                Message = ""
    //            };
    //            var user = await _userManager.Users
    //                .Include(u => u.RefreshTokens)
    //                .FirstOrDefaultAsync(u => u.Email == request.Email);

    //            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
    //            {
    //                authModel.Message = _localizer["Login_InvalidCredentials"];
    //                authModel.IsSuccess = false;
    //            }
    //            if (!user.IsActive)
    //            {
    //                authModel.Message += _localizer["Login_IsNotActive"];
    //                authModel.IsSuccess = false;
    //            }
    //            if (user.AccountType != (int)AccountTypeEnum.Owner)
    //            {
    //                authModel.Message += ", "+_localizer["Login_RoleNotAuthorized"];
    //                authModel.IsSuccess = false;
    //            }
    //            if (!authModel.IsSuccess)
    //            {
    //                _logger.LogError(authModel.Message);
    //                return authModel;
    //            }
    //            var jwt = await CreateJwtToken(user);
    //            var refreshToken = GenerateRefreshToken();

    //            user.RefreshTokens ??= new List<RefreshToken>();
    //            user.RefreshTokens.Add(refreshToken);
    //            var result = await _userManager.UpdateAsync(user);
    //            if (!result.Succeeded)
    //            {
    //                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
    //                authModel.Message = errors;
    //                authModel.IsSuccess = false;
    //                _logger.LogError(authModel.Message);
    //                return authModel;
    //            }
    //            return new AuthModel
    //            {
    //                Id = Guid.Parse(user.Id),
    //                Email = user.Email!,
    //                FullName = user.FullName!,
    //                Token = jwt.Token,
    //                ExpiresOn = jwt.ExpiresOn,
    //                RefreshToken = refreshToken.Token,
    //                RefreshTokenExpiration = refreshToken.RefreshTokenExpireOn,
    //                IsAuthenticated = true,
    //                AccountType = (int)AccountTypeEnum.Owner,
    //                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
    //            };
    //        }
    //        #endregion
    //        #region Logout
    //        public async Task<IResponseModel> LogoutAsync(string userId)
    //        {
    //            IResponseModel responseModel = new ResponseModel();
    //            var user = await _userManager.FindByIdAsync(userId);
    //            if (user == null)
    //            {
    //                //responseModel.Response((int)HttpStatusCode.NotFound, true, _localizer["NoAdminsFound"],null);
    //                _logger.LogError(_localizer["NoAdminsFound"]);

    //                return ResponseModel.Response((int)HttpStatusCode.NotFound, true, _localizer["NoAdminsFound"], null);

    //            }
    //            user.RefreshTokens = null; // or mark it as expired
    //            var result = await _userManager.UpdateAsync(user);
    //            if (!result.Succeeded)
    //            {
    //                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
    //                _logger.LogError(errors);
    //                return ResponseModel.Response((int)HttpStatusCode.InternalServerError, true, _localizer["NoAdminsFound"], null);

    //                // responseModel.Response((int)HttpStatusCode.InternalServerError, true, errors, null);
    //            }
    //            return ResponseModel.Response((int)HttpStatusCode.OK, false, _localizer["LoggedOut"], null);
    //        }
    //        #endregion
    //        #region Refresh Token
    //        public async Task<AuthModel> RefreshTokenAsync(string token)
    //        {
    //            var user = await _userManager.Users
    //                .Include(u => u.RefreshTokens)
    //                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

    //            if (user == null)
    //                return new AuthModel { Message = "Invalid token." };

    //            var oldToken = user.RefreshTokens!.First(t => t.Token == token);
    //            if (!oldToken.IsActive)
    //                return new AuthModel { Message = "Inactive or expired refresh token." };

    //            oldToken.RevokedOn = DateTime.UtcNow;
    //            var newRefreshToken = GenerateRefreshToken();
    //            user.RefreshTokens.Add(newRefreshToken);
    //            await _userManager.UpdateAsync(user);

    //            var jwt = await CreateJwtToken(user);

    //            return new AuthModel
    //            {
    //                Email = user.Email!,
    //                Token = jwt.Token,
    //                ExpiresOn = jwt.ExpiresOn,
    //                RefreshToken = newRefreshToken.Token,
    //                RefreshTokenExpiration = newRefreshToken.RefreshTokenExpireOn,
    //                IsAuthenticated = true
    //            };
    //        }
    //        #endregion

    //        #region Revoke Token
    //        public async Task<bool> RevokeTokenAsync(string token)
    //        {
    //            var user = await _userManager.Users
    //                .Include(u => u.RefreshTokens)
    //                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

    //            if (user == null)
    //                return false;

    //            var refreshToken = user.RefreshTokens!.FirstOrDefault(t => t.Token == token);
    //            if (refreshToken == null || !refreshToken.IsActive)
    //                return false;

    //            refreshToken.RevokedOn = DateTime.UtcNow;
    //            await _userManager.UpdateAsync(user);
    //            return true;
    //        }
    //        #endregion

    //        #region Helpers
    //        private async Task<(string Token, DateTime ExpiresOn)> CreateJwtToken(ApplicationUser user)
    //        {
    //            var userClaims = await _userManager.GetClaimsAsync(user);
    //            var roles = await _userManager.GetRolesAsync(user);
    //            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

    //            var claims = new List<Claim>
    //            {
    //                new(ClaimTypes.NameIdentifier, user.Id),
    //                new(ClaimTypes.Email, user.Email ?? ""),
    //                new(ClaimTypes.Name, user.FullName ?? ""),
    //                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    //            }
    //            .Union(userClaims)
    //            .Union(roleClaims);

    //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
    //            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //            var expires = DateTime.UtcNow.AddHours(double.Parse(_config["Jwt:DurationInHours"]!));

    //            var token = new JwtSecurityToken(
    //                issuer: _config["Jwt:Issuer"],
    //                audience: _config["Jwt:Audience"],
    //                claims: claims,
    //                expires: expires,
    //                signingCredentials: creds
    //            );

    //            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    //        }

    //        private RefreshToken GenerateRefreshToken()
    //        {
    //            var randomBytes = new byte[32];
    //            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
    //            rng.GetBytes(randomBytes);

    //            return new RefreshToken
    //            {
    //                Token = Convert.ToBase64String(randomBytes),
    //                CreatedOn = DateTime.UtcNow,
    //                ExpiresOn = DateTime.UtcNow.AddHours(double.Parse(_config["Jwt:DurationInHours"]!)),
    //                RefreshTokenExpireOn = DateTime.UtcNow.AddDays(double.Parse(_config["Jwt:RefreshTokenValidityInDays"]!))
    //            };
    //        }
    //        #endregion

    //        #region Admins
    //        public async Task<List<AdminItemDto>> GetAllAdminsAsync()
    //        {
    //            var users = await _userManager.Users
    //                .Where(u => u.AccountType == (int)AccountTypeEnum.Admin || u.AccountType == (int)AccountTypeEnum.SuperAdmin)
    //                .ToListAsync();

    //            var result = new List<AdminItemDto>();

    //            foreach (var u in users)
    //            {
    //                var roles = await _userManager.GetRolesAsync(u);

    //                result.Add(new AdminItemDto
    //                {
    //                    Id = u.Id,
    //                    FirstName = u.FirstName,
    //                    LastName = u.LastName,
    //                    Email = u.Email!,
    //                    PhoneNumber = u.PhoneNumber ?? "",
    //                    Role = roles.FirstOrDefault() ?? "",
    //                    IsActive = u.IsActive
    //                });
    //            }

    //            return result;
    //        }

    //        #region Create Admin
    //        public async Task<AuthModel> CreateAdminAsync(CreateAdminRequest request)
    //        {
    //            if (await _userManager.FindByEmailAsync(request.Email) != null)
    //                return new AuthModel { IsSuccess = false, Message = _localizer["EmailAlreadyExists"].Value };

    //            var existingPhoneUser = await _userManager.Users
    //                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

    //            if (existingPhoneUser != null)
    //                return new AuthModel { IsSuccess = false, Message = _localizer["PhoneNumberAlreadyExists"].Value };

    //            var user = new ApplicationUser
    //            {
    //                UserName = request.Email,
    //                Email = request.Email,
    //                FirstName = request.FirstName,
    //                LastName = request.LastName,
    //                PhoneNumber = request.PhoneNumber,
    //                CreatedOn = DateTime.UtcNow,
    //                IsActive = false,
    //                AccountType = (int)AccountTypeEnum.SuperAdmin
    //            };

    //            var result = await _userManager.CreateAsync(user);
    //            if (!result.Succeeded)
    //            {
    //                return new AuthModel
    //                {
    //                    IsSuccess = false,
    //                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
    //                };
    //            }

    //            await _userManager.AddToRoleAsync(user, "SuperAdmin");

    //            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    //            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
    //            var activationLink = $"{_appSettings.BaseUrl}/Accounts/ActivateAccount?userId={user.Id}&token={encodedToken}";

    //            var subject = _localizer["ActivateAccountSubject"].Value;
    //            var body = BuildActivationEmailBody(user.FirstName, activationLink);

    //            await _emailSender.SendEmailAsync(user.Email!, subject, body);

    //            return new AuthModel
    //            {
    //                Id = Guid.Parse(user.Id),
    //                Email = user.Email!,
    //                FullName = $"{user.FirstName} {user.LastName}",
    //                Role = "SuperAdmin",
    //                IsAuthenticated = false,
    //                IsSuccess = true,
    //                Message = _localizer["AdminCreate_Success_PendingActivation"].Value
    //            };
    //        }
    //        #endregion

    //        public async Task<bool> ResetPasswordAsyncForAdmin(string userId, string token, string newPassword)
    //        {
    //            var user = await _userManager.FindByIdAsync(userId);
    //            if (user == null) return false;

    //            var decodedBytes = WebEncoders.Base64UrlDecode(token);
    //            var decodedToken = Encoding.UTF8.GetString(decodedBytes);

    //            var resetResult = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

    //            if (!resetResult.Succeeded)
    //            {
    //                _logger.LogError("Password reset failed for user {Email}: {Errors}",
    //                    user.Email, string.Join(", ", resetResult.Errors.Select(e => e.Description)));
    //                return false;
    //            }

    //            user.IsActive = true;
    //            user.EmailConfirmed = true;
    //            user.PhoneNumberConfirmed = true;
    //            await _userManager.UpdateAsync(user);
    //            return true;
    //        }

    //        private string BuildActivationEmailBody(string firstName, string activationLink)
    //        {
    //            string imageUrl = "https://filemanager-lucastay-a4gjbpdqekd0chfg.westeurope-01.azurewebsites.net//Uploads/DividoImage/ee2370e8-0a48-45c7-895f-eaa56d2163e5.jpg";
    //            return $@"
    //<!DOCTYPE html>
    //<html lang='en'>
    //<head>
    //    <meta charset='UTF-8' />
    //    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    //    <title>Activate Account</title>
    //</head>
    //<body style='margin:0; padding:0; background-color:#f8f9fa; font-family:Arial, Helvetica, sans-serif;'>
    //    <table width='100%' cellspacing='0' cellpadding='0' border='0' style='background-color:#f8f9fa; padding:40px 0;'>
    //        <tr>
    //            <td align='center'>
    //                <table width='600' cellpadding='0' cellspacing='0' border='0' style='background-color:#ffffff; border-radius:10px; box-shadow:0 2px 8px rgba(0,0,0,0.1); padding:40px;'>
    //                    <tr>
    //                        <td align='center' style='padding-bottom:20px;'>
    //                            <img src='{imageUrl}' alt='Divido Logo' style='height:55px;' />
    //                        </td>
    //                    </tr>
    //                    <tr>
    //                        <td align='center' style='color:#333333; font-size:24px; font-weight:bold; padding-bottom:10px;'>
    //                            Welcome to Divido Admin
    //                        </td>
    //                    </tr>
    //                    <tr>
    //                        <td align='left' style='color:#555555; font-size:16px; line-height:1.6; padding-bottom:15px;'>
    //                            Hello <strong>{firstName}</strong>,
    //                        </td>
    //                    </tr>
    //                    <tr>
    //                        <td align='left' style='color:#555555; font-size:16px; line-height:1.6;'>
    //                            Your account has been successfully created. To activate it and set your password, please click the button below.
    //                        </td>
    //                    </tr>
    //                    <tr>
    //                        <td align='center' style='padding:30px 0;'>
    //                            <a href='{activationLink}'
    //                               style='display:inline-block; background-color:#5DBA47; color:#272727; padding:12px 28px; border-radius:8px; font-size:16px; font-weight:600; text-decoration:none;'>
    //                                Activate My Account
    //                            </a>
    //                        </td>
    //                    </tr>
    //                    <tr>
    //                        <td align='center' style='padding-top:20px; color:#888888; font-size:14px; line-height:1.5;'>
    //                            If you didn’t request this, please ignore this email.<br/>
    //                            <span style='color:#aaaaaa;'>© {DateTime.UtcNow.Year} Divido. All rights reserved.</span>
    //                        </td>
    //                    </tr>
    //                </table>
    //            </td>
    //        </tr>
    //    </table>
    //</body>
    //</html>";
    //        }



    //        public async Task<string> GenerateActivationTokenAsync(string userId)
    //            {
    //                var user = await _userManager.FindByIdAsync(userId);
    //                if (user == null)
    //                    throw new Exception("User not found");

    //                return await _userManager.GeneratePasswordResetTokenAsync(user);
    //            }

    //         public async Task<bool> ValidateActivationTokenAsync(string userId, string token)
    //            {
    //                var user = await _userManager.FindByIdAsync(userId);
    //                if (user == null)
    //                    return false;

    //            var decodedBytes = WebEncoders.Base64UrlDecode(token);
    //            var decodedToken = Encoding.UTF8.GetString(decodedBytes);

    //            return await _userManager.VerifyUserTokenAsync(
    //                user,
    //                _userManager.Options.Tokens.PasswordResetTokenProvider,
    //                UserManager<ApplicationUser>.ResetPasswordTokenPurpose,
    //                decodedToken);

    //        }

    //        public async Task<bool> ActivateAccountAsync(string userId)
    //            {
    //                var user = await _userManager.FindByIdAsync(userId);
    //                if (user == null)
    //                    return false;

    //                user.IsActive = true;
    //                await _userManager.UpdateAsync(user);
    //                return true;
    //        }


    //        #endregion
    //        public async Task<bool> ChangePassword(string userId, string currentPassword)
    //        {
    //            var user = await _userManager.FindByIdAsync(userId);

    //            if (user == null)
    //            {
    //                return false;
    //            }
    //            else
    //            {
    //                if (!await _userManager.CheckPasswordAsync(user, currentPassword))
    //                {
    //                    return false;
    //                }
    //                else
    //                {
    //                    return true;
    //                }
    //            }
    //        }
    //        #region Owners
    //        public async Task<List<AdminItemDto>> GetAllOwnersAsync()
    //        {
    //            var users = await _userManager.Users
    //                .Where(u => u.AccountType == (int)AccountTypeEnum.Owner)
    //                .ToListAsync();

    //            var result = new List<AdminItemDto>();

    //            foreach (var u in users)
    //            {
    //                var roles = await _userManager.GetRolesAsync(u);

    //                result.Add(new AdminItemDto
    //                {
    //                    Id = u.Id,
    //                    FirstName = u.FirstName,
    //                    LastName = u.LastName,
    //                    Email = u.Email!,
    //                    PhoneNumber = u.PhoneNumber ?? "",
    //                    Role = roles.FirstOrDefault() ?? "",
    //                    IsActive = u.IsActive,
    //                    IsOwner = u.IsOwner
    //                });
    //            }

    //            return result;
    //        }
    //        #endregion
    //        #region Create Admin
    //        public async Task<AuthModel> CreateOwnerAsync(CreateAdminRequest request)
    //        {
    //            if (await _userManager.FindByEmailAsync(request.Email) != null)
    //                return new AuthModel { IsSuccess = false, Message = _localizer["EmailAlreadyExists"].Value };

    //            var existingPhoneUser = await _userManager.Users
    //                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

    //            if (existingPhoneUser != null)
    //                return new AuthModel { IsSuccess = false, Message = _localizer["PhoneNumberAlreadyExists"].Value };

    //            var user = new ApplicationUser
    //            {
    //                UserName = request.Email,
    //                Email = request.Email,
    //                FirstName = request.FirstName,
    //                LastName = request.LastName,
    //                PhoneNumber = request.PhoneNumber,
    //                CreatedOn = DateTime.UtcNow,
    //                IsActive = false,
    //                AccountType = (int)AccountTypeEnum.Owner,
    //                IsOwner=request.IsOwner,
    //            };

    //            var result = await _userManager.CreateAsync(user);
    //            if (!result.Succeeded)
    //            {
    //                return new AuthModel
    //                {
    //                    IsSuccess = false,
    //                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
    //                };
    //            }

    //            await _userManager.AddToRoleAsync(user, "SuperAdmin");

    //            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    //            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
    //            var activationLink = $"{_appSettings.BaseUrl}/Accounts/ActivateAccount?userId={user.Id}&token={encodedToken}";

    //            var subject = _localizer["ActivateAccountSubject"].Value;
    //            var body = BuildActivationEmailBody(user.FirstName, activationLink);

    //            await _emailSender.SendEmailAsync(user.Email!, subject, body);

    //            return new AuthModel
    //            {
    //                Id = Guid.Parse(user.Id),
    //                Email = user.Email!,
    //                FullName = $"{user.FirstName} {user.LastName}",
    //                Role = "SuperAdmin",
    //                IsAuthenticated = false,
    //                IsSuccess = true,
    //                Message = _localizer["AdminCreate_Success_PendingActivation"].Value
    //            };
    //        }
    //        #endregion

    //    }
}
