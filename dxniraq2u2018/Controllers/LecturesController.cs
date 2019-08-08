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
using System.Net.Mail;
using System.Net;
using FluentEmail.Mailgun;
using FluentEmail.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using dxniraq2u2018.AuxiliaryClasses;

namespace dxniraq2u2018.Controllers
{
    [Authorize]
    public class LecturesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _environment;
        private int PagSize = 50;
        public LecturesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: Lectures

        public async Task<IActionResult> Index(string SearchString, int productPage = 1)
        {
            LectureViewModel LectureViewModel = new LectureViewModel();

            if (string.IsNullOrEmpty(SearchString))
            {
                LectureViewModel = new LectureViewModel()
                { Lectures = _context.Lectures.Where(a => a.Date > DateTime.Now).Include(a => a.Instructor).Include(a => a.Branch).OrderByDescending(a => a.Date) };
            }
            else if (!string.IsNullOrEmpty(SearchString))
            {
                LectureViewModel = new LectureViewModel()
                {
                    Lectures = _context.Lectures.Where(a => a.Date > DateTime.Now).Include(a => a.Instructor).Include(a => a.Branch).OrderByDescending(a => a.Date).Where(a => a.Title.Contains(SearchString) || a.Content.Contains(SearchString) || a.Branch.Name.Contains(SearchString) || a.Instructor.ArName.Contains(SearchString))
                };
            }

            var count = LectureViewModel.Lectures.Count();
            LectureViewModel.Lectures = LectureViewModel.Lectures.OrderBy(p => p.Id)
                .Skip((productPage - 1) * PagSize)
                .Take(PagSize).ToList();

            LectureViewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PagSize,
                TotalItem = count
            };
            return View(LectureViewModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ViewAll()
        {
            var applicationDbContext = _context.Lectures.Where(a => a.IsAdminApproved == true).Include(l => l.Branch).Include(a => a.Instructor).OrderByDescending(a => a.Date);
            return View(await applicationDbContext.ToListAsync());

        }

        public async Task<IActionResult> IndexAdmin()
        {
            var applicationDbContext = _context.Lectures.Where(a => a.IsAdminApproved == false).Include(l => l.Branch).Include(a => a.Instructor).OrderByDescending(a => a.Date);
            return View(await applicationDbContext.ToListAsync());

        }

        public static void SendEmailAsync()
        {
            //var smtpClient = new SmtpClient
            //{
            //    Host = "smtp.gmail.com", // set your SMTP server name here
            //    Port = 587, // Port 
            //    EnableSsl = true,
            //    Credentials = new NetworkCredential("info@almahfal.org", "bazbomb521")
            //};

            //using (var message = new MailMessage("info@almahfal.org", "info@filspay.com")
            //{
            //    Subject = "Subject",
            //    Body = "Body"
            //})
            //{
            //     smtpClient.SendMailAsync(message);
            //}

            var sender = new MailgunSender(
     "arid.my", //https://github.com/lukencode/FluentEmail
     "key-a18c85ac0e054b5a4f078fed4d48afb7" // Mailgun API Key
 );

            Email.DefaultSender = sender;

            var email = Email
                .From("info@almahfal.org")
                .To("info@filspay.com", "Saif")
                 .CC("yousifalsewaidi@gmail.com", "Yousif")
                .Subject("New Course Added")
                .Body("Design please");

            email.SendAsync();
        }


        // GET: Lectures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lectures
                .Include(l => l.Branch)
                   .Include(l => l.Instructor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // GET: Lectures/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Name");
            ViewData["InstructorId"] = new SelectList(_context.ApplicationUser.Where(a => a.IsInstructor == true), "Id", "ArName");
            //ViewData["LevelType"] = new SelectList(Common.CourseLevel , "Id", "ArName");

            ViewData["CurrentDate"] = DateTime.Now;
            return View();
        }



        // POST: Lectures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Date,InstructorId,BranchId,Seats,IsOpen,LevelType,IsOnline,IsAdminApproved,Flyer")] Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                lecture.IsOpen = true;
                lecture.IsOnline = false;
                lecture.IsAdminApproved = false;
                lecture.InstructorId = _userManager.GetUserId(User);


                _context.Add(lecture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Name");
            // ViewData["InstructorId"] = new SelectList(_context.ApplicationUser.Where(a => a.IsInstructor == true), "Id", "ArName");
            //  https://techbrij.com/asp-net-core-mvc-enums-select-taghelper
            return View(lecture);
        }

        // GET: Lectures/Create
        public IActionResult CreateAdmin()
        {
            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Name");
            ViewData["InstructorId"] = new SelectList(_context.ApplicationUser.Where(a => a.IsInstructor == true), "Id", "ArName");
            //ViewData["LevelType"] = new SelectList(Common.CourseLevel , "Id", "ArName");

            ViewData["CurrentDate"] = DateTime.Now;
            return View();
        }



        // POST: Lectures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin([Bind("Id,Title,Content,Date,InstructorId,BranchId,Seats,IsOpen,LevelType,IsOnline,IsAdminApproved,Flyer")] Lecture lecture, IFormFile myfile)
        {
            if (ModelState.IsValid)
            {
                lecture.Flyer = await UserFile.UploadeNewImageAsync(lecture.Flyer,
       myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 620, 877);

                _context.Add(lecture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Name");
            ViewData["InstructorId"] = new SelectList(_context.ApplicationUser.Where(a => a.IsInstructor == true), "Id", "ArName");
            //  https://techbrij.com/asp-net-core-mvc-enums-select-taghelper
            return View(lecture);
        }

        // GET: Lectures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lectures.Include(a => a.Instructor).Include(a => a.Instructor.MemberType).SingleOrDefaultAsync(m => m.Id == id);
            if (lecture == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Name", lecture.BranchId);
            return View(lecture);
        }

        // POST: Lectures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Date,InstructorId,BranchId,Seats,IsOpen,LevelType,IsOnline,IsAdminApproved,Flyer")] Lecture lecture, IFormFile myfile)
        {
            if (id != lecture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    lecture.Flyer = await UserFile.UploadeNewFileAsync(lecture.Flyer,
   myfile, _environment.WebRootPath, Properties.Resources.imgFolder);

                    _context.Update(lecture);
                    await _context.SaveChangesAsync();

                    if (lecture.IsAdminApproved==true ) {
                        SendEmailAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LectureExists(lecture.Id))
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
            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Name", lecture.BranchId);
            return View(lecture);
        }

        // GET: Lectures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lectures
                .Include(l => l.Branch)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // POST: Lectures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lecture = await _context.Lectures.SingleOrDefaultAsync(m => m.Id == id);
            _context.Lectures.Remove(lecture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LectureExists(int id)
        {
            return _context.Lectures.Any(e => e.Id == id);
        }
    }
}
