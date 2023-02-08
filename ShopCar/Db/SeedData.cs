using Microsoft.AspNetCore.Identity;

namespace ShopCar.Db
    {
    public static class SeedData
        {
        public static async Task EnsureSeedData (IServiceProvider provider) {
          var roleMgr=  provider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach(var roleName in RoleNames.AllRole)
                {
                var role = roleMgr.FindByNameAsync(roleName).Result;
                if(role == null) {
                   var result= roleMgr.CreateAsync(new IdentityRole { Name = roleName }).Result;
                    if(!result.Succeeded) throw new Exception(result.Errors.First().Description);
                
                }

                UserManager<IdentityUser> userManager = provider.GetRequiredService<UserManager<IdentityUser>>();

               
                IdentityResult identityResultAdmin = await userManager.CreateAsync(DefaultUser.Admin, "qqqWWW1_");
                IdentityResult identityResultModerator = await userManager.CreateAsync(DefaultUser.Moderator, "qqqWWW1_");
                IdentityResult identityResultUser = await userManager.CreateAsync(DefaultUser.User, "qqqWWW1_");

                if(identityResultAdmin.Succeeded || identityResultModerator.Succeeded || identityResultUser.Succeeded) {
                   
                IdentityUser? identityAdmin = await userManager.FindByEmailAsync(DefaultUser.Admin.Email);
                IdentityUser? identityModerator = await userManager.FindByEmailAsync(DefaultUser.Moderator.Email);
                IdentityUser? identityUser = await userManager.FindByEmailAsync(DefaultUser.User.Email);

                    await userManager.AddToRoleAsync(identityAdmin, RoleNames.Admin);
                    await userManager.AddToRoleAsync(identityModerator, RoleNames.Moderator);
                    await userManager.AddToRoleAsync(identityUser, RoleNames.User);
                
                }




                }
          
          
                   
          
          
              
          }


        }
    }
