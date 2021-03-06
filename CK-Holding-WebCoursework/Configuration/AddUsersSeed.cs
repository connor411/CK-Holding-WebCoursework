﻿using CK_Holding_WebCoursework.Data;
using CK_Holding_WebCoursework.Models;
using CK_Holding_WebCoursework.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CK_Holding_WebCoursework.Configuration
{
    // A seeding class to create users.
    public class AddUsersSeed
    {
        // The main method called to start the process.
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> um)
        {
            context.Database.EnsureCreated();
            if(context.Users.Any())
            {
                return;
            }
            await Seed(um);
        }

        // Creates the array of RegisterViewModels that holds all the user information. 
        private static async Task Seed(UserManager<ApplicationUser> um)
        { 
            var users = new RegisterViewModel[]
                {
                    new RegisterViewModel
                    {
                        Email = "Member1@email.com",
                        UserName = "Member1",
                        Password = "password",
                        ConfirmPassword = "password"
                    },
                     new RegisterViewModel
                    {
                        Email = "Customer1@email.com",
                        UserName = "Customer1",
                        Password = "password",
                        ConfirmPassword = "password"
                    },
                     new RegisterViewModel
                    {
                        Email = "Customer2@email.com",
                        UserName = "Customer2",
                        Password = "password",
                        ConfirmPassword = "password"
                    },
                     new RegisterViewModel
                    {
                        Email = "Customer3@email.com",
                        UserName = "Customer3",
                        Password = "password",
                        ConfirmPassword = "password"
                    },
                     new RegisterViewModel
                    {
                        Email = "Customer4@email.com",
                        UserName = "Customer4",
                        Password = "password",
                        ConfirmPassword = "password"
                    },
                     new RegisterViewModel
                    {
                        Email = "Customer5@email.com",
                        UserName = "Customer5",
                        Password = "password",
                        ConfirmPassword = "password"
                    },
                };
          await AddUsers(users, um);
        }

        // Seed a user to the databse. 
        private static async Task AddUsers(RegisterViewModel[] users, UserManager<ApplicationUser> um)
        {
            foreach (RegisterViewModel vm in users)
            {
                var user = new ApplicationUser { UserName = vm.UserName, Email = vm.Email };
                var AddUser = await um.CreateAsync(user);
                if (AddUser.Succeeded)
                {
                    Console.Write("Added User: " + vm.UserName);
                }
                else
                {
                    Console.Write("User: " + vm.UserName + " was not created");
                }
                var AddPassword = await um.AddPasswordAsync(user, vm.Password);
                if (AddPassword.Succeeded)
                {
                    Console.Write("Added Password for: " + vm.UserName);
                }
                else
                {
                    Console.Write("User: " + vm.UserName + "'s password was not created");
                }
                Console.Write("Added Password for: " + vm.UserName);
            }
        }
    }
}
