using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CK_Holding_WebCoursework.Models;

namespace CK_Holding_WebCoursework.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        /// <summary>
        /// A database table to hold annoucements based on the model Annoucemnt
        /// </summary>
        public DbSet<CK_Holding_WebCoursework.Models.Annoucement> Annoucements { get; set; }

        /// <summary>
        /// A database table to hold comments based on the model Comment
        /// </summary>
        public DbSet<CK_Holding_WebCoursework.Models.Comment> Comments { get; set; }
    }
}
