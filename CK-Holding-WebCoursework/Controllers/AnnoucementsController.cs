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
    /// <summary>
    /// Holds all the logic for annoucements
    /// </summary>
    public class AnnoucementsController : Controller
    {
        //The web application database
        private readonly ApplicationDbContext _context;
        //The user manager used to create, edit and delete users
        private readonly UserManager<ApplicationUser> _um;

        //The hosting enviroment used for storing images
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// The constructor of the class to assign the class variables
        /// </summary>
        /// <param name="context">The database of the web application</param>
        /// <param name="um">The user manager used to create, edit and delete users</param>
        /// <param name="hostingEnvironment">The hosting enviroement used to store images</param>
        public AnnoucementsController(ApplicationDbContext context, UserManager<ApplicationUser> um, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _um = um;
        }

        // GET: Annoucements
        // The index page to list all annoucements
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Annoucements.ToListAsync());
        }



        // GET: Annoucements/Details/5
        //The get the details view model of an annoucement based on id
        [AllowAnonymous]
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

        // Add 1 to the view counter of an annoucement. 
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

        /// <summary>
        /// A Details method to display an annoucment and a list of comments from different users. 
        /// </summary>
        /// <param name="viewModel">A viewModel with a new comment.</param>
        /// <returns>The updated viewModel</returns>
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

        /// <summary>
        /// Get the AnnoucmenetDetailsViewModel from an annoucement in the database.
        /// </summary>
        /// <param name="annoucement">The annoucement used to find the viewModel</param>
        /// <returns>viewModel of the annoucement</returns>
        private async Task<AnnoucementDetailsViewModel> GetAnnoucementDetailsViewModelFromAnnoucement(Annoucement annoucement)
        {
            AnnoucementDetailsViewModel viewModel = new AnnoucementDetailsViewModel();
            viewModel.Annoucement = annoucement;

            List<Comment> comments = await _context.Comments
                .Where(x => x.MyAnnoucement == annoucement).ToListAsync();
            viewModel.Comments = comments;
            return viewModel;
        }

        /// <summary>
        /// Gets the current user who is signed in
        /// </summary>
        /// <returns>A application user who is signed in</returns>
        private ApplicationUser GetCurrentUser()
        {
            var userId = _um.GetUserId(HttpContext.User);
            ApplicationUser user = _context.Users.FirstOrDefault(x => x.Id == userId);
            return user;
           
        }

        // GET: Annoucements/Create
        // returns the create view for creating annoucements
        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Annoucements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // This method is created if a new annoucement has been created. 
        // The date and time, the current user and a new counter is added to the annoucement
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
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
        // This method is called when a user clicks edit on an annoucement.
        // This will return the edit view for an annoucement
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
        // If the user has made a change in the edit view this method is called
        // The method updates the database with the new data
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
        // Displays the delete view for an annoucement based on id.
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
        // This method deletes the annoucement and the comments for that annoucement.
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
