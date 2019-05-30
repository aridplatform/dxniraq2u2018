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
    public class FaqsController : Controller
    {
        private readonly ApplicationDbContext _context;
                private UserManager<ApplicationUser> _userManager;
        public FaqsController(ApplicationDbContext context,UserManager<ApplicationUser> userMrg)
        {
            _context = context;
                        _userManager = userMrg;
        }

        // GET: Faqs
        public async Task<IActionResult> Index(int id)
        {
            ViewData["FaqCategoryName"] = _context.FaqCategory.SingleOrDefault(a => a.Id == id).Name;
            ViewData["FaqUserName"] = _context.FaqCategory.Include(a=>a.ApplicationUser).SingleOrDefault(a => a.Id == id).ApplicationUser.ArName;
            var applicationDbContext = _context.Faq.Where(a => a.FaqCategoryId == id).OrderBy(a => a.Order).Include(f => f.FaqCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> IndexAdmin(int id)
        {
            var applicationDbContext = _context.Faq.Where(a => a.FaqCategoryId == id).OrderBy(a => a.Order).Include(f => f.FaqCategory);
            ViewData["fid"] = id;
            ViewData["FaqCategoryName"] = _context.FaqCategory.SingleOrDefault(a => a.Id == id).Name;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Faqs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.Faq
                .Include(f => f.FaqCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        // GET: Faqs/Create
        public IActionResult Create(int fid)
        {
            ViewData["FaqCategoryId"] = new SelectList(_context.FaqCategory.Where(a => a.Id==fid), "Id", "Name");

            try
            {
                ViewData["OrderSequence"] = (_context.Faq.Where(a => a.FaqCategoryId == fid).LastOrDefault().Order) + 1;
            }
            catch (Exception)
            {

                ViewData["OrderSequence"] = 1;
            }

            return View();
        }

        // POST: Faqs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FaqCategoryId,Order,Question,Answer,Meta")] Faq faq)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faq);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexAdmin", "Faqs", new { id = faq.FaqCategoryId });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["FaqCategoryId"] = new SelectList(_context.FaqCategory.Where(a => a.ApplicationUserId == _userManager.GetUserId(User)), "Id", "Name", faq.FaqCategoryId);
            return View(faq);
        }

        // GET: Faqs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.Faq.SingleOrDefaultAsync(m => m.Id == id);
            if (faq == null)
            {
                return NotFound();
            }
            ViewData["FaqCategoryId"] = new SelectList(_context.FaqCategory.Where(a => a.ApplicationUserId == _userManager.GetUserId(User)), "Id", "Name", faq.FaqCategoryId);
            return View(faq);
        }

        // POST: Faqs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FaqCategoryId,Order,Question,Answer,Meta")] Faq faq)
        {
            if (id != faq.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faq);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaqExists(faq.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Faqs", new {id= faq.FaqCategoryId });
            }
            ViewData["FaqCategoryId"] = new SelectList(_context.FaqCategory, "Id", "Id", faq.FaqCategoryId);
            return View(faq);
        }

        // GET: Faqs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.Faq
                .Include(f => f.FaqCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        // POST: Faqs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faq = await _context.Faq.SingleOrDefaultAsync(m => m.Id == id);
            _context.Faq.Remove(faq);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Faqs", new { id = faq.FaqCategoryId });
        }

        private bool FaqExists(int id)
        {
            return _context.Faq.Any(e => e.Id == id);
        }
    }
}
