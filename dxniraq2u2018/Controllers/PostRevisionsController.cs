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
    public class PostRevisionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostRevisionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PostRevisions
        public async Task<IActionResult> Index()
        {
            var ApplicationDbContext = _context.PostRevisions.Include(p => p.EditorUser).Include(p => p.Post);
            return View(await ApplicationDbContext.ToListAsync());
        }

        // GET: PostRevisions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postRevision = await _context.PostRevisions
                .Include(p => p.EditorUser)
                .Include(p => p.Post)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postRevision == null)
            {
                return NotFound();
            }

            return View(postRevision);
        }

        // GET: PostRevisions/Create
        public IActionResult Create()
        {
            ViewData["EditorUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Body");
            return View();
        }

        // POST: PostRevisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EditorUserId,EditorDateTime,PostId,Title,Body")] PostRevision postRevision)
        {
            if (ModelState.IsValid)
            {
                postRevision.Id = Guid.NewGuid();
                _context.Add(postRevision);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EditorUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", postRevision.EditorUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Body", postRevision.PostId);
            return View(postRevision);
        }

        // GET: PostRevisions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postRevision = await _context.PostRevisions.SingleOrDefaultAsync(m => m.Id == id);
            if (postRevision == null)
            {
                return NotFound();
            }
            ViewData["EditorUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", postRevision.EditorUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Body", postRevision.PostId);
            return View(postRevision);
        }

        // POST: PostRevisions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,EditorUserId,EditorDateTime,PostId,Title,Body")] PostRevision postRevision)
        {
            if (id != postRevision.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postRevision);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostRevisionExists(postRevision.Id))
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
            ViewData["EditorUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", postRevision.EditorUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Body", postRevision.PostId);
            return View(postRevision);
        }

        // GET: PostRevisions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postRevision = await _context.PostRevisions
                .Include(p => p.EditorUser)
                .Include(p => p.Post)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postRevision == null)
            {
                return NotFound();
            }

            return View(postRevision);
        }

        // POST: PostRevisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var postRevision = await _context.PostRevisions.SingleOrDefaultAsync(m => m.Id == id);
            _context.PostRevisions.Remove(postRevision);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostRevisionExists(Guid id)
        {
            return _context.PostRevisions.Any(e => e.Id == id);
        }
    }
}
