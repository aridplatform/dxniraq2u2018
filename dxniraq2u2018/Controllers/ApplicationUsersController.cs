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
using Microsoft.AspNetCore.Http;
using dxniraq2u2018.AuxiliaryClasses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace dxniraq2u2018.Controllers
{
    [Authorize]
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IHostingEnvironment _environment;
        private int PagSize = 100;
        public ApplicationUsersController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userMrg, IHostingEnvironment environment)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userMrg;
            _environment = environment;
        }

        // GET: ApplicationUsers
        public async Task<IActionResult> Index(string SearchString, int productPage = 1)
        {
            ApplicationUserViewModel ApplicationUserViewModel = new ApplicationUserViewModel();

            if (string.IsNullOrEmpty(SearchString))
            {
                ApplicationUserViewModel = new ApplicationUserViewModel()
                { ApplicationUsers = _context.ApplicationUser.OrderByDescending(a => a.RegDate)};
            }
            else if (!string.IsNullOrEmpty(SearchString))
            {
                ApplicationUserViewModel = new ApplicationUserViewModel()
                {
                    ApplicationUsers = _context.ApplicationUser.OrderByDescending(a => a.RegDate).Where(a => a.ArName.Contains(SearchString) || a.EnName.Contains(SearchString) || a.Email.Contains(SearchString) || a.PhoneNumber.Contains(SearchString) || a.DxnId.Contains(SearchString))
                };
            }
            
            var count = ApplicationUserViewModel.ApplicationUsers.Count();
            ApplicationUserViewModel.ApplicationUsers = ApplicationUserViewModel.ApplicationUsers.OrderBy(p => p.Id)
                .Skip((productPage - 1) * PagSize)
                .Take(PagSize).ToList();

            ApplicationUserViewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PagSize,
                TotalItem = count
            };
            return View(ApplicationUserViewModel);
        }


        public async Task<IActionResult> UsersList()
        {
            var applicationDbContext = _context.ApplicationUser.Where(a => a.SponsorId == _context.ApplicationUser.SingleOrDefault(c => c.Id == _userManager.GetUserId(User)).DxnId).OrderByDescending(a => a.RegDate).Include(a => a.Branch).Include(a => a.City).Include(a => a.MemberType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
                .Include(a => a.Branch)
                .Include(a => a.City)
                .Include(a => a.MemberType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Id");
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "ArCityName");
            ViewData["MemberTypeId"] = new SelectList(_context.MemberTypes, "Id", "Id");
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArName,EnName,SponsorId,DxnId,MemberTypeId,CityId,Gender,SecondEmail,DateofBirth,ProfileImage,IsInstructor,IsBranchManager,IsBranchEmployee,IsDriver,BranchId,IsDXNMember,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Id", applicationUser.BranchId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "ArCityName", applicationUser.CityId);
            ViewData["MemberTypeId"] = new SelectList(_context.MemberTypes, "Id", "Id", applicationUser.MemberTypeId);
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Name", applicationUser.BranchId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "ArCityName", applicationUser.CityId);
            ViewData["MemberTypeId"] = new SelectList(_context.MemberTypes, "Id", "TypeAbbreviation", applicationUser.MemberTypeId);
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ArName,EnName,SponsorId,DxnId,MemberTypeId,CityId,Gender,SecondEmail,DateofBirth,ProfileImage,IsInstructor,IsBranchManager,IsBranchEmployee,IsDriver,BranchId,IsDXNMember,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser, IFormFile myfile)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser thisUser = _userManager.FindByIdAsync(applicationUser.Id).Result;
                    thisUser.Email = applicationUser.Email;
                    thisUser.SponsorId = applicationUser.SponsorId;
                    thisUser.DxnId = applicationUser.DxnId;
                    thisUser.MemberTypeId = applicationUser.MemberTypeId;
                    thisUser.SecondEmail = applicationUser.SecondEmail;
                    thisUser.PhoneNumber = applicationUser.PhoneNumber;
                    thisUser.ArName = applicationUser.ArName;
                    thisUser.EnName = applicationUser.EnName;
                    thisUser.IsInstructor = applicationUser.IsInstructor;
                    thisUser.DateofBirth = applicationUser.DateofBirth;
                    thisUser.Gender = applicationUser.Gender;
                    thisUser.IsBranchManager = applicationUser.IsBranchManager;
                    thisUser.IsBranchEmployee = applicationUser.IsBranchEmployee;
                    thisUser.CityId = applicationUser.CityId;
                    //  thisUser.ProfileImage = applicationUser.ProfileImage;

                    thisUser.ProfileImage = await UserFile.UploadeNewImageAsync(thisUser.ProfileImage,
            myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 300, 300);

                    thisUser.IsDriver = applicationUser.IsDriver;
                    thisUser.BranchId = applicationUser.BranchId;
                    thisUser.IsDXNMember = applicationUser.IsDXNMember;
                    thisUser.EmailConfirmed = applicationUser.EmailConfirmed;
                    await _userManager.UpdateAsync(thisUser);

                    //_context.Update(applicationUser);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "ApplicationUsers", new { id });
            }
            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Name", applicationUser.BranchId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "ArCityName", applicationUser.CityId);
            ViewData["MemberTypeId"] = new SelectList(_context.MemberTypes, "Id", "TypeAbbreviation", applicationUser.MemberTypeId);


            return View(applicationUser);
        }


        public async Task<IActionResult> UpdateDXN(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == Id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateDXN(string Id, string DxnId, string EnName, string ArName, string PhoneNumber)
        {

            _context.ApplicationUser.SingleOrDefault(a => a.Id == Id).DxnId = DxnId;
            _context.ApplicationUser.SingleOrDefault(a => a.Id == Id).EnName = EnName;
            _context.ApplicationUser.SingleOrDefault(a => a.Id == Id).ArName = ArName;
            _context.ApplicationUser.SingleOrDefault(a => a.Id == Id).PhoneNumber = PhoneNumber;

            _context.SaveChanges();
            return RedirectToAction("UsersList");

        }

        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
                .Include(a => a.Branch)
                .Include(a => a.City)
                .Include(a => a.MemberType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicationUser.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ControlPanel(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
            .SingleOrDefaultAsync(m => m.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            //await _signInManager.SignInAsync(user, isPersistent: false);
            await _signInManager.SignInAsync(applicationUser, isPersistent: true);
            //if (result.IsCompletedSuccessfully)
            //{

            //  return RedirectToLocal(returnUrl);
            return RedirectToAction("Control", "Home");
            //}

            //   return View(applicationUser);
        }



        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}
