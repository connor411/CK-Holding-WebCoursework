using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CK_Holding_WebCoursework.Data;
using Microsoft.AspNetCore.Identity;
using CK_Holding_WebCoursework.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CK_Holding_WebCoursework.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _um;
        
        public MembersController(ApplicationDbContext context, UserManager<ApplicationUser> um)
        {
            _context = context;
            _um = um;
        }

        private ApplicationUser GetCurrentUser()
        {
            var userId = _um.GetUserId(HttpContext.User);
            ApplicationUser user = _context.Users.FirstOrDefault(x => x.Id == userId);
            return user;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            string UserName = GetCurrentUser().UserName;
            ViewData["Message"] = "Welcome Member User " + UserName;
            return View();
        }
    }
}
