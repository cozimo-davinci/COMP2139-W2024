using COMP2139_Lab1.Areas.ProjectManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace COMP2139_Lab1.Data
{
    public class ContextSeed
    {

        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Basic.ToString()));

        }

        public static async Task SuperSeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var superUser = new ApplicationUser
            {
                UserName = "superAdmin",
                Email = "adminsupport@domain.com",
                FirstName = "Matrix",
                LastName = "Architect",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            if (userManager.Users.All(u => u.Id != superUser.Id))
            {
                var user = await userManager.FindByEmailAsync(superUser.Email);
                if (user == null)
                {
                    var result =  await userManager.CreateAsync(superUser, "superAdmin228%");

                    if(result.Succeeded)
                    {

                        // Log Message
                        Console.WriteLine("SuperAdmin user created successfully");

                        await userManager.AddToRoleAsync(superUser, Enum.Roles.SuperAdmin.ToString());
                        await userManager.AddToRoleAsync(superUser, Enum.Roles.Admin.ToString());
                        await userManager.AddToRoleAsync(superUser, Enum.Roles.Moderator.ToString());
                        await userManager.AddToRoleAsync(superUser, Enum.Roles.Basic.ToString());

                    }
                    else
                    {
                        // Log errors if user creation fails
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error creating SuperAdmin user: {error.Description}");
                        }
                    }
                }
                else
                {
                    // Log that the user already exists
                    Console.WriteLine("SuperAdmin user already exists.");
                }

            }
            }
        }
    }

