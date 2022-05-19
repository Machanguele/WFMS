using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    public class DataContextSeed
    {
        public DataContextSeed()
        {
        }
       
        public static async Task SeedAsync(DataContext context, ILoggerFactory loggerFactory, 
            UserManager<AppUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                if (!context.Departments.Any())
                {
                    var departmentsData = File.ReadAllText("../Persistence/SeedData/departmentsSeed.json");
                    var departments = JsonSerializer.Deserialize<List<Department>>(departmentsData);
                    foreach (var department in departments)
                    {
                        await context.AddAsync(department);
                    }

                    await context.SaveChangesAsync();
                }
                if (!context.ApplicationPermissions.Any())
                {
                    var permissionData = File.ReadAllText("../Persistence/SeedData/permissionsSeed.json");
                    var permissions = JsonSerializer.Deserialize<List<ApplicationPermission>>(permissionData);
                    foreach (var permission in permissions)
                    {
                        await context.AddAsync(permission);
                    }

                    await context.SaveChangesAsync();
                }
                
                if (!context.ApplicationRoles.Any())
                {
                    var roleData = File.ReadAllText("../Persistence/SeedData/rolesSeed.json");
                    var roles = JsonSerializer.Deserialize<List<ApplicationRole>>(roleData);
                    foreach (var role in roles)
                    {
                        await context.AddAsync(role);
                        if (role.Name == "SystemAdministrator")
                        {
                            var permissions = await context.ApplicationPermissions.ToListAsync();
                            foreach (var permission in permissions)
                            {
                                var rolePermission = new ApplicationRolePermission()
                                {
                                    ApplicationPermission = permission,
                                    ApplicationRole = role
                                };
                                await context.AddAsync(rolePermission);
                            }
                        }
                    }
                    await context.SaveChangesAsync();
                }
                
                if (!userManager.Users.Any())
                {
                    var userData = File.ReadAllText("../Persistence/SeedData/usersSeed.json");
                    var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
                    foreach (var user in users)
                    {
                        
                        await userManager.CreateAsync(new AppUser
                        {
                            EmailConfirmed = true,
                            Email = user.Email,
                            FullName = user.FullName,
                            UserName = user.Email,
                            ApplicationRole = await context.ApplicationRoles.FirstOrDefaultAsync(x=> x.Name=="SystemAdministrator" )
                        }, "Pa$$w0rd");
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<DataContext>();
                logger.LogError(ex, ex.Message);
            }
        }
    }
}