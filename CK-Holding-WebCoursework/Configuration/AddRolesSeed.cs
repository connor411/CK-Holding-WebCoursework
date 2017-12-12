using CK_Holding_WebCoursework.Data;
using CK_Holding_WebCoursework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
            await Seed(um, rm);
        }

        private static async Task Seed(UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
        {
            var roles = new String[] { "Employee", "Customer" };
            await AddRoles(rm ,roles);
            string[] UserNames = { "Member1", "Customer1", "Customer2", "Customer3", "Customer4", "Customer5" };
            string[] roleNames = { "Employee", "Customer", "Customer", "Customer", "Customer", "Customer" };
            await AddRolesToUsers(UserNames, roleNames, um, rm);
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
            }
        }

        private static async Task AddRoleToUser(string UserName, string roleName, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
        {
            ApplicationUser user = await um.FindByNameAsync(UserName);
            var ir = await um.AddToRoleAsync(user, roleName);
        }
    }
}
