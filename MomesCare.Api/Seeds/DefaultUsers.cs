
using Firebase.Auth;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Entities.Models;
using System.Security.Claims;



namespace MomesCare.Api.Web.Api.Seeds
{
    public  class DefaultUsers
    {
        private  readonly FirebaseAuthClient _firebaseAuthClient;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly DbSet<ApplicationUser> users;

        public DefaultUsers(FirebaseAuthClient firebaseAuthClient, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            DbSet<ApplicationUser> users)
        {
            this._firebaseAuthClient = firebaseAuthClient;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.users = users;
        }

        public  async Task SeedAdminAsync()
        {


            try
            {

                var response = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync("admin@gmail.com", "Test@123", "Admin");
                if (response != null && response.User != null && response.User.Credential != null && response.User.Uid != null)
                {

                    //Seed Default User
                    var defaultUser = new ApplicationUser
                    {
                        Id = response.User.Info.Uid,
                        FirstName = response.User.Info.DisplayName,
                        LastName = response.User.Info.DisplayName,
                        UserName = response.User.Info.Email,
                        Email = response.User.Info.Email,
                        EmailConfirmed = true,
                    };


                    if (userManager.Users.All(u => u.Id != defaultUser.Id && u.Email != defaultUser.Email))
                    {
                        var user = await userManager.FindByNameAsync(defaultUser.UserName);
                        if (user == null)
                        {
                            await userManager.CreateAsync(defaultUser, "Test@123");
                            await userManager.AddToRoleAsync(defaultUser, MomesCare.Api.Helpers.Enums.UserRoles.Admin.ToString());
                            await userManager.AddClaimsAsync(defaultUser, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Admin"),
                            new Claim(JwtClaimTypes.GivenName, "Admin"),
                            new Claim(JwtClaimTypes.FamilyName, "Admin"),
                            new Claim(JwtClaimTypes.WebSite, "http://admin.com")
                        });
                        }
                    }



                }
            }
            catch(Exception ex) { }
           
           
        }
        public async Task SeedUserAsync()
        {

            try {

                for (int i = 1; i <= 5; i++)
                {
                    string name = "user" + i;


                    var response = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(name + "@gmail.com", "Test@123", name);
                    if (response != null && response.User != null && response.User.Credential != null && response.User.Uid != null)
                    {


                        var defaultUser = new ApplicationUser
                        {
                            Id = response.User.Uid,
                            FirstName = name,
                            LastName = " ",
                            UserName = response.User.Info.Email,
                            Email = response.User.Info.Email
                        };

                        if (await users.FirstOrDefaultAsync(x => x.Email == defaultUser.Email || x.UserName == defaultUser.UserName) == null)
                        {

                            var identityResult = await userManager.CreateAsync(defaultUser, "Test@123");

                            if (identityResult.Succeeded)
                            {

                                await userManager.AddToRoleAsync(defaultUser, MomesCare.Api.Helpers.Enums.UserRoles.User.ToString());
                                Console.WriteLine($"Register is Successfully:{i} ");

                            }
                        }

                    }

                }

            }
            catch(Exception ex) { }

           
        }
       

    }
}