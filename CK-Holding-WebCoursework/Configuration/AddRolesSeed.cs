using CK_Holding_WebCoursework.Data;
using CK_Holding_WebCoursework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CK_Holding_WebCoursework.Configuration
{
    /// <summary>
    /// A seeding class to create roles and apply them to users Claims are also created.
    /// </summary>
    public class AddRolesSeed
    {
        /// <summary>
        /// The starting method that is called to start the process of creating roles and applying them.
        /// If there are roles or claims in the data then the program will not start the seeding.
        /// </summary>
        /// <param name="context">The database for the web application</param>
        /// <param name="um">User manager, used to create, delete, and edit users within the database</param>
        /// <param name="rm">Roles manager, used to create, delete, and edit roles within the database</param>
        /// <returns>If the database has been seeded then do not seed.</returns>
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
        {
            context.Database.EnsureCreated();
            if (context.Roles.Any())
            {
                return;
            }
            await Seed(um, rm, context);
            if (context.RoleClaims.Any())
            {
                return;
            }
            await AddClaims(context, rm);
        }

        /// <summary>
        /// A helper function to create the dta in arrays.
        /// </summary>
        /// <param name="um">User manager, used to create, delete, and edit users within the database</param>
        /// <param name="rm">Roles manager, used to create, delete, and edit roles within the database</param>
        /// <param name="context">The database for the web application</param>
        /// <returns></returns>
        private static async Task Seed(UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm, ApplicationDbContext context)
        {
            var roles = new String[] { "Employee", "Customer" };
            await AddRoles(rm ,roles);
            string[] UserNames = { "Member1", "Customer1", "Customer2", "Customer3", "Customer4", "Customer5" };
            string[] roleNames = { "Employee", "Customer", "Customer", "Customer", "Customer", "Customer" };
            await AddRolesToUsers(UserNames, roleNames, um, rm);
        }

        /// <summary>
        /// Add claims to the database.
        /// </summary>
        /// <param name="context">The database for the web application</param>
        /// <param name="rm">Roles manager, used to create, delete, and edit roles within the database/param>
        /// <returns></returns>
        private static async Task AddClaims(ApplicationDbContext context, RoleManager<IdentityRole> rm)
        {
                IdentityRole Employee = context.Roles.FirstOrDefault(x => x.Name == "Employee");
                await rm.AddClaimAsync(Employee, new Claim("CanEditPost", "CanEditPost"));
                await rm.AddClaimAsync(Employee, new Claim("CanCreatePost", "CanCreatePost"));
                await rm.AddClaimAsync(Employee, new Claim("CanDeletePost", "CanDeletePost"));
        }

        /// <summary>
        /// A method to loop through the arrays calling the method AddRoleToUser. 
        /// </summary>
        /// <param name="UserName">Array of Usernames which a role will be applyed to.</param>
        /// <param name="roleNames">Array of roles for which a user will be applyed to</param>
        /// <param name="um">User manager, used to create, delete, and edit users within the database</param>
        /// <param name="rm">Roles manager, used to create, delete, and edit roles within the database</param>
        /// <returns></returns>
        private static async Task AddRolesToUsers(string[] UserName, string[] roleNames, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
        {
            for(int i = 0; i< UserName.Length; i++)
            {
                await AddRoleToUser(UserName[i], roleNames[i], um, rm);
             
            }
        }

        /// <summary>
        /// Seeds roles to the database.
        /// </summary>
        /// <param name="rm">Roles manager, used to create, delete, and edit roles within the database</param>
        /// <param name="roles">Array of roles for which a user will be applyed to</param>
        /// <returns></returns>
        private static async Task AddRoles(RoleManager<IdentityRole> rm, string[] roles)
        {
            foreach(string role in roles)
            {
                var AddRole = await rm.CreateAsync(new IdentityRole(role));
                if (AddRole.Succeeded)
                {
                    Console.Write("Added Role: " + role);
                }
                else
                {
                    Console.Write("Role: " + role + " was not created");
                }
               
            }
        }

        /// <summary>
        /// A method to add a role to a user.
        /// </summary>
        /// <param name="UserName">A username used to find the user in a database. This user is then given a role</param>
        /// <param name="roleName">A role to be applyed to a user</param>
        /// <param name="um">User manager, used to create, delete, and edit users within the database</param>
        /// <param name="rm">Roles manager, used to create, delete, and edit roles within the database</param>
        /// <returns></returns>
        private static async Task AddRoleToUser(string UserName, string roleName, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
        {
            ApplicationUser user = await um.FindByNameAsync(UserName);
            var userRole = await um.AddToRoleAsync(user, roleName);
            if (userRole.Succeeded)
            {
                Console.Write("Added role: " + roleName + " To User: " + UserName);
            }
            else
            {
                Console.Write("Role: " + roleName + " was not created");
            }
        }
    }
}
