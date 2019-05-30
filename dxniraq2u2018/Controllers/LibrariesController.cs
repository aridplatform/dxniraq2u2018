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

namespace dxniraq2u2018.Controllers
{
    public class LibrariesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public LibrariesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: Libraries
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Library.Where(a => a.ApplicationUserId == _userManager.GetUserId(User)).OrderByDescending(a=>a.Id).Include(l => l.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Libraries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _context.Library
                .Include(l => l.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (library == null)
            {
                return NotFound();
            }

            return View(library);
        }

        // GET: Libraries/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Libraries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FileName,FileType,UrlPath,ApplicationUserId,Description,Date")] Library library, IFormFile myfile)
        {
            if (ModelState.IsValid)
            {

                if(library.FileType==0) { 
                library.UrlPath = await UserFile.UploadeNewImageAsync(library.UrlPath,
myfile, _environment.WebRootPath, Properties.Resources.FileFolder, 300, 300);
                }
                else
                {

                    library.UrlPath = await UserFile.UploadeNewFileAsync(library.UrlPath,
                myfile, _environment.WebRootPath, Properties.Resources.FileFolder);
                }
                library.ApplicationUserId = _userManager.GetUserId(User);
                library.Date = DateTime.Now;
                _context.Add(library);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(library);
        }

        // GET: Libraries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _context.Library.SingleOrDefaultAsync(m => m.Id == id);
            if (library == null)
            {
                return NotFound();
            }
          
            return View(library);
        }

        // POST: Libraries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FileName,FileType,UrlPath,ApplicationUserId,Description,Date")] Library library, IFormFile myfile)
        {
            if (id != library.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    library.UrlPath = await UserFile.UploadeNewImageAsync(library.UrlPath,
myfile, _environment.WebRootPath, Properties.Resources.FileFolder, 300, 300);
                    library.ApplicationUserId = _userManager.GetUserId(User);
                
                    _context.Update(library);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryExists(library.Id))
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
           
            return View(library);
        }

        // GET: Libraries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _context.Library
                .Include(l => l.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (library == null)
            {
                return NotFound();
            }

            return View(library);
        }

        // POST: Libraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var library = await _context.Library.SingleOrDefaultAsync(m => m.Id == id);
            _context.Library.Remove(library);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryExists(int id)
        {
            return _context.Library.Any(e => e.Id == id);
        }
    }
}
