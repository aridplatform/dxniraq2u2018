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
    public class CommentMetricsController : Controller
    {
        private readonly ApplicationDbContext _context;
                private UserManager<ApplicationUser> _userManager;

        public CommentMetricsController(ApplicationDbContext context,UserManager<ApplicationUser> userMrg)
        {
            _context = context;
                        _userManager = userMrg;
        }

        // GET: CommentMetrics
        public async Task<IActionResult> Index()
        {
            var ApplicationDbContext = _context.CommentMetrics.Include(c => c.ApplicationUser).Include(c => c.PostComment);
            return View(await ApplicationDbContext.ToListAsync());
        }

        // GET: CommentMetrics/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentMetric = await _context.CommentMetrics
                .Include(c => c.ApplicationUser)
                .Include(c => c.PostComment)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (commentMetric == null)
            {
                return NotFound();
            }

            return View(commentMetric);
        }

        // GET: CommentMetrics/Create
        public IActionResult Create()
        {
          
            ViewData["PostCommentId"] = new SelectList(_context.PostComments, "Id", "Comment");
            return View();
        }

        // POST: CommentMetrics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PostCommentId,ApplicationUserId,VoteValue,Date,ReportType")] CommentMetric commentMetric)
        {
            if (ModelState.IsValid)
            {
                commentMetric.ApplicationUserId = _userManager.GetUserId(User);
                commentMetric.Date = DateTime.Now;
                commentMetric.Id = Guid.NewGuid();
                _context.Add(commentMetric);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            ViewData["PostCommentId"] = new SelectList(_context.PostComments, "Id", "Comment", commentMetric.PostCommentId);
            return View(commentMetric);
        }

        // GET: CommentMetrics/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentMetric = await _context.CommentMetrics.SingleOrDefaultAsync(m => m.Id == id);
            if (commentMetric == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", commentMetric.ApplicationUserId);
            ViewData["PostCommentId"] = new SelectList(_context.PostComments, "Id", "Comment", commentMetric.PostCommentId);
            return View(commentMetric);
        }

        // POST: CommentMetrics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,PostCommentId,ApplicationUserId,VoteValue,Date,ReportType")] CommentMetric commentMetric)
        {
            if (id != commentMetric.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentMetric);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentMetricExists(commentMetric.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", commentMetric.ApplicationUserId);
            ViewData["PostCommentId"] = new SelectList(_context.PostComments, "Id", "Comment", commentMetric.PostCommentId);
            return View(commentMetric);
        }

        // GET: CommentMetrics/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentMetric = await _context.CommentMetrics
                .Include(c => c.ApplicationUser)
                .Include(c => c.PostComment)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (commentMetric == null)
            {
                return NotFound();
            }

            return View(commentMetric);
        }

        // POST: CommentMetrics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var commentMetric = await _context.CommentMetrics.SingleOrDefaultAsync(m => m.Id == id);
            _context.CommentMetrics.Remove(commentMetric);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentMetricExists(Guid id)
        {
            return _context.CommentMetrics.Any(e => e.Id == id);
        }
    }
}
