using App.Common.Const;
using App.Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace App.Common.SeedData
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManger)
        {
            //if (!roleManger.Roles.Any())
            //{
            //    await roleManger.CreateAsync(new IdentityRole(RolesEnum.SuperAdmin.ToString()));
            //    await roleManger.CreateAsync(new IdentityRole(RolesEnum.User.ToString()));
            //}


            var roles = Enum.GetValues(typeof(RoleEnum));
            foreach (RoleEnum role in roles)
            {
                if (!roleManger.RoleExistsAsync(role.ToString()).Result)
                {
                    await roleManger.CreateAsync(new IdentityRole(role.ToString()));
                }
            }




        }
    }
}
