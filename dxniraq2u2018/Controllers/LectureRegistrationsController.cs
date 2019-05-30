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
using Microsoft.AspNetCore.Authorization;

namespace dxniraq2u2018.Controllers
{
    [Authorize]
    public class LectureRegistrationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LectureRegistrationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: LectureRegistrations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LectureRegistrations.Include(l => l.Lecture);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LectureRegistrations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lectureRegistration = await _context.LectureRegistrations
                .Include(l => l.Lecture)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (lectureRegistration == null)
            {
                return NotFound();
            }

            return View(lectureRegistration);
        }

        // GET: LectureRegistrations/Create
        public IActionResult Create()
        {
            ViewData["LectureId"] = new SelectList(_context.Lectures, "Id", "Id");
            return View();
        }

        // POST: LectureRegistrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LectureId,UserId,Date")] LectureRegistration lectureRegistration)
        {
            if (ModelState.IsValid)
            {
                lectureRegistration.UserId = _userManager.GetUserId(User);
                _context.Add(lectureRegistration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LectureId"] = new SelectList(_context.Lectures, "Id", "Id", lectureRegistration.LectureId);
            return View(lectureRegistration);
        }

        // GET: LectureRegistrations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lectureRegistration = await _context.LectureRegistrations.SingleOrDefaultAsync(m => m.Id == id);
            if (lectureRegistration == null)
            {
                return NotFound();
            }
            ViewData["LectureId"] = new SelectList(_context.Lectures, "Id", "Id", lectureRegistration.LectureId);
            return View(lectureRegistration);
        }

        // POST: LectureRegistrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,LectureId,UserId,Date")] LectureRegistration lectureRegistration)
        {
            if (id != lectureRegistration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lectureRegistration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LectureRegistrationExists(lectureRegistration.Id))
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
            ViewData["LectureId"] = new SelectList(_context.Lectures, "Id", "Id", lectureRegistration.LectureId);
            return View(lectureRegistration);
        }

        // GET: LectureRegistrations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lectureRegistration = await _context.LectureRegistrations
                .Include(l => l.Lecture)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (lectureRegistration == null)
            {
                return NotFound();
            }

            return View(lectureRegistration);
        }

        // POST: LectureRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var lectureRegistration = await _context.LectureRegistrations.SingleOrDefaultAsync(m => m.Id == id);
            _context.LectureRegistrations.Remove(lectureRegistration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LectureRegistrationExists(Guid id)
        {
            return _context.LectureRegistrations.Any(e => e.Id == id);
        }
    }
}
