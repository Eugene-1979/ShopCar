using Microsoft.AspNetCore.Identity;

namespace ShopCar.Db
    {
    public static class RoleNames
        {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Moderator = "Moderator";

        public static IEnumerable<string> AllRole
            {
            get
                {
                yield return Admin;
                yield return User;
                yield return Moderator;

                }
            } 
        }


    public static class DefaultUser {
        public static readonly IdentityUser Admin = new IdentityUser
            {
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            UserName = "admin@gmail.com"
            };
            
            public static readonly IdentityUser Moderator = new IdentityUser
            {
            Email = "moderator@gmail.com",
                EmailConfirmed = true,
                UserName = "moderator@gmail.com"
            };  
            public static readonly IdentityUser User = new IdentityUser
            {
            Email = "user@gmail.com",
            EmailConfirmed = true,
            UserName = "user@gmail.com"
            };
        public static IEnumerable<IdentityUser> AllUser
            {
            get
                {
                yield return Admin;
                yield return User;
                yield return Moderator;

                }
            }
        }
    }
