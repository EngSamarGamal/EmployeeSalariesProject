using System.Security.Claims;
using App.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Common.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user) => user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        public static string GetAdminUserId(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User is null");

            var claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList();

            // Log claims to debug
            Console.WriteLine("User Claims:");
            foreach (var claim in claims)
            {
                Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? user.FindFirst("sub")?.Value // Some JWTs use "sub" (subject) instead
                      ?? user.FindFirst("uid")?.Value // Custom user ID claim
                      ?? user.FindFirst("aid")?.Value; // Application ID claim

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User ID claim is missing in the token");
            }

            return userId;
        }

        public static string GetUserIdAPI(this ClaimsPrincipal user) => user.FindFirst("uid")!.Value;
        public static string GetUserFullName(this ClaimsPrincipal user) => user.FindFirst("FullNaNE")!.Value;
        public static string GetUserEmail(this ClaimsPrincipal user) => user.FindFirst(ClaimTypes.Email)!.Value;
        //user.FindFirst("uid")!.Value;
        //user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        public static string GetLanguageFromHeader(this HttpRequest Request)
        {
            // en , it , ar , ....
            var env = Request.Headers["language"].FirstOrDefault();
            if (env == null)
            {
                env = "en";
            }
            return env;
            //if (env == null)
            //{
            //	env = Thread.CurrentThread.CurrentCulture.Name == "ar-sa" ? Enum.GetName((ELanguages)(int)ELanguages.AR) : Enum.GetName((ELanguages)(int)ELanguages.EN);
            //}
            //return (int)Enum.Parse(typeof(ELanguages), env?.ToUpper() ?? ELanguages.EN.ToString());
        }


        //Get Enum
        public static SelectList GenerateSelectListFromEnum<TEnum>(string mode, object databaseData = null) where TEnum : Enum
        {
            if (mode == "add")
            {
                var data = Enum.GetValues(typeof(TEnum))
                               .Cast<TEnum>()
                               .Select(e => new SelectListItem
                               {
                                   Value = ((int)(object)e).ToString(),
                                   Text = e.ToString()
                               })
                               .ToList();
                return new SelectList(data, "Value", "Text");
            }
            else if (mode == "edit" && databaseData != null)
            {
                var dataFromDb = databaseData as List<int>;
                if (dataFromDb == null)
                {
                    throw new ArgumentException("Invalid data passed for 'edit' mode");
                }

                var data = Enum.GetValues(typeof(TEnum))
                               .Cast<TEnum>()
                               .Select(e => new SelectListItem
                               {
                                   Value = ((int)(object)e).ToString(),
                                   Text = e.ToString(),
                                   Selected = dataFromDb.Contains((int)(object)e)
                               })
                               .ToList();
                return new SelectList(data, "Value", "Text");
            }
            else
            {
                return null;
            }
        }

        public static string GetEnvironmentFromHeader(this HttpRequest Request)
        {
            // en , it , ar , ....
            var env = Request.Headers["environment"].FirstOrDefault();
            if (env == null)
            {
                env = EnviromentEnum.ERP.ToString();
            }
            return env;
            //if (env == null)
            //{
            //	env = Thread.CurrentThread.CurrentCulture.Name == "ar-sa" ? Enum.GetName((ELanguages)(int)ELanguages.AR) : Enum.GetName((ELanguages)(int)ELanguages.EN);
            //}
            //return (int)Enum.Parse(typeof(ELanguages), env?.ToUpper() ?? ELanguages.EN.ToString());
        }


    }
}
