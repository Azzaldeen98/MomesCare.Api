using Microsoft.AspNetCore.Identity;
using MomesCare.Api.Helpers.Enums;

namespace MomesCare.Api.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                if (roleManager.FindByNameAsync("Admin").Result == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin.ToString()));
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User.ToString()));
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Doctor.ToString()));

                }
            }
            catch (Exception ex) {

            }
        }


    
    }
}