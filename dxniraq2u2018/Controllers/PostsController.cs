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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using dxniraq2u2018.AuxiliaryClasses;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using static dxniraq2u2018.Models.Common;

namespace dxniraq2u2018.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _environment;

        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userMrg, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _userManager = userMrg;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int id)
        {
            var ApplicationDbContext = _context.Posts.Where(a => a.CommunityId == id).Include(p => p.ApplicationUser).Include(p => p.Community);
            ViewData["CommunityName"] = _context.Communities.SingleOrDefault(a => a.Id == id).Name;
            return View(await ApplicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> List()
        {
            var ApplicationDbContext = _context.Posts.OrderByDescending(a => a.DateTime).Include(p => p.ApplicationUser).Include(a => a.Community);
            @ViewData["PageName"] = "جديد المشاركات في جميع المجتمعات العلمية";
            return View(await ApplicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Featured()
        {
            var ApplicationDbContext = _context.Posts.Where(a => a.IsFeatured == true).OrderByDescending(a => a.DateTime).Include(p => p.ApplicationUser).Include(a => a.Community);
            @ViewData["PageName"] = "المشاركات المميزة. ساهم بإثرائها";
            return View(await ApplicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> UnSolved()
        {

            var PostViewModel = new PostViewModel
            {
                PostComments = _context.PostComments.Include(p => p.Post).Include(a => a.Post.Community).Include(p => p.ApplicationUser).OrderByDescending(a => a.DateTime)
            };
            @ViewData["PageName"] = "اسئلة لم تحل. ساهم بحلها";
            return View(PostViewModel);
        }

        public async Task<IActionResult> Solved()
        {

            var PostViewModel = new PostViewModel
            {
                PostComments = _context.PostComments.Include(p => p.Post).Include(a => a.Post.Community).Include(p => p.ApplicationUser).OrderByDescending(a => a.DateTime)
            };
            @ViewData["PageName"] = "اسئلة تم حلها. ساهم بإثراءها";
            return View(PostViewModel);
        }

        // GET: Posts/Details/5
        //[Route("{Details}/{id}")]
        public async Task<IActionResult> Details(Guid? id, string t)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                if (_context.Posts.SingleOrDefault(a => a.Id == id).IsDeleted == true)
                {
                    return NotFound();
                }

                if (_context.PostComments.Where(a => a.PostId == id & a.IsBestAnswer == true).Count() > 0)
                { ViewData["Answered"] = "AnswerAssigned"; }

                //if (_context.PostComment.Include(a=>a.Post).Include(a=>a.Post.Community).Where(a => a.Post.Community.CommunityType == CommunityType.Community).Count() > 0)
                //{ ViewData["Answered"] = "AnswerAssigned"; }

                if (_context.Posts.SingleOrDefault(a => a.Id == id).PostType != GroupPostType.QA)
                { ViewData["Answered"] = "AnswerAssigned"; }


                var postViewModel = new PostViewModel
                {
                    Post = await _context.Posts
 .Include(p => p.ApplicationUser)
 .Include(p => p.Community)
 .SingleOrDefaultAsync(m => m.Id == id),
                    PostComments = _context.PostComments.Where(a => a.PostId == id).Include(p => p.ApplicationUser),
                    PostMetrics = _context.PostMetrics.Where(a => a.PostId == id).Include(a => a.ApplicationUser),
                    CommentMetrics = _context.CommentMetrics.Where(a => a.PostComment.PostId == id).Include(a => a.ApplicationUser)
                };

                ViewData["PostWeight"] = _context.PostMetrics.Where(a => a.PostId == id).Sum(a => a.VoteValue);
                //ViewData["CommentWeight"] = _context.CommentMetric.Where(a => a.PostId == id).Sum(a=>a.VoteValue);
                if (postViewModel == null)
                {
                    return NotFound();
                }

                if (string.IsNullOrWhiteSpace(t))
                {
                    t = _context.Posts.First(p => p.Id == id).Title.Replace("/", "-");
                    t = t.Replace(@"\", "-");
                    return RedirectToAction("Details", new { id = id, t = RemoveSpecialChars(t.Replace(" ", "-")) });
                }
                else
                {
                    _context.Posts.SingleOrDefaultAsync(m => m.Id == id).Result.Reads++;
                }
                await _context.SaveChangesAsync();
                return View(postViewModel);

            }
            catch (Exception)
            {
                t = _context.Posts.First(p => p.Id == id).Tags;
                return RedirectToAction("Details", new { id = id });
                // throw;
            }
        }

        public static string RemoveSpecialChars(string str)
        {
            // Create  a string array and add the special characters you want to remove
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]" };
            //Iterate the number of times based on the String array length.
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], "-");
                }
            }
            return str;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([Bind("Id,Comment,DateTime,IsHidden,IsFeatured,IsDeleted,PostId,ApplicationUserId")] PostComment postComment, Guid PostId)
        {
            if (ModelState.IsValid)
            {
                postComment.ApplicationUserId = _userManager.GetUserId(User);
                postComment.DateTime = DateTime.Now;
                postComment.IsDeleted = false;
                postComment.IsFeatured = false;
                postComment.IsHidden = false;
                postComment.PostId = PostId;
                postComment.Id = Guid.NewGuid();
                _context.Add(postComment);

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = PostId });
            }
            return RedirectToAction("Details", new { id = PostId });
        }


        // GET: Posts/Create
        public IActionResult Create(int cid)
        {
            ViewData["CommunityId"] = new SelectList(_context.Communities.Where(a => a.Id == cid), "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? cid, [Bind("Id,Title,Body,DateTime,IsCommentsAllowed,Image,File,IsApproved,IsHidden,IsFeatured,Reads,IsDeleted,CommunityId,ApplicationUserId,Tags,PostType,IsPublishRequest,PublishRequestStatus,IsGifted,GiftType")] Post post, IFormFile myfile, IFormFile myfile1)
        {
            if (ModelState.IsValid)
            {
                post.Image = await UserFile.UploadeNewImageAsync(post.Image,
    myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 500, 500);
                post.File = await UserFile.UploadeNewFileAsync(post.File,
          myfile1, _environment.WebRootPath, Properties.Resources.imgFolder);
                post.ApplicationUserId = _userManager.GetUserId(User);
                post.DateTime = DateTime.Now;
                post.Reads = 0;
                post.IsApproved = false;
                post.IsDeleted = false;
                post.IsHidden = false;
                post.IsFeatured = false;
                post.IsGifted = false;
                //if (_context.Community.SingleOrDefault(a => a.Id == cid).CommunityType == CommunityType.Community)
                //{
                post.PostType = GroupPostType.QA;
                //}
                //else
                //{ post.PostType = PostType.Article; }

                post.Id = Guid.NewGuid();
                _context.Add(post);


                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = post.Id });
            }
            ViewData["CommunityId"] = new SelectList(_context.Communities.Where(a => a.Id == post.CommunityId), "Id", "Name", cid);
            return View(post);
        }

        // GET: Posts/Create
        public IActionResult CreateGroupPost(int cid)
        {
            ViewData["CommunityId"] = new SelectList(_context.Communities.Where(a => a.Id == cid), "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroupPost(int? cid, [Bind("Id,Title,Body,DateTime,IsCommentsAllowed,Image,File,IsApproved,IsHidden,IsFeatured,Reads,IsDeleted,CommunityId,ApplicationUserId,Tags,PostType,IsPublishRequest,PublishRequestStatus,IsGifted,GiftType")] Post post, IFormFile myfile, IFormFile myfile1)
        {
            if (ModelState.IsValid)
            {
                post.Image = await UserFile.UploadeNewImageAsync(post.Image,
    myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 700, 500);
                post.File = await UserFile.UploadeNewFileAsync(post.File,
          myfile1, _environment.WebRootPath, Properties.Resources.imgFolder);
                post.ApplicationUserId = _userManager.GetUserId(User);
                post.DateTime = DateTime.Now;
                post.Reads = 0;
                post.IsApproved = true;
                post.IsDeleted = false;
                post.IsHidden = false;
                post.IsFeatured = false;
                post.IsGifted = false;
                //if (_context.Community.SingleOrDefault(a => a.Id == cid).CommunityType == CommunityType.Community)
                //{
                post.PostType = GroupPostType.QA;
                //}
                //else
                //{ post.PostType = PostType.Article; }

                post.Id = Guid.NewGuid();
                _context.Add(post);


                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = post.Id });
            }
            ViewData["CommunityId"] = new SelectList(_context.Communities.Where(a => a.Id == post.CommunityId), "Id", "Name", cid);
            return View(post);
        }

        // GET: Posts/Create
        public IActionResult CreateBlog(int cid)
        {
            ViewData["CommunityId"] = new SelectList(_context.Communities.Where(a => a.Id == cid), "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBlog(int? cid, [Bind("Id,Title,Body,DateTime,IsCommentsAllowed,Image,File,IsApproved,IsHidden,IsFeatured,Reads,IsDeleted,CommunityId,ApplicationUserId,Tags,PostType,IsPublishRequest,PublishRequestStatus,IsGifted,GiftType")] Post post, IFormFile myfile, IFormFile myfile1)
        {
            if (ModelState.IsValid)
            {
                post.Image = await UserFile.UploadeNewImageAsync(post.Image,
    myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 500, 500);
                post.File = await UserFile.UploadeNewFileAsync(post.File,
          myfile1, _environment.WebRootPath, Properties.Resources.FileFolder);
                post.ApplicationUserId = _userManager.GetUserId(User);
                post.DateTime = DateTime.Now;
                post.Reads = 0;
                post.IsApproved = true;
                post.IsDeleted = false;
                post.IsHidden = false;
                post.IsFeatured = false;
                post.IsGifted = false;
                post.Body = (System.Text.RegularExpressions.Regex.Replace(post.Body, @"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>", String.Empty)).Replace("\n", "<br/>");
                //post.Title = RemoveSpecialChars(post.Title.Replace("/", " "));
                //  post.Title = post.Title.Replace("/", "");
                //  post.Title = post.Title.Replace(@"\", string.Empty);
                //if (_context.Community.SingleOrDefault(a => a.Id == cid).CommunityType == CommunityType.Community)
                //{
                post.PostType = GroupPostType.QA;
                //}
                //else
                //{ post.PostType = PostType.Article; }

                post.Id = Guid.NewGuid();
                _context.Add(post);


                await _context.SaveChangesAsync();
                return RedirectToAction("EditBlog", new { id = post.Id });
            }
            ViewData["CommunityId"] = new SelectList(_context.Communities.Where(a => a.Id == post.CommunityId), "Id", "Name", cid);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> EditBlog(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            //ViewData["Body"] = (post.Body).Replace("<br/>", "\n");
            ViewData["CommunityId"] = new SelectList(_context.Set<Community>(), "Id", "Name", post.CommunityId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBlog(Guid id, [Bind("Id,Title,Body,DateTime,IsCommentsAllowed,Image,File,IsApproved,IsHidden,IsFeatured,Reads,IsDeleted,CommunityId,ApplicationUserId,Tags,PostType,IsPublishRequest,PublishRequestStatus,IsGifted,GiftType")] Post post, IFormFile myfile, IFormFile myfile1)
        {
            if (id != post.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    post.Image = await UserFile.UploadeNewImageAsync(post.Image,
myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 500, 500);

                    post.File = await UserFile.UploadeNewFileAsync(post.File,
              myfile1, _environment.WebRootPath, Properties.Resources.imgFolder);
                    post.ApplicationUserId = _userManager.GetUserId(User);
                    post.DateTime = DateTime.Now;
                    //post.Body = (System.Text.RegularExpressions.Regex.Replace(post.Body, @"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>", String.Empty)).Replace("\n", "<br/>");
                    //  post.Title = post.Title.Replace("/", "-");

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = post.Id });
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", post.ApplicationUserId);
            ViewData["CommunityId"] = new SelectList(_context.Set<Community>(), "Id", "BgImage", post.CommunityId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.ApplicationUser)
                .Include(p => p.Community)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            //var PostComments = _context.PostComment.Where(m => m.PostId == id);
            //var Scorelog = _context.ScoreLog.Where(m => m.PostId == id);
            //var PostMetric =  _context.PostMetric.SingleOrDefault(m => m.PostId == id);
            _context.Posts.SingleOrDefault(m => m.Id == id).IsDeleted = true;

            //_context.PostComment.RemoveRange(PostComments);
            //_context.ScoreLog.RemoveRange(Scorelog);
            //_context.PostMetric.Remove(PostMetric);
            //_context.Post.Remove(post);

            _context.SaveChanges();
            return RedirectToAction(nameof(Index), "Blogs");
        }

        private bool PostExists(Guid id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        [Authorize]
        public IActionResult Follow(Guid id)
        {
            Post post = _context.Posts.Where(m => m.Id == id).SingleOrDefault();
            if (post == null)
            {
                return NotFound();
            }
            string currentuserid = _userManager.GetUserId(User);
            if (_context.PostMetrics.Where(f => f.PostId == id && f.ApplicationUserId == currentuserid).Count() == 0)
            {
                _context.PostMetrics.Add(new PostMetric
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = currentuserid,
                    PostId = id,
                    NotifyMe = true,
                    DateTime = DateTime.Now
                });

            }

            else
            {
                _context.PostMetrics.SingleOrDefault(a => a.PostId == id && a.ApplicationUserId == currentuserid).NotifyMe = true;
            }
            _context.SaveChanges();
            return RedirectToAction("Details/" + id);
        }

        [Authorize]
        public IActionResult UnFollow(Guid id)
        {
            Post post = _context.Posts.Where(m => m.Id == id).SingleOrDefault();
            if (post == null)
            {
                return NotFound();
            }
            string currentuserid = _userManager.GetUserId(User);
            if (_context.PostMetrics.Where(f => f.PostId == id && f.ApplicationUserId == currentuserid && f.NotifyMe == true).Count() > 0)
            {
                _context.PostMetrics.SingleOrDefault(a => a.PostId == id & a.ApplicationUserId == currentuserid).NotifyMe = false;
                _context.SaveChanges();
            }
            //ViewData["Count"] = _context.PostMetric.Count(a => a.PostId == id);
            return RedirectToAction("Details/" + id);
        }

        [Authorize]
        public IActionResult Up(Guid id)
        {
            Post post = _context.Posts.Where(m => m.Id == id).SingleOrDefault();
            if (post == null)
            {
                return NotFound();
            }

            string currentuserid = _userManager.GetUserId(User);
            if (_context.PostMetrics.Where(f => f.PostId == id && f.ApplicationUserId == currentuserid).Count() == 0)
            {
                if (currentuserid != post.ApplicationUserId)
                {
                    _context.PostMetrics.Add(new PostMetric
                    {
                        Id = Guid.NewGuid(),
                        ApplicationUserId = currentuserid,
                        PostId = id,
                        NotifyMe = false,
                        VoteValue = 1,
                        DateTime = DateTime.Now,
                        ReportType = ReportType.None
                    });

                    if (_context.PostMetrics.Where(a => a.PostId == id).Sum(a => a.VoteValue) > 5)
                    {
                        post.IsFeatured = true;
                    }
                }
            }
            else if (_context.PostMetrics.Where(f => f.PostId == id && f.ApplicationUserId == currentuserid && f.VoteValue == 0).Count() > 0)
            {
                if (currentuserid != post.ApplicationUserId)
                {
                    _context.PostMetrics.SingleOrDefault(a => a.PostId == id & a.ApplicationUserId == currentuserid).VoteValue = 1;

                    if (_context.PostMetrics.Where(a => a.PostId == id).Sum(a => a.VoteValue) > 5)
                    {
                        post.IsFeatured = true;
                    }


                }
            }

            else if (_context.PostMetrics.Where(f => f.PostId == id && f.ApplicationUserId == currentuserid && f.VoteValue == -1).Count() > 0)
            {
                if (currentuserid != post.ApplicationUserId)
                {
                    _context.PostMetrics.SingleOrDefault(a => a.PostId == id & a.ApplicationUserId == currentuserid).VoteValue = 1;

                    if (_context.PostMetrics.Where(a => a.PostId == id).Sum(a => a.VoteValue) > 5)
                    {
                        post.IsFeatured = true;
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Details/" + id);
        }
        [Authorize]
        public IActionResult Down(Guid id)
        {
            Post post = _context.Posts.Where(m => m.Id == id).SingleOrDefault();

            if (post == null)
            {
                return NotFound();
            }

            string currentuserid = _userManager.GetUserId(User);

            if (_context.PostMetrics.Where(f => f.PostId == id && f.ApplicationUserId == currentuserid).Count() == 0)
            {
                if (currentuserid != post.ApplicationUserId)
                {
                    _context.PostMetrics.Add(new PostMetric
                    {
                        Id = Guid.NewGuid(),
                        ApplicationUserId = currentuserid,
                        PostId = id,
                        NotifyMe = false,
                        VoteValue = -1
                    });

                }

            }
            else if (_context.PostMetrics.Where(f => f.PostId == id && f.ApplicationUserId == currentuserid && f.VoteValue == 0).Count() > 0)
            {
                if (currentuserid != post.ApplicationUserId)
                {
                    _context.PostMetrics.SingleOrDefault(a => a.PostId == id & a.ApplicationUserId == currentuserid).VoteValue = -1;

                }
            }

            else if (_context.PostMetrics.Where(f => f.PostId == id && f.ApplicationUserId == currentuserid && f.VoteValue == 1).Count() > 0)
            {
                if (currentuserid != post.ApplicationUserId)
                {
                    _context.PostMetrics.SingleOrDefault(a => a.PostId == id & a.ApplicationUserId == currentuserid).VoteValue = -1;
                }
            }
            _context.SaveChanges();


            return RedirectToAction("Details/" + id);
        }


        [Authorize]
        public IActionResult CommentUp(Guid id)
        {

            PostComment PostComment = _context.PostComments.Where(m => m.Id == id).SingleOrDefault();

            if (PostComment == null)
            {
                return NotFound();
            }

            string currentuserid = _userManager.GetUserId(User);
            if (currentuserid != PostComment.ApplicationUserId)
            {
                if (_context.CommentMetrics.Where(f => f.PostCommentId == id && f.ApplicationUserId == currentuserid).Count() == 0)
                {
                    _context.CommentMetrics.Add(new CommentMetric
                    {
                        Id = Guid.NewGuid(),
                        ApplicationUserId = currentuserid,
                        PostCommentId = id,
                        VoteValue = 1,
                        Date = DateTime.Now,
                        ReportType = ReportType.None
                    });

                }
                else if (_context.CommentMetrics.Where(f => f.PostCommentId == id && f.ApplicationUserId == currentuserid && f.VoteValue == -1).Count() > 0)
                {
                    _context.CommentMetrics.SingleOrDefault(a => a.PostCommentId == id & a.ApplicationUserId == currentuserid).VoteValue = 1;
                }

                _context.SaveChanges();
            }
            return RedirectToAction("Details/" + id);
        }

        [Authorize]
        public IActionResult CommentDown(Guid id)
        {

            PostComment PostComment = _context.PostComments.Where(m => m.Id == id).SingleOrDefault();

            if (PostComment == null)
            {
                return NotFound();
            }

            string currentuserid = _userManager.GetUserId(User);
            if (currentuserid != PostComment.ApplicationUserId)
            {
                if (_context.CommentMetrics.Where(f => f.PostCommentId == id && f.ApplicationUserId == currentuserid).Count() == 0)
                {
                    _context.CommentMetrics.Add(new CommentMetric
                    {
                        Id = Guid.NewGuid(),
                        ApplicationUserId = currentuserid,
                        PostCommentId = id,
                        VoteValue = -1,
                        Date = DateTime.Now,
                        ReportType = ReportType.None
                    });

                }
                else if (_context.CommentMetrics.Where(f => f.PostCommentId == id && f.ApplicationUserId == currentuserid && f.VoteValue == 1).Count() > 0)
                {
                    _context.CommentMetrics.SingleOrDefault(a => a.PostCommentId == id & a.ApplicationUserId == currentuserid).VoteValue = -1;
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Details/" + id);
        }
        public IActionResult Best(Guid id)
        {

            PostComment PostComment = _context.PostComments.Where(m => m.Id == id).SingleOrDefault();

            if (PostComment == null)
            {
                return NotFound();
            }

            string currentuserid = _userManager.GetUserId(User);

            if (PostComment.IsBestAnswer == true)
            {
                _context.PostComments.SingleOrDefault(a => a.Id == id).IsBestAnswer = false;
            }
            else
            { _context.PostComments.SingleOrDefault(a => a.Id == id).IsBestAnswer = true; }
            //_context.PostComment.SingleOrDefault(a => a.Id == id).IsBestAnswer = true;
            //if (_context.PostComment.Where(f => f.Id == id && f.ApplicationUserId == currentuserid).Count() == 0)
            //{
            //    PostComment.IsBestAnswer = true;

            //}
            //else if (_context.PostComment.Where(f => f.Id == id && f.ApplicationUserId == currentuserid && f.IsBestAnswer == true).Count() > 0)
            //{
            //    _context.PostComment.SingleOrDefault(a => a.Id == id & a.ApplicationUserId == currentuserid).IsBestAnswer = false;
            //}


            _context.SaveChanges();
            //return RedirectToAction("Details/" + id);
            return RedirectToAction("Details", new { id = id });
        }

    }
}
