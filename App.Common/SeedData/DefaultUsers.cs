using App.Common.Const;
using App.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace App.Common.SeedData
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser
            {
                FirstName = "user",
                LastName = "user",
                //FullName = "User",
                UserName = "user@user.com",
                Email = "user@user.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                AccountType = 1,
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "P@ssw0rd");
                await userManager.AddToRoleAsync(defaultUser, RoleEnum.User.ToString());
            }
        }

        public static async Task SeedSuperAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManger)
        {
            var defaultUser = new ApplicationUser
            {
                FirstName = "admin",
                LastName = "admin",
                //FullName= "Admin",
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                AccountType = 1,
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "P@ssw0rd");               
            }

            foreach (var role in Enum.GetValues(typeof(RoleEnum)))
            {
                if (!await userManager.IsInRoleAsync(defaultUser, role.ToString()))
                {
                    await userManager.AddToRoleAsync(defaultUser, role.ToString());
                }
            }
            await roleManger.SeedClaimsForSuperUser();
        }


        private static async Task SeedClaimsForSuperUser(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(RoleEnum.SuperAdmin.ToString());
            // Todo Loop for Modules Enum and check for every permission in every module then add not exist
            //await roleManager.AddPermissionClaims(adminRole, Modules.Users.ToString());
        }

        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsList(module);

            try
            {
                foreach (var permission in allPermissions)
                {
                    if (!allClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                        await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
