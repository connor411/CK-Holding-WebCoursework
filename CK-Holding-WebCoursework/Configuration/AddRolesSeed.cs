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
    public class AddRolesSeed
    {
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

        private static async Task Seed(UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm, ApplicationDbContext context)
        {
            var roles = new String[] { "Employee", "Customer" };
            await AddRoles(rm ,roles);
            string[] UserNames = { "Member1", "Customer1", "Customer2", "Customer3", "Customer4", "Customer5" };
            string[] roleNames = { "Employee", "Customer", "Customer", "Customer", "Customer", "Customer" };
            await AddRolesToUsers(UserNames, roleNames, um, rm);
        }

        private static async Task AddClaims(ApplicationDbContext context, RoleManager<IdentityRole> rm)
        {
                IdentityRole Employee = context.Roles.FirstOrDefault(x => x.Name == "Employee");
                await rm.AddClaimAsync(Employee, new Claim("CanEditPost", "CanEditPost"));
                await rm.AddClaimAsync(Employee, new Claim("CanCreatePost", "CanCreatePost"));
                await rm.AddClaimAsync(Employee, new Claim("CanDeletePost", "CanDeletePost"));
        }

        private static async Task AddRolesToUsers(string[] UserName, string[] roleNames, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
        {
            for(int i = 0; i< UserName.Length; i++)
            {
                await AddRoleToUser(UserName[i], roleNames[i], um, rm);
             
            }
        }

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
