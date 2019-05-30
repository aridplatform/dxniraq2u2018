using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dxniraq2u2018.Data;
using dxniraq2u2018.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using dxniraq2u2018.AuxiliaryClasses;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace dxniraq2u2018.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogPostsController(ApplicationDbContext context, IHostingEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        // GET: BlogPosts
        [Authorize(Roles = RoleName.Admins)]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BlogPosts.Include(b => b.AdminUser).Include(b => b.ApplicationUser).Include(b => b.BlogCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BlogPosts
        public async Task<IActionResult> PostsLists(int? id)
        {


            var applicationDbContext = _context.BlogPosts.Where(a => a.BlogCategoryId == id)
.Include(b => b.AdminUser)
.Include(b => b.ApplicationUser)
.Include(b => b.BlogCategory);

            return View(await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> SectionLists(int? id)
        {


            var applicationDbContext = _context.BlogPosts.Where(a => a.BlogCategory.BlogSectionId == id)
.Include(b => b.AdminUser)
.Include(b => b.ApplicationUser)
.Include(b => b.BlogCategory);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BlogPosts
        public async Task<IActionResult> Congratulations()
        {
            var applicationDbContext = _context.BlogPosts.Where(a => a.BlogCategoryId == 1005)
                .Include(b => b.AdminUser)
                .Include(b => b.ApplicationUser)
                .Include(b => b.BlogCategory);
            return View(await applicationDbContext.ToListAsync());
        }



        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id, string t)
        {
            if (id == null)
            {
                return NotFound();
            }

            var BlogPostViewModel = new BlogPostViewModel
            {
                BlogPost = await _context.BlogPosts
                .Include(b => b.AdminUser)
                .Include(b => b.ApplicationUser)
                .Include(b => b.BlogCategory)
                .SingleOrDefaultAsync(m => m.Id == id),
                                BlogAlbum = _context.BlogAlbum.Where(m => m.BlogPostId == id)
            };
            ViewData["Title"] = BlogPostViewModel.BlogPost.Title;

            if (string.IsNullOrWhiteSpace(t))
            {
                t = _context.BlogPosts.First(p => p.Id == id).Title;
                return RedirectToAction("Details", new { id = id, t = t.Replace(" ", "-") });
            }

            if (BlogPostViewModel.BlogPost == null)
            {
                return NotFound();
            }
            return View(BlogPostViewModel);
        }


        // GET: BlogPosts/Create
        [Authorize]
        public IActionResult CreateFromSection(int SectionId)
        {

            ViewData["BlogCategoryId"] = new SelectList(_context.BlogCategories.Where(c => c.BlogSectionId == SectionId), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFromSection([Bind("Id,BlogCategoryId,Introduction,Title,Body,Image,IsVisible,IsFeatured,Date,ApplicationUserId,AdminUserId,Imagethumb,Meta,Filelink")] BlogPost blogPost, IFormFile myfile, IFormFile Filelink)
        {
            if (ModelState.IsValid)
            {
                blogPost.ApplicationUserId = _userManager.GetUserId(User);
                blogPost.AdminUserId = _userManager.GetUserId(User);
                blogPost.Image = await UserFile.UploadeNewImageAsync(blogPost.Image,
              myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 300, 300);
                blogPost.Imagethumb = await UserFile.UploadeNewImageAsync(blogPost.Image,
    myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 50, 50);

                blogPost.Filelink = await UserFile.UploadeNewFileAsync(blogPost.Filelink,
    Filelink, _environment.WebRootPath, Properties.Resources.FileFolder);

                blogPost.Date = DateTime.Today.Date;
                blogPost.IsVisible = true;

                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AdminUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", blogPost.AdminUserId);
            //ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", blogPost.ApplicationUserId);
            ViewData["BlogCategoryId"] = new SelectList(_context.BlogCategories, "Id", "Name", blogPost.BlogCategoryId);
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        [Authorize]
        public IActionResult NewCong(int SectionId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewCong([Bind("Id,BlogCategoryId,Introduction,Title,Body,Image,IsVisible,IsFeatured,Date,ApplicationUserId,AdminUserId,Imagethumb,Meta,Filelink")] BlogPost blogPost, IFormFile myfile)
        {
            if (ModelState.IsValid)
            {
                blogPost.ApplicationUserId = _userManager.GetUserId(User);
                blogPost.AdminUserId = _userManager.GetUserId(User);
                blogPost.Image = await UserFile.UploadeNewImageAsync(blogPost.Image,
              myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 300, 300);
                blogPost.Imagethumb = await UserFile.UploadeNewImageAsync(blogPost.Image,
    myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 50, 50);

                blogPost.Date = DateTime.Today.Date;
                blogPost.IsVisible = true;
                blogPost.BlogCategoryId = 1005;

                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Congratulations));
            }
            //ViewData["AdminUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", blogPost.AdminUserId);
            //ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", blogPost.ApplicationUserId);

            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public IActionResult Create(int sid)
        {

            ViewData["BlogCategoryId"] = new SelectList(_context.BlogCategories.Where(c => c.BlogSectionId == sid), "Id", "Name");
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BlogCategoryId,Introduction,Title,Body,Image,IsVisible,IsFeatured,Date,ApplicationUserId,AdminUserId,Imagethumb,Meta,Filelink")] BlogPost blogPost, IFormFile myfile, IFormFile filelink)
        {
            if (ModelState.IsValid)
            {
                blogPost.ApplicationUserId = _userManager.GetUserId(User);
                blogPost.AdminUserId = _userManager.GetUserId(User);
                blogPost.Image = await UserFile.UploadeNewImageAsync(blogPost.Image,
              myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 300, 300);

                blogPost.Imagethumb = await UserFile.UploadeNewImageAsync(blogPost.Imagethumb,
    myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 50, 50);

                //                blogPost.Filelink = await UserFile.UploadeNewFileAsync(blogPost.Filelink,
                //filelink, _environment.WebRootPath, Properties.Resources.FileFolder);

                blogPost.Filelink = await UserFile.UploadeNewFileAsync(blogPost.Filelink, filelink, _environment.WebRootPath, Properties.Resources.FileFolder);

                blogPost.Date = DateTime.Today.Date;
                blogPost.IsVisible = true;

                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AdminUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", blogPost.AdminUserId);
            //ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", blogPost.ApplicationUserId);
            ViewData["BlogCategoryId"] = new SelectList(_context.BlogCategories, "Id", "Name", blogPost.BlogCategoryId);
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.SingleOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }
            //ViewData["AdminUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", blogPost.AdminUserId);
            //ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", blogPost.ApplicationUserId);
            ViewData["BlogCategoryId"] = new SelectList(_context.BlogCategories, "Id", "Name", blogPost.BlogCategoryId);
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogCategoryId,Introduction,Title,Body,Image,IsVisible,IsFeatured,Date,ApplicationUserId,AdminUserId,Imagethumb,Meta,Filelink")] BlogPost blogPost, IFormFile myfile, IFormFile filelink)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    blogPost.ApplicationUserId = _userManager.GetUserId(User);
                    blogPost.AdminUserId = _userManager.GetUserId(User);


                    blogPost.Image = await UserFile.UploadeNewImageAsync(blogPost.Image,
          myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 300, 300);

                    blogPost.Imagethumb = await UserFile.UploadeNewImageAsync(blogPost.Imagethumb,
    myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 50, 50);



                    blogPost.Filelink = await UserFile.UploadeNewFileAsync(blogPost.Filelink, filelink, _environment.WebRootPath, Properties.Resources.FileFolder);

                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AdminUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", blogPost.AdminUserId);
            //ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", blogPost.ApplicationUserId);
            ViewData["BlogCategoryId"] = new SelectList(_context.BlogCategories, "Id", "Name", blogPost.BlogCategoryId);
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.AdminUser)
                .Include(b => b.ApplicationUser)
                .Include(b => b.BlogCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.SingleOrDefaultAsync(m => m.Id == id);
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }
    }
}
