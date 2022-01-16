using AspNetCoreIdentityAPI.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityAPI.Models
{
    public class IdentityContextSeed
    {
        public static async Task SeedEssentialAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(AuthorizationRole.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(AuthorizationRole.Roles.Editor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(AuthorizationRole.Roles.User.ToString()));

            AppUser defaultUser = new AppUser
            {
                UserName = AuthorizationRole.default_userName,
                Email = AuthorizationRole.default_email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(x=>x.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, AuthorizationRole.default_password);
                await userManager.AddToRoleAsync(defaultUser, AuthorizationRole.default_role.ToString());
            }
        }
    }
}
