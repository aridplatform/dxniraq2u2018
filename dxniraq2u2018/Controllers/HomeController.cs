using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dxniraq2u2018.Models;
using dxniraq2u2018.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace dxniraq2u2018.Controllers
{

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //[Authorize]
        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                BlogPost = _context.BlogPosts.Include(b => b.ApplicationUser).Include(b => b.BlogCategory).OrderByDescending(a => a.Id).Take(6),
                ApplicationUser = _context.ApplicationUser
                .Where(m => m.ProfileImage != null)
                .Include(c => c.City).OrderByDescending(a => a.RegDate).Take(4),
                Product = _context.Products.Where(a => a.IsAvailable == true).Take(8)
                //Testimonial = _context.Testimonials.Include(e => e.ApplicationUser).Take(3)

            };

            //var applicationDbContext = _context.BlogPosts.Include(b => b.ApplicationUser).Include(b => b.BlogCategory);
            return View(homeViewModel);
        }

        public IActionResult English()
        {
            var homeViewModel = new HomeViewModel
            {
                BlogPost = _context.BlogPosts.Include(b => b.ApplicationUser).Include(b => b.BlogCategory).OrderByDescending(a => a.Id).Take(6),
                ApplicationUser = _context.ApplicationUser
                .Where(m => m.ProfileImage != null)
                .Include(c => c.City).OrderByDescending(a => a.RegDate).Take(4),
                Product = _context.Products.Where(a => a.IsAvailable == true).Take(8)
                //Testimonial = _context.Testimonials.Include(e => e.ApplicationUser).Take(3)

            };

            //var applicationDbContext = _context.BlogPosts.Include(b => b.ApplicationUser).Include(b => b.BlogCategory);
            return View(homeViewModel);
        }

        public IActionResult Mobile()
        {
            //return RedirectToAction("Mobile", "Products");
            ViewData["UserAgent"] = Request.Headers["User-Agent"].ToString();
            if (Request.Headers["User-Agent"].ToString() == "true")
            {
                //Mobile Browser Detected
            }
            else
            {
                //Desktop Browser Detected
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Control()
        {
            var currentuser = await _userManager.GetUserAsync(User);
            if (currentuser.ArName == null)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (_context.Invoices.Where(a => a.AddressId == 1 && a.ApplicationUserId ==_userManager.GetUserId(User)).Count() > 0)
            {
                return RedirectToAction("Index", "Invoices");
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            LecturesController.SendEmailAsync();
            return View();
        }


        public IActionResult PrivacyPolicy()
        {


            return View();
        }



        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
