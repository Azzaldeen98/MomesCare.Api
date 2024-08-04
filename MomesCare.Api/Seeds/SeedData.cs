

using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Web.Api.Seeds;
using System.Diagnostics;

namespace MomesCare.Api.Seeds;

public class SeedData
{


    public async static void EnsureSeedData(WebApplication app, FirebaseAuthClient _firebaseAuthClient)
    {

        try
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<DataContext>();
                //context.Database.Migrate();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


                await DefaultRoles.SeedAsync(roleManager);
                if (context != null)
                {
                    var defUser = new DefaultUsers(_firebaseAuthClient, userManager, roleManager, context.Users);
                    await defUser.SeedAdminAsync();
                    await defUser.SeedUserAsync();
                    await DefaultOthers.SeedDefaultDataAsync(context);

                }

            }

        }catch(Exception ex) { 
        
                 Debug.WriteLine(ex);
        }
    }
}
