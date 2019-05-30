using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dxniraq2u2018.Data;
using dxniraq2u2018.Models;
using Microsoft.AspNetCore.Http;
using dxniraq2u2018.AuxiliaryClasses;
using Microsoft.AspNetCore.Hosting;

namespace dxniraq2u2018.Controllers
{
    public class BlogAlbumsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;

        public BlogAlbumsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: BlogAlbums
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BlogAlbum.Include(b => b.BlogPost);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BlogAlbums/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogAlbum = await _context.BlogAlbum
                .Include(b => b.BlogPost)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (blogAlbum == null)
            {
                return NotFound();
            }

            return View(blogAlbum);
        }

        // GET: BlogAlbums/Create
        public IActionResult Create(int BlogPostId)
        {
            ViewData["BlogPostId"] = BlogPostId;
            return View();
        }

        // POST: BlogAlbums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BlogPostId,Image,ImageThumb,OrderId")] BlogAlbum blogAlbum, IFormFile myfile)
        {
            if (ModelState.IsValid)
            {
                blogAlbum.Id = Guid.NewGuid();
                blogAlbum.BlogPostId = blogAlbum.BlogPostId;
                blogAlbum.Image = await UserFile.UploadeNewImageAsync(blogAlbum.Image,
              myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 300, 300);

                blogAlbum.ImageThumb = await UserFile.UploadeNewImageAsync(blogAlbum.ImageThumb,
           myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 50, 50);

                _context.Add(blogAlbum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["BlogPostId"] = new SelectList(_context.BlogPosts, "Id", "Body", blogAlbum.BlogPostId);
            ViewData["BlogPostId"] = blogAlbum.BlogPostId;
            return View(blogAlbum);
        }

        // GET: BlogAlbums/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogAlbum = await _context.BlogAlbum.SingleOrDefaultAsync(m => m.Id == id);
            if (blogAlbum == null)
            {
                return NotFound();
            }
            ViewData["BlogPostId"] = new SelectList(_context.BlogPosts, "Id", "Title", blogAlbum.BlogPostId);
            return View(blogAlbum);
        }

        // POST: BlogAlbums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,BlogPostId,Image,ImageThumb,OrderId")] BlogAlbum blogAlbum, IFormFile myfile)
        {
            if (id != blogAlbum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    blogAlbum.Image = await UserFile.UploadeNewImageAsync(blogAlbum.Image,
            myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 300, 300);

                    blogAlbum.ImageThumb = await UserFile.UploadeNewImageAsync(blogAlbum.ImageThumb,
               myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 50, 50);

                    _context.Update(blogAlbum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogAlbumExists(blogAlbum.Id))
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
            //ViewData["BlogPostId"] = new SelectList(_context.BlogPosts, "Id", "Body", blogAlbum.BlogPostId);
            return View(blogAlbum);
        }

        // GET: BlogAlbums/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogAlbum = await _context.BlogAlbum
                .Include(b => b.BlogPost)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (blogAlbum == null)
            {
                return NotFound();
            }

            return View(blogAlbum);
        }

        // POST: BlogAlbums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var blogAlbum = await _context.BlogAlbum.SingleOrDefaultAsync(m => m.Id == id);
            _context.BlogAlbum.Remove(blogAlbum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogAlbumExists(Guid id)
        {
            return _context.BlogAlbum.Any(e => e.Id == id);
        }
    }
}
