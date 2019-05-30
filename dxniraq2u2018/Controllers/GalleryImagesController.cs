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
using Microsoft.AspNetCore.Http;
using dxniraq2u2018.AuxiliaryClasses;

namespace dxniraq2u2018.Controllers
{
    public class GalleryImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;

        public GalleryImagesController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: GalleryImages
        public async Task<IActionResult> Index(int id)
        {
            var applicationDbContext = _context.GalleryImage.Where(a => a.GalleryId == id).Include(g => g.Gallery);
            ViewData["Albumname"] = _context.Gallery.SingleOrDefault(a => a.Id == id).Subject;
            ViewData["Description"] = _context.Gallery.SingleOrDefault(a => a.Id == id).Description;
            ViewData["id"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Display(int id)
        {
            try
            {
                ViewData["Albumname"] = _context.Gallery.SingleOrDefault(a => a.Id == id).Subject;
                var applicationDbContext = _context.GalleryImage.Where(a => a.GalleryId == id).Include(g => g.Gallery);
                ViewData["PhImage"] = "../../images/" + _context.GalleryImage.FirstOrDefault(a => a.GalleryId == id)._thumb;
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception)
            {
                return View();
            }
        }

        // GET: GalleryImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleryImage = await _context.GalleryImage
                .Include(g => g.Gallery)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (galleryImage == null)
            {
                return NotFound();
            }

            return View(galleryImage);
        }

        // GET: GalleryImages/Create
        public IActionResult Create(int gid)
        {
            ViewData["GalleryId"] = new SelectList(_context.Gallery.Where(a => a.Id == gid), "Id", "Subject");
            return View();
        }

        // POST: GalleryImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Subject,Url,_thumb,GalleryId")] GalleryImage galleryImage, IFormFile myfile)
        {
            if (ModelState.IsValid)
            {

                //                galleryImage.Url = await UserFile.UploadeNewImageAsync(galleryImage.Url,
                //myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 500, 500);

                galleryImage._thumb = await UserFile.UploadeNewImageAsync(galleryImage.Url,
myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 500, 500);

                _context.Add(galleryImage);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "GalleryImages", new { id = galleryImage.GalleryId });
                //  return RedirectToAction(nameof(Index));
            }
            ViewData["GalleryId"] = new SelectList(_context.Gallery, "Id", "Subject", galleryImage.GalleryId);
            return View(galleryImage);
        }

        // GET: GalleryImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleryImage = await _context.GalleryImage.SingleOrDefaultAsync(m => m.Id == id);
            if (galleryImage == null)
            {
                return NotFound();
            }
            ViewData["GalleryId"] = new SelectList(_context.Gallery, "Id", "Subject", galleryImage.GalleryId);
            return View(galleryImage);
        }

        // POST: GalleryImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Url,_thumb,GalleryId")] GalleryImage galleryImage, IFormFile myfile)
        {
            if (id != galleryImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    galleryImage.Url = await UserFile.UploadeNewImageAsync(galleryImage.Url,
myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 500, 500);

                    galleryImage._thumb = await UserFile.UploadeNewImageAsync(galleryImage._thumb,
    myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 100, 100);
                    _context.Update(galleryImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalleryImageExists(galleryImage.Id))
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
            ViewData["GalleryId"] = new SelectList(_context.Gallery, "Id", "Subject", galleryImage.GalleryId);
            return View(galleryImage);
        }

        // GET: GalleryImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleryImage = await _context.GalleryImage
                .Include(g => g.Gallery)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (galleryImage == null)
            {
                return NotFound();
            }

            return View(galleryImage);
        }

        // POST: GalleryImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var galleryImage = await _context.GalleryImage.SingleOrDefaultAsync(m => m.Id == id);
            _context.GalleryImage.Remove(galleryImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GalleryImageExists(int id)
        {
            return _context.GalleryImage.Any(e => e.Id == id);
        }
    }
}
