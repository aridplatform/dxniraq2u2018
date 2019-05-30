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
    public class PostMetricsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public PostMetricsController(ApplicationDbContext context, UserManager<ApplicationUser> userMrg)
        {
            _context = context;
            _userManager = userMrg;
        }

        // GET: PostMetrics
        public async Task<IActionResult> Index()
        {
            var ApplicationDbContext = _context.PostMetrics.Include(p => p.ApplicationUser).Include(p => p.Post);
            return View(await ApplicationDbContext.ToListAsync());
        }

        // GET: PostMetrics/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postMetric = await _context.PostMetrics
                .Include(p => p.ApplicationUser)
                .Include(p => p.Post)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postMetric == null)
            {
                return NotFound();
            }

            return View(postMetric);
        }

        // GET: PostMetrics/Create
        public IActionResult Create()
        {
           
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title");
            return View();
        }

        // POST: PostMetrics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PostId,ApplicationUserId,VoteValue,DateTime,NotifyMe,ReportType")] PostMetric postMetric)
        {
            if (ModelState.IsValid)
            {
                postMetric.Id = Guid.NewGuid();
                postMetric.ApplicationUserId = _userManager.GetUserId(User);
                postMetric.DateTime = DateTime.Now;
                _context.Add(postMetric);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
          
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", postMetric.PostId);
            return View(postMetric);
        }

        // GET: PostMetrics/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postMetric = await _context.PostMetrics.SingleOrDefaultAsync(m => m.Id == id);
            if (postMetric == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", postMetric.ApplicationUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Body", postMetric.PostId);
            return View(postMetric);
        }

        // POST: PostMetrics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,PostId,ApplicationUserId,VoteValue,DateTime,NotifyMe,ReportType")] PostMetric postMetric)
        {
            if (id != postMetric.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postMetric);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostMetricExists(postMetric.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", postMetric.ApplicationUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Body", postMetric.PostId);
            return View(postMetric);
        }

        // GET: PostMetrics/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postMetric = await _context.PostMetrics
                .Include(p => p.ApplicationUser)
                .Include(p => p.Post)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postMetric == null)
            {
                return NotFound();
            }

            return View(postMetric);
        }

        // POST: PostMetrics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var postMetric = await _context.PostMetrics.SingleOrDefaultAsync(m => m.Id == id);
            _context.PostMetrics.Remove(postMetric);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostMetricExists(Guid id)
        {
            return _context.PostMetrics.Any(e => e.Id == id);
        }
    }
}
