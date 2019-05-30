using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dxniraq2u2018.Data;
using dxniraq2u2018.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using dxniraq2u2018.AuxiliaryClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using dxniraq2u2018.Services;
using static dxniraq2u2018.Models.Common;

namespace dxniraq2u2018.Controllers
{
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _environment;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public BlogsController(ApplicationDbContext context, UserManager<ApplicationUser> userMrg, IHostingEnvironment environment, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userMrg;
            _environment = environment;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        // GET: Badges
        [AllowAnonymous]
        public async Task<IActionResult> List()
        {
            var ApplicationDbContext = _context.Communities.Where(a => a.CommunityType == CommunityType.Personal).Include(a => a.ApplicationUser).OrderByDescending(a => a.Id);
            return View(await ApplicationDbContext.ToListAsync());
        }

        // GET: Blogs
        //[Route("Blog")]
        public async Task<IActionResult> Index(int? id, string keyword)
        {
            if (id == null && _signInManager.IsSignedIn(User) && _context.Communities.Where(a => a.CommunityType == CommunityType.Personal && a.Id == id).Count() > 0)
            {
                return RedirectToAction("Index", new { id = _context.Communities.SingleOrDefault(a => a.CommunityType == CommunityType.Personal && a.ApplicationUserId == _userManager.GetUserId(User)).Id });
            }



            if (_context.Communities.Where(a => a.CommunityType == CommunityType.Personal && a.Id == id).Count() > 0)
            {
                var CommunityEntity = await _context.Communities
                                 .SingleOrDefaultAsync(m => m.Id == id);

                ViewData["Count"] = _context.CommunityFollowers.Count(a => a.CommunityId == id);
                ViewData["CommunityId"] = id;
                //var commentMetric = await _context.Community
                //                   .SingleOrDefaultAsync(m => m.Id == id);
                //if (commentMetric == null)
                //{
                //    return NotFound();
                //}
                if (keyword != null)
                {
                    var communityViewModel = new CommunityViewModel
                    {
                        ApplicationUser = await _userManager.Users
.SingleOrDefaultAsync(u => u.Id == CommunityEntity.ApplicationUserId),

                        CommunityFollower = _context.CommunityFollowers
.Include(f => f.ApplicationUser)
.Include(f => f.Community)
.Where(f => f.ApplicationUserId == CommunityEntity.ApplicationUserId)
.ToList(),

                        Community = await _context.Communities
               .SingleOrDefaultAsync(m => m.Id == id),

                        Posts = _context.Posts.Where(b => b.CommunityId == id && b.IsDeleted == false && (b.Title.Contains(keyword) || b.Body.Contains(keyword) || b.Tags.Contains(keyword))).OrderByDescending(a => a.DateTime),
                        //& (a => a.Body.Contains(keyword) || a.Title.Contains(keyword))),
                        PostComments = _context.PostComments.Where(a => a.Post.CommunityId == id),
                        PostMetrics = _context.PostMetrics.Where(a => a.Post.CommunityId == id)
                    };
                    return View(communityViewModel);
                }
                else
                {
                    var communityViewModel = new CommunityViewModel
                    {
                        ApplicationUser = await _userManager.Users
                  .SingleOrDefaultAsync(u => u.Id == CommunityEntity.ApplicationUserId),

                        CommunityFollower = _context.CommunityFollowers
                  .Include(f => f.ApplicationUser)
                   .Include(f => f.Community)
                  .Where(f => f.ApplicationUserId == CommunityEntity.ApplicationUserId)
                  .ToList(),

                        Community = await _context.Communities
                                   .SingleOrDefaultAsync(m => m.Id == id),

                        Posts = _context.Posts.Where(a => a.CommunityId == id && a.IsDeleted == false).Include(p => p.ApplicationUser).Include(p => p.Community).OrderByDescending(a => a.DateTime),
                        PostComments = _context.PostComments.Where(a => a.Post.CommunityId == id),
                        PostMetrics = _context.PostMetrics.Where(a => a.Post.CommunityId == id)
                    };
                    return View(communityViewModel);
                }
            }
            else { return RedirectToAction("Create"); }
        }


        // GET: Communities/Create
        public IActionResult Create()
        {
            if (_context.Communities.Where(a => a.CommunityType == CommunityType.Personal && a.ApplicationUserId == _userManager.GetUserId(User)).Count() > 0)
            {
                return RedirectToAction("Index", new { id = _context.Communities.SingleOrDefault(a => a.CommunityType == CommunityType.Personal && a.ApplicationUserId == _userManager.GetUserId(User)).Id });
            };

          
            ViewData["ArName"] = "مدونة " + _context.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).ArName;
            ViewData["EnName"] = _context.Users.SingleOrDefault(a => a.Id == _userManager.GetUserId(User)).EnName + " Blog";
            return View();
        }

        // POST: Communities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserId,Name,ShortName,BgImage,Logo,Description,CreationDate,CommunityType,SpecialityId,SecurityLevel,IsCommentsAllowed,IsFeatured,IsApproved,IsSuspended,Tags")] Community community, IFormFile myfile, IFormFile myfile1)
        {
            if (ModelState.IsValid)
            {
                community.ApplicationUserId = _userManager.GetUserId(User);
                community.BgImage = await UserFile.UploadeNewImageAsync(community.BgImage,
myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 400, 150);

                community.Logo = await UserFile.UploadeNewImageAsync(community.Logo,
myfile1, _environment.WebRootPath, Properties.Resources.imgFolder, 100, 100);
                community.CreationDate = DateTime.Now;
                community.IsFeatured = false;
                community.IsSuspended = false;
                community.IsApproved = false;
                community.IsCommentsAllowed = true;
                community.SecurityLevel = SecurityLevel.Open;
                community.CommunityType = CommunityType.Personal;
                _context.Add(community);
                await _context.SaveChangesAsync();
                try
                {
                    var ApplicationUser = _context.Communities.Include(c => c.ApplicationUser).SingleOrDefault(a => a.Id == community.Id).ApplicationUser;
                  
                    //EmailContent content2 = _context.EmailContents.Where(m => m.Id == 1020).SingleOrDefault();
                    //BackgroundJob.Schedule(() => _emailSender.SendEmailAsync(ApplicationUser.Email, content2.ArSubject, content2.ArContent), TimeSpan.FromMinutes(1));
                }
                catch (Exception)
                {
                    return RedirectToAction(nameof(Index), new { id = community.Id });
                }


                return RedirectToAction(nameof(Index), new { id = community.Id });
            }
            //ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", community.ApplicationUserId);
            //ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", community.SpecialityId);
            return View(community);
        }
        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
                //  id = _context.Community.SingleOrDefault(a => a.CommunityType == CommunityType.Personal && a.ApplicationUserId == _userManager.GetUserId(User)).Id;
            }

            var community = await _context.Communities.SingleOrDefaultAsync(m => m.Id == id && m.ApplicationUserId == _userManager.GetUserId(User));
            if (community == null)
            {
                return NotFound();
            }
            // ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", community.ApplicationUserId);
            //ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", community.SpecialityId);
            return View(community);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationUserId,Name,ShortName,BgImage,Logo,Description,CreationDate,CommunityType,SpecialityId,SecurityLevel,IsCommentsAllowed,IsFeatured,IsApproved,IsSuspended,Tags")] Community community, IFormFile myfile, IFormFile myfile1)
        {
            if (id != community.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    community.ApplicationUserId = _userManager.GetUserId(User);
                    community.BgImage = await UserFile.UploadeNewImageAsync(community.BgImage,
    myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 1145, 330);

                    community.Logo = await UserFile.UploadeNewImageAsync(community.Logo,
    myfile1, _environment.WebRootPath, Properties.Resources.imgFolder, 100, 100);

                    community.CreationDate = DateTime.Now;
                    community.IsFeatured = false;
                    community.IsSuspended = false;
                    community.IsApproved = false;
                    community.IsCommentsAllowed = true;
                    community.SecurityLevel = SecurityLevel.Open;
                    community.CommunityType = CommunityType.Personal;
                    _context.Update(community);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunityExists(community.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", community.ApplicationUserId);
            return View(community);
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                id = _context.Communities.SingleOrDefault(a => a.CommunityType == CommunityType.Personal && a.ApplicationUserId == _userManager.GetUserId(User)).Id;
            }

            var community = await _context.Communities
                .Include(c => c.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (community == null)
            {
                return NotFound();
            }

            return View(community);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var community = await _context.Communities.SingleOrDefaultAsync(m => m.Id == id);
            var communityfollowers = _context.CommunityFollowers.Where(a => a.CommunityId == id);
            var PostMetrics = _context.PostMetrics.Where(a => a.Post.CommunityId == id);
            var PostComments = _context.PostComments.Where(a => a.Post.CommunityId == id);
            var Posts = _context.Posts.Where(a => a.CommunityId == id);
            _context.CommunityFollowers.RemoveRange(communityfollowers);
            _context.Posts.RemoveRange(Posts);
            _context.Communities.Remove(community);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommunityExists(int id)
        {
            return _context.Communities.Any(e => e.Id == id);
        }

        [Authorize]
        public IActionResult Follow(int id)
        {
            Community community = _context.Communities.Where(m => m.Id == id).SingleOrDefault();
            if (community == null)
            {
                return NotFound();
            }
            string currentuserid = _userManager.GetUserId(User);

            if (_context.CommunityFollowers.Where(f => f.CommunityId == id && f.ApplicationUserId == currentuserid).Count() == 0)
            {
                _context.CommunityFollowers.Add(new CommunityFollower
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = currentuserid,
                    CommunityId = id,
                    NotifyMe = true,
                    IsAdmin = false
                });
                _context.SaveChanges();
            }
            return RedirectToAction("Index/" + id);
        }

        [Authorize]
        public IActionResult Unfollow(int id)
        {
            Community community = _context.Communities.Where(m => m.Id == id).SingleOrDefault();
            if (community == null)
            {
                return NotFound();
            }

            if (_context.CommunityFollowers.Where(f => f.CommunityId == id && f.ApplicationUserId == _userManager.GetUserId(User)).Count() > 0)
            {
                var Followed = _context.CommunityFollowers.SingleOrDefault(f => f.CommunityId == id && f.ApplicationUserId == _userManager.GetUserId(User));
                _context.CommunityFollowers.Remove(Followed);
                _context.SaveChanges();
            }
            return RedirectToAction("Index/" + id);
        }
    }
}
