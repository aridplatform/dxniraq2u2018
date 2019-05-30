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
using dxniraq2u2018.AuxiliaryClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace dxniraq2u2018.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(ApplicationDbContext context, IHostingEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.CategoryProduct);
            return View(await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> ListItems(int Id, string sid)
        {
            //var CategoryName = _context.CategoryProducts.Where(e => e.Id == Id);
            //ViewData["Title"] = CategoryName;
            if (sid != null)
            {
                HttpContext.Session.SetString("sid", sid);
            }
            var applicationDbContext = _context.Products.Where(m => m.CategoryProductId == Id).Include(p => p.CategoryProduct);
            return View(await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Mobile(int Id, string sid)
        {
            //var CategoryName = _context.CategoryProducts.Where(e => e.Id == Id);
            //ViewData["Title"] = CategoryName;
            if (sid != null)
            {
                HttpContext.Session.SetString("sid", sid);
            }
            var applicationDbContext = _context.Products.Where(m => m.IsAvailable == true).Include(p => p.CategoryProduct);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }



            var products = await _context.Products
                .Include(p => p.CategoryProduct)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }
            var DXNID = await _context.ApplicationUser
                        .SingleOrDefaultAsync(m => m.Id == _userManager.GetUserId(User));
            if (DXNID.DxnId != null)
            {
                ViewData["DXNId"] = DXNID.DxnId;
            }
            else
            { ViewData["DXNId"] = DXNID.SponsorId; }


            //if (products.Quantity < 0)
            //{
            //    ViewData["Stock"] = 0;
            //}
            //else ViewData["Stock"] = 1;
            return View(products);
        }


        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> ProductDetails(int? id, string sid)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (sid != null)
            {
                HttpContext.Session.SetString("sid", sid);
            }

            var products = await _context.Products
                .Include(p => p.CategoryProduct)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryProductId"] = new SelectList(_context.CategoryProducts, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductNameArabic,ProductNameEnglish,ProductCode,PV,SV,DescArabic,DescEnglish,MemberPrice,CategoryProductId,IsAvailable,Image,Weight,NonMemberPrice,BarCode")] Product products, IFormFile myfile)
        {
            if (ModelState.IsValid)
            {
                products.Image = await UserFile.UploadeNewImageAsync(products.Image,
              myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 300, 300);
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryProductId"] = new SelectList(_context.CategoryProducts, "Id", "Name", products.CategoryProductId);
            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["CategoryProductId"] = new SelectList(_context.CategoryProducts, "Id", "Name", products.CategoryProductId);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductNameArabic,ProductNameEnglish,ProductCode,PV,SV,DescArabic,DescEnglish,MemberPrice,CategoryProductId,IsAvailable,Image,Weight,NonMemberPrice,BarCode")] Product products, IFormFile myfile)
        {
            if (id != products.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    products.Image = await UserFile.UploadeNewImageAsync(products.Image,
            myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 300, 300);

                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
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
            ViewData["CategoryProductId"] = new SelectList(_context.CategoryProducts, "Id", "Name", products.CategoryProductId);
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.CategoryProduct)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Purchase(int id, string DxnId, int quantity)
        {


            Controllers.ShoppingCartItemsController.AddItemToshoppingCart(_context, id, DxnId, quantity, _userManager.GetUserId(User));

            var Item =
        _context.Products.Where(m => m.Id == id);

            //Item.FirstOrDefault().Quantity = (Item.FirstOrDefault().Quantity) - quantity;
            _context.SaveChanges();

            // Redirect("ShoppingCarts");
            return RedirectToAction("Index", "ShoppingCartItems");

        }
    }
}
