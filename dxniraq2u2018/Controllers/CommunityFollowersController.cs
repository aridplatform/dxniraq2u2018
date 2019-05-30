using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dxniraq2u2018.Data;
using dxniraq2u2018.Models;

namespace dxniraq2u2018.Controllers
{
    public class CommunityFollowersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommunityFollowersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CommunityFollowers
        public async Task<IActionResult> Index(int id)
        {
            var ApplicationDbContext = _context.CommunityFollowers.Where(a => a.CommunityId == id).Include(c => c.ApplicationUser).Include(c => c.Community);
            ViewData["CommunityName"] = _context.Communities.SingleOrDefault(a => a.Id == id).Name;
            return View(await ApplicationDbContext.ToListAsync());
        }


        // GET: CommunityFollowers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communityFollower = await _context.CommunityFollowers
                .Include(c => c.ApplicationUser)
                .Include(c => c.Community)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (communityFollower == null)
            {
                return NotFound();
            }

            return View(communityFollower);
        }

        // GET: CommunityFollowers/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "BgImage");
            return View();
        }

        // POST: CommunityFollowers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CommunityId,ApplicationUserId,NotifyMe,IsAdmin")] CommunityFollower communityFollower)
        {
            if (ModelState.IsValid)
            {
                communityFollower.Id = Guid.NewGuid();
                _context.Add(communityFollower);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", communityFollower.ApplicationUserId);
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "BgImage", communityFollower.CommunityId);
            return View(communityFollower);
        }

        // GET: CommunityFollowers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communityFollower = await _context.CommunityFollowers.SingleOrDefaultAsync(m => m.Id == id);
            if (communityFollower == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", communityFollower.ApplicationUserId);
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "Id", communityFollower.CommunityId);
            return View(communityFollower);
        }

        // POST: CommunityFollowers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CommunityId,ApplicationUserId,NotifyMe,IsAdmin")] CommunityFollower communityFollower)
        {
            if (id != communityFollower.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(communityFollower);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunityFollowerExists(communityFollower.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new {id=communityFollower.CommunityId });
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", communityFollower.ApplicationUserId);
            ViewData["CommunityId"] = new SelectList(_context.Communities, "Id", "BgImage", communityFollower.CommunityId);
            return View(communityFollower);
        }

        // GET: CommunityFollowers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communityFollower = await _context.CommunityFollowers
                .Include(c => c.ApplicationUser)
                .Include(c => c.Community)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (communityFollower == null)
            {
                return NotFound();
            }

            return View(communityFollower);
        }

        // POST: CommunityFollowers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var communityFollower = await _context.CommunityFollowers.SingleOrDefaultAsync(m => m.Id == id);
            _context.CommunityFollowers.Remove(communityFollower);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommunityFollowerExists(Guid id)
        {
            return _context.CommunityFollowers.Any(e => e.Id == id);
        }
    }
}
