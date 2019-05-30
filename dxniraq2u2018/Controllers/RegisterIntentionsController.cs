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
    public class RegisterIntentionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public RegisterIntentionsController(ApplicationDbContext context, UserManager<ApplicationUser> userMrg)
        {
            _context = context;
            _userManager = userMrg;
        }

        // GET: RegisterIntentions
        [Authorize(Roles = RoleName.Admins)]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RegisterIntention.Include(r => r.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> Exist()
        {
                      return View();
        }

        public async Task<IActionResult> Confirmed()
        {
            return View();
        }

        // GET: RegisterIntentions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registerIntention = await _context.RegisterIntention
                .Include(r => r.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (registerIntention == null)
            {
                return NotFound();
            }

            return View(registerIntention);
        }

        // GET: RegisterIntentions/Create
        public IActionResult Create()
        {
            var registerIntention = _context.RegisterIntention
                               .SingleOrDefault(m => m.ApplicationUserId == _userManager.GetUserId(User));

            if (registerIntention == null)
            {
                return View();
            }

            return RedirectToAction("Exist");
        }

        // POST: RegisterIntentions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserId")] RegisterIntention registerIntention)
        {
            if (ModelState.IsValid)
            {

                registerIntention.ApplicationUserId = _userManager.GetUserId(User);
                _context.Add(registerIntention);
                await _context.SaveChangesAsync();
                return RedirectToAction("Confirmed");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", registerIntention.ApplicationUserId);
            return View(registerIntention);
        }

        // GET: RegisterIntentions/Edit/5
        [Authorize(Roles = RoleName.Admins)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registerIntention = await _context.RegisterIntention.SingleOrDefaultAsync(m => m.Id == id);
            if (registerIntention == null)
            {
                return NotFound();
            }

            return View(registerIntention);
        }

        // POST: RegisterIntentions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationUserId")] RegisterIntention registerIntention)
        {
            if (id != registerIntention.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    registerIntention.ApplicationUserId = _userManager.GetUserId(User);
                    _context.Update(registerIntention);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegisterIntentionExists(registerIntention.Id))
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
            // ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", registerIntention.ApplicationUserId);
            return View(registerIntention);
        }

        // GET: RegisterIntentions/Delete/5
        [Authorize(Roles = RoleName.Admins)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registerIntention = await _context.RegisterIntention
                .Include(r => r.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (registerIntention == null)
            {
                return NotFound();
            }

            return View(registerIntention);
        }

        // POST: RegisterIntentions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registerIntention = await _context.RegisterIntention.SingleOrDefaultAsync(m => m.Id == id);
            _context.RegisterIntention.Remove(registerIntention);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegisterIntentionExists(int id)
        {
            return _context.RegisterIntention.Any(e => e.Id == id);
        }
    }
}
