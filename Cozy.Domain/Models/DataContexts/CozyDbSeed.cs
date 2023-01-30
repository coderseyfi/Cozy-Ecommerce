using Cozy.Domain.Models.Entities.Membership;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cozy.Domain.Models.DataContext
{
    public static class CozyDbSeed
    {
        public static IApplicationBuilder SeedMembership(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<CozyUser>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CozyUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<CozyRole>>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                string superAdminRoleName = configuration["defaultAccount:superAdmin"];
                string superAdminEmail = configuration["defaultAccount:email"];
                string superAdminUserName = configuration["defaultAccount:username"];
                string superAdminPassword = configuration["defaultAccount:password"];
                string superAdminName = configuration["defaultAccount:name"];
                string superAdminSurname= configuration["defaultAccount:surname"];

                var superAdminRole = roleManager.FindByNameAsync(superAdminRoleName).Result;


                if (superAdminRole == null)
                {
                    superAdminRole = new CozyRole
                    {
                        Name = superAdminRoleName,
                    };

                    var roleResult = roleManager.CreateAsync(superAdminRole).Result;

                    if (!roleResult.Succeeded)
                    {
                        throw new Exception("Problem at RoleCreating.....");
                    }
                }

                var superAdminUser = userManager.FindByEmailAsync(superAdminEmail).Result;

                if (superAdminUser == null)
                {
                    superAdminUser = new CozyUser
                    {
                        Email = superAdminEmail,
                        UserName = superAdminUserName,
                        EmailConfirmed = true,
                        Name=superAdminName,
                        Surname=superAdminSurname
                    };

                    var userResult = userManager.CreateAsync(superAdminUser, superAdminPassword).Result;

                    if (!userResult.Succeeded)
                    {
                        throw new Exception("Problem occurred when user is created");
                    }
                }

                var isInRole = userManager.IsInRoleAsync(superAdminUser, superAdminRole.Name).Result;

                if (isInRole != true)
                {
                    userManager.AddToRoleAsync(superAdminUser, superAdminRole.Name).Wait();
                }

            }


            return app;
        }
        public static void SeedUserRole(RoleManager<CozyRole> roleManager)
        {

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                CozyRole role = new CozyRole();
                role.Name = "User";

                IdentityResult roleResult = roleManager.

                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Moderator").Result)
            {
                CozyRole role = new CozyRole();
                role.Name = "Moderator";

                IdentityResult roleResult = roleManager.

                CreateAsync(role).Result;
            }

        }
    }
}