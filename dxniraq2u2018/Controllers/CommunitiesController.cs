using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dxniraq2u2018.Data;
using dxniraq2u2018.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using dxniraq2u2018.AuxiliaryClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


using Newtonsoft.Json;
using static dxniraq2u2018.Models.Common;

namespace ARID.Controllers
{
    //[Route("Community/[action]")]
    public class CommunitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private UserManager<ApplicationUser> _userManager;

        public CommunitiesController(ApplicationDbContext ApplicationDbContext, IHostingEnvironment environment, UserManager<ApplicationUser> userMrg)
        {
            _context = ApplicationDbContext;
            _environment = environment;
            _userManager = userMrg;
        }
        

        public async Task<IActionResult> Index()
        {
            var communityViewModel = new CommunityViewModel
            {
                Communities = _context.Communities.Where(a => a.CommunityType == CommunityType.Community),
                CommunityFollower = _context.CommunityFollowers.ToList(),
                Posts = _context.Posts.ToList(),
              
            };
            
            return View(communityViewModel);
        }

       


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Count"] = _context.CommunityFollowers.Count(a => a.CommunityId == id);
            ViewData["CommunityId"] = id;
            //var commentMetric = await _context.Community
            //                   .SingleOrDefaultAsync(m => m.Id == id);
            //if (commentMetric == null)
            //{
            //    return NotFound();
            //}

            var communityViewModel = new CommunityViewModel
            {
                ApplicationUser = await _userManager.Users
              .SingleOrDefaultAsync(u => u.Id == _userManager.GetUserId(User)),

                CommunityFollower = _context.CommunityFollowers
              .Include(f => f.ApplicationUser)
               .Include(f => f.Community)
              .Where(f => f.ApplicationUserId == _userManager.GetUserId(User))
              .ToList(),

                Community = await _context.Communities
                               .SingleOrDefaultAsync(m => m.Id == id),

                Posts = _context.Posts.Where(a => a.CommunityId == id).Include(p => p.ApplicationUser).Include(p => p.Community),
                PostComments = _context.PostComments.Where(a => a.Post.CommunityId == id),
                PostMetrics = _context.PostMetrics.Where(a => a.Post.CommunityId == id)
            };

            return View(communityViewModel);
        }

    
        // GET: Communities/Create
        public IActionResult Create()
        {

         
         
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
                community.BgImage = await UserFile.UploadeNewFileAsync(community.BgImage,
myfile, _environment.WebRootPath, dxniraq2u2018.Properties.Resources.imgFolder);

                community.Logo = await UserFile.UploadeNewImageAsync(community.Logo,
myfile1, _environment.WebRootPath, dxniraq2u2018.Properties.Resources.imgFolder, 50, 50);

                community.CreationDate = DateTime.Now;
                community.IsFeatured = false;
                community.IsSuspended = false;
                community.IsApproved = false;
                community.IsCommentsAllowed = true;
                community.SecurityLevel = SecurityLevel.Open;
                community.CommunityType = CommunityType.Community;
                _context.Add(community);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", community.ApplicationUserId);
          
            return View(community);
        }

        // GET: Communities/Edit/5
        [Authorize(Roles = RoleName.Admins)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var community = await _context.Communities.SingleOrDefaultAsync(m => m.Id == id);
            if (community == null)
            {
                return NotFound();
            }

         
            return View(community);
        }

        // POST: Communities/Edit/5
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
                    community.BgImage = await UserFile.UploadeNewFileAsync(community.BgImage,
myfile, _environment.WebRootPath, dxniraq2u2018.Properties.Resources.imgFolder);

                    community.Logo = await UserFile.UploadeNewImageAsync(community.Logo,
    myfile1, _environment.WebRootPath, dxniraq2u2018.Properties.Resources.imgFolder, 50, 50);

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

        // GET: Communities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
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

        // POST: Communities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var community = await _context.Communities.SingleOrDefaultAsync(m => m.Id == id);
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
            return RedirectToAction("Details/" + id);
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
            return RedirectToAction("Details/" + id);
        }
    }
}
