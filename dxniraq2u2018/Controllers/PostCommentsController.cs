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
    public class PostCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public PostCommentsController(ApplicationDbContext context,UserManager<ApplicationUser> userMrg)
        {
            _context = context;
            _userManager = userMrg;
        }

        // GET: PostComments
        public async Task<IActionResult> Index()
        {
            var ApplicationDbContext = _context.PostComments.Include(p => p.ApplicationUser).Include(p => p.Post);
            return View(await ApplicationDbContext.ToListAsync());
        }

        // GET: PostComments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postComment = await _context.PostComments
                .Include(p => p.ApplicationUser)
                .Include(p => p.Post)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postComment == null)
            {
                return NotFound();
            }

            return View(postComment);
        }

        // GET: PostComments/Create
        public IActionResult Create()
        {
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title");
            return View();
        }

        // POST: PostComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Comment,DateTime,IsHidden,IsFeatured,IsDeleted,PostId,ApplicationUserId,IsBestAnswer")] PostComment postComment)
        {
            if (ModelState.IsValid)
            {
                postComment.ApplicationUserId = _userManager.GetUserId(User);
                postComment.DateTime = DateTime.Now;
                postComment.IsDeleted = false;
                postComment.IsFeatured = false;
                postComment.IsHidden = false;

                postComment.Id = Guid.NewGuid();
                _context.Add(postComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", postComment.PostId);
            return View(postComment);
        }

        // GET: PostComments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postComment = await _context.PostComments.SingleOrDefaultAsync(m => m.Id == id);
            if (postComment == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", postComment.ApplicationUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", postComment.PostId);
            return View(postComment);
        }

        // POST: PostComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Comment,DateTime,IsHidden,IsFeatured,IsDeleted,PostId,ApplicationUserId,IsBestAnswer")] PostComment postComment)
        {
            if (id != postComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostCommentExists(postComment.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", postComment.ApplicationUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", postComment.PostId);
            return View(postComment);
        }

        // GET: PostComments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postComment = await _context.PostComments
                .Include(p => p.ApplicationUser)
                .Include(p => p.Post)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postComment == null)
            {
                return NotFound();
            }

            return View(postComment);
        }

        // POST: PostComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var postComment = await _context.PostComments.SingleOrDefaultAsync(m => m.Id == id);
            _context.PostComments.Remove(postComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostCommentExists(Guid id)
        {
            return _context.PostComments.Any(e => e.Id == id);
        }
        public IActionResult Best(Guid id)
        {

            PostComment PostComment = _context.PostComments.Where(m => m.Id == id).SingleOrDefault();

            if (PostComment == null)
            {
                return NotFound();
            }

            string currentuserid = _userManager.GetUserId(User);

            if (_context.PostComments.Where(f => f.Id == id && f.ApplicationUserId == currentuserid).Count() == 0)
            {
                PostComment.IsBestAnswer = true;

            }
            else if (_context.PostComments.Where(f => f.Id == id && f.ApplicationUserId == currentuserid && f.IsBestAnswer == true).Count() > 0)
            {
                _context.PostComments.SingleOrDefault(a => a.Id == id & a.ApplicationUserId == currentuserid).IsBestAnswer = true;
            }
            _context.SaveChanges();
            return RedirectToAction("Details/" + id);
        }

    }
}
