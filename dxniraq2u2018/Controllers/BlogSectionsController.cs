using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dxniraq2u2018.Data;
using dxniraq2u2018.Models;
using Microsoft.AspNetCore.Authorization;

namespace dxniraq2u2018.Controllers
{
    [Authorize]
    public class BlogSectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogSectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BlogSections
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogSection.ToListAsync());
        }

        // GET: BlogSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogSection = await _context.BlogSection
                .SingleOrDefaultAsync(m => m.Id == id);
            if (blogSection == null)
            {
                return NotFound();
            }

            return View(blogSection);
        }

        // GET: BlogSections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogSections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] BlogSection blogSection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogSection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogSection);
        }

        // GET: BlogSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogSection = await _context.BlogSection.SingleOrDefaultAsync(m => m.Id == id);
            if (blogSection == null)
            {
                return NotFound();
            }
            return View(blogSection);
        }

        // POST: BlogSections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] BlogSection blogSection)
        {
            if (id != blogSection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogSection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogSectionExists(blogSection.Id))
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
            return View(blogSection);
        }

        // GET: BlogSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogSection = await _context.BlogSection
                .SingleOrDefaultAsync(m => m.Id == id);
            if (blogSection == null)
            {
                return NotFound();
            }

            return View(blogSection);
        }

        // POST: BlogSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogSection = await _context.BlogSection.SingleOrDefaultAsync(m => m.Id == id);
            _context.BlogSection.Remove(blogSection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogSectionExists(int id)
        {
            return _context.BlogSection.Any(e => e.Id == id);
        }
    }
}
