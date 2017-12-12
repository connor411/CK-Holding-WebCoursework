using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CK_Holding_WebCoursework.Data;
using CK_Holding_WebCoursework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CK_Holding_WebCoursework.Controllers
{
    
    public class AnnoucementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _um;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AnnoucementsController(ApplicationDbContext context, UserManager<ApplicationUser> um, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _um = um;
        }

        // GET: Annoucements
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Annoucements.ToListAsync());
        }



        // GET: Annoucements/Details/5
        //[Authorize(Roles = "Employee, Customer")]
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            AddCounter(id);

            Annoucement annoucement = await _context.Annoucements
                .SingleOrDefaultAsync(m => m.Id == id);
            if (annoucement == null)
            {
                return NotFound();
            }

            AnnoucementDetailsViewModel viewModel = new AnnoucementDetailsViewModel();



            viewModel.Annoucement = annoucement;
            
            List<Comment> comments = await _context.Comments
                .Where(x => x.MyAnnoucement == annoucement).ToListAsync();

            viewModel.Comments = comments;


            return View(viewModel);
        }


        [ValidateAntiForgeryToken]
        private void AddCounter(int? id)
        {
            var annoucement =  _context.Annoucements.SingleOrDefaultAsync(m => m.Id == id);
            if(annoucement != null)
            {
                annoucement.Result.counter++;
                
                _context.SaveChanges();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee, Customer")]
        public async Task<IActionResult> Details([Bind("AnnoucementID, Description")]
           AnnoucementDetailsViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                Comment comment = new Comment();

                comment.Description = viewModel.Description;
                comment.User = GetCurrentUser();
                comment.UserName = comment.User.UserName;

                Annoucement annoucement = await _context.Annoucements.SingleOrDefaultAsync(m => m.Id == viewModel.AnnoucementID);

                if(annoucement == null)
                {
                    return NotFound();
                }

                comment.MyAnnoucement = annoucement;
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                viewModel = await GetAnnoucementDetailsViewModelFromAnnoucement(annoucement);
            }

            return View(viewModel);
        }

        private async Task<AnnoucementDetailsViewModel> GetAnnoucementDetailsViewModelFromAnnoucement(Annoucement annoucement)
        {
            AnnoucementDetailsViewModel viewModel = new AnnoucementDetailsViewModel();
            viewModel.Annoucement = annoucement;

            List<Comment> comments = await _context.Comments
                .Where(x => x.MyAnnoucement == annoucement).ToListAsync();
            viewModel.Comments = comments;
            return viewModel;
        }

        private ApplicationUser GetCurrentUser()
        {
            var userId = _um.GetUserId(HttpContext.User);
            ApplicationUser user = _context.Users.FirstOrDefault(x => x.Id == userId);
            return user;
           
        }

        // GET: Annoucements/Create
        [Authorize(Roles = "Employee")]
        //[Authorize(Policy = "CanCreatePost")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Annoucements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ImageLocation")] Annoucement annoucement, IFormFile pic)
        {
            if (ModelState.IsValid)
            {
                annoucement.DateAndTimeOfPost = DateTime.Now;
                annoucement.User = GetCurrentUser();
                annoucement.UserName = annoucement.User.UserName;
                annoucement.counter = 0;

                if (pic != null)
                {
                    var fileName = Path.Combine(_hostingEnvironment.WebRootPath, Path.GetFileName(pic.FileName));

                    if (!System.IO.File.Exists(fileName))
                    {
                        pic.CopyTo(new FileStream(fileName, FileMode.Create));
                    }

                    
                    annoucement.ImageLocation = fileName;
                    annoucement.ImageName = "/" + Path.GetFileName(fileName);
                }

                _context.Add(annoucement);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(annoucement);
        }

        // GET: Annoucements/Edit/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annoucement = await _context.Annoucements.SingleOrDefaultAsync(m => m.Id == id);
            if (annoucement == null)
            {
                return NotFound();
            }
            return View(annoucement);
        }

        // POST: Annoucements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] Annoucement annoucement)
        {
            if (id != annoucement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    annoucement.DateAndTimeOfPost = DateTime.Now;
                    _context.Update(annoucement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnoucementExists(annoucement.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(annoucement);
        }

        // GET: Annoucements/Delete/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annoucement = await _context.Annoucements
                .SingleOrDefaultAsync(m => m.Id == id);

            var comments =  _context.Comments.Where(x => x.MyAnnoucement.Id == id);

            if (annoucement == null)
            {
                return NotFound();
            }
            else if (comments == null)
            {
                return NotFound();
            }

            return View(annoucement);
        }

        // POST: Annoucements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var annoucement = await _context.Annoucements.SingleOrDefaultAsync(m => m.Id == id);
            _context.Annoucements.Remove(annoucement);
            var comments = _context.Comments.Where(x => x.MyAnnoucement.Id == id);
            foreach (var elem in comments)
            {
                _context.Comments.Remove(elem);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AnnoucementExists(int id)
        {
            return _context.Annoucements.Any(e => e.Id == id);
        }
    }
}
