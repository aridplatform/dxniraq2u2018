using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dxniraq2u2018.Data;
using dxniraq2u2018.Models;
using Microsoft.AspNetCore.Identity;

namespace dxniraq2u2018.Controllers
{
    public class FaqCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
                private UserManager<ApplicationUser> _userManager;

        public FaqCategoriesController(ApplicationDbContext context,UserManager<ApplicationUser> userMrg)
        {
            _context = context;
                        _userManager = userMrg;
        }

        // GET: FaqCategories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FaqCategory.Include(f => f.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FaqCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faqCategory = await _context.FaqCategory
                .Include(f => f.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (faqCategory == null)
            {
                return NotFound();
            }

            return View(faqCategory);
        }

        // GET: FaqCategories/Create
        public IActionResult Create()
        {
         
            return View();
        }

        // POST: FaqCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ApplicationUserId")] FaqCategory faqCategory)
        {
            if (ModelState.IsValid)
            {
                faqCategory.ApplicationUserId = _userManager.GetUserId(User);
                _context.Add(faqCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
          
            return View(faqCategory);
        }

        // GET: FaqCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faqCategory = await _context.FaqCategory.SingleOrDefaultAsync(m => m.Id == id);
            if (faqCategory == null)
            {
                return NotFound();
            }
          
            return View(faqCategory);
        }

        // POST: FaqCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ApplicationUserId")] FaqCategory faqCategory)
        {
            if (id != faqCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    faqCategory.ApplicationUserId = _userManager.GetUserId(User);
                    _context.Update(faqCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaqCategoryExists(faqCategory.Id))
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
          
            return View(faqCategory);
        }

        // GET: FaqCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faqCategory = await _context.FaqCategory
                .Include(f => f.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (faqCategory == null)
            {
                return NotFound();
            }

            return View(faqCategory);
        }

        // POST: FaqCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faqCategory = await _context.FaqCategory.SingleOrDefaultAsync(m => m.Id == id);
            _context.FaqCategory.Remove(faqCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FaqCategoryExists(int id)
        {
            return _context.FaqCategory.Any(e => e.Id == id);
        }
    }
}
