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

namespace dxniraq2u2018.Controllers
{
    [Authorize]
    [Route("Cart/[action]")]
    public class ShoppingCartItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartItemsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> RemoveFromCart(Guid Id)
        {
            var shoppingCartItem =
                    _context.ShoppingCartItems.SingleOrDefault(
                        s => s.Id == Id & s.ApplicationUserId == _userManager.GetUserId(User));

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Quantity > 1)
                {
                    shoppingCartItem.Quantity--;
                    localAmount = shoppingCartItem.Quantity;

                    //               var Item =
                    //_context.Products.Where(m => m.Id == shoppingCartItem.ProductId);

                    //               Item.FirstOrDefault().Quantity = (Item.FirstOrDefault().Quantity) + 1;
                    //               _context.SaveChanges();
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            //return localAmount;
        }


        public async Task<IActionResult> AddToCart(Guid Id)
        {
            var shoppingCartItem =
                     _context.ShoppingCartItems.SingleOrDefault(
                        s => s.Id == Id & s.ApplicationUserId == _userManager.GetUserId(User));

            if (shoppingCartItem == null)
            {

                return RedirectToAction(nameof(Index));

            }
            else
            {
                shoppingCartItem.Quantity++;
                //                var Item =
                //_context.Products.Where(m => m.Id == shoppingCartItem.ProductId);

                //                Item.FirstOrDefault().Quantity = (Item.FirstOrDefault().Quantity) - 1;
                //                _context.SaveChanges();
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ClearCart()
        {
            var cartItems = _context
                                .ShoppingCartItems
                                 .Where(m => m.ApplicationUserId == _userManager.GetUserId(User));
            //.Where(cart => cart.ShoppingCartId == ShoppingCartId);

            // ReturnToStore();

            _context.ShoppingCartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public void ReturnToStore()
        {
            var cartItems = _context
              .ShoppingCartItems;
            foreach (ShoppingCartItem ShoppingCartItem in cartItems)
            {
                //var Item = _context.Products.Where(m => m.Id == ShoppingCartItem.ProductId);
                //Item.FirstOrDefault().Quantity = (Item.FirstOrDefault().Quantity) + ShoppingCartItem.Quantity;

                //_context.SaveChangesAsync();
            }
        }

        // GET: ShoppingCartItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ShoppingCartItems.Where(m => m.ApplicationUserId == _userManager.GetUserId(User)).Include(s => s.ApplicationUser).Include(s => s.Product);

            //var x = _context.ShoppingCartItems.Include(a => a.ApplicationUser).Include(a => a.Product).Sum(m => m.TotalWeight * m.Quantity);
            //ViewData["Totalweight"] = x;
            //ViewData["Totalprice"] = _context.ShoppingCartItems.Include(a => a.ApplicationUser).Include(a => a.Product).Sum(m => m.SalePrice);
            // ViewData["TotalSV"] = _context.ShoppingCartItems.Include(a => a.ApplicationUser).Include(a => a.Product).Sum(m => m.TotalSV * m.Quantity);

            // ViewData["TotalPV"] = _context.ShoppingCartItems.Include(a => a.ApplicationUser).Include(a => a.Product).(m => m.TotalPV).Sum;

            ViewData["TotalPV"] = _context.ShoppingCartItems.Where(m => m.ApplicationUserId == _userManager.GetUserId(User)).Select(m => m.Product.PV * m.Quantity).Sum();
            ViewData["Totalprice"] = _context.ShoppingCartItems.Where(m => m.ApplicationUserId == _userManager.GetUserId(User)).Select(m => m.Product.MemberPrice * m.Quantity).Sum();

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ShoppingCartItems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartItem = await _context.ShoppingCartItems
                .Include(s => s.ApplicationUser)
                .Include(s => s.Product)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (shoppingCartItem == null)
            {
                return NotFound();
            }

            return View(shoppingCartItem);
        }

        // GET: ShoppingCartItems/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductCode");
            return View();
        }

        // POST: ShoppingCartItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,ApplicationUserId,DXNId,Quantity")] ShoppingCartItem shoppingCartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", shoppingCartItem.ApplicationUserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductCode", shoppingCartItem.ProductId);
            return View(shoppingCartItem);
        }

        // GET: ShoppingCartItems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartItem = await _context.ShoppingCartItems.SingleOrDefaultAsync(m => m.Id == id);
            if (shoppingCartItem == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", shoppingCartItem.ApplicationUserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductCode", shoppingCartItem.ProductId);
            return View(shoppingCartItem);
        }

        // POST: ShoppingCartItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ProductId,ApplicationUserId,DXNId,Quantity")] ShoppingCartItem shoppingCartItem)
        {
            if (id != shoppingCartItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartItemExists(shoppingCartItem.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", shoppingCartItem.ApplicationUserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductCode", shoppingCartItem.ProductId);
            return View(shoppingCartItem);
        }

        // GET: ShoppingCartItems/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartItem = await _context.ShoppingCartItems
                 .Where(m => m.ApplicationUserId == _userManager.GetUserId(User))
                .Include(s => s.ApplicationUser)
                .Include(s => s.Product)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (shoppingCartItem == null)
            {
                return NotFound();
            }

            return View(shoppingCartItem);
        }

        // POST: ShoppingCartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var shoppingCartItem = await _context.ShoppingCartItems.SingleOrDefaultAsync(m => m.Id == id);
            _context.ShoppingCartItems.Remove(shoppingCartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartItemExists(Guid id)
        {
            return _context.ShoppingCartItems.Any(e => e.Id == id);
        }



        //public async Task<IActionResult> Checkout2Address()
        //      {
        //          return RedirectToAction("ShipToAddress", "Addresses");
        //      }



        public async Task<IActionResult> Checkout()
        {

            var shoppingCartItem =
                   _context.ShoppingCartItems.Where(m => m.ApplicationUserId == _userManager.GetUserId(User)).Any();

            var Address =
                 _context.Addresses.FirstOrDefault(s => s.ApplicationUserId == _userManager.GetUserId(User));

            var Amount = _context.ShoppingCartItems.Where(m => m.ApplicationUserId == _userManager.GetUserId(User)).Select(m => m.Product.MemberPrice * m.Quantity).Sum();
            if (Address == null)
            { return RedirectToAction(nameof(Create), "Addresses"); }

            if (shoppingCartItem != false)
            {
                Invoice Invoice = new Invoice();
                Invoice.ApplicationUserId = _userManager.GetUserId(User);
                //   Invoice.AccountantId = _userManager.GetUserId(User);
                Invoice.Amount = Amount;
                //  Invoice.DeliveredDate = DateTime.Today.Date;
                //  Invoice.DriverId = _userManager.GetUserId(User);
                Invoice.IsCalled = false;
                Invoice.IsloggedToDxnSystem = false;
                Invoice.IssuingDate = DateTime.Today.Date;
                // Invoice.PaidDate = DateTime.Today.Date;
                Invoice.PaymentStatus = false;
                Invoice.PaymentMethodId = 1;
                //Invoice.AddressId = Address.Id;
                Invoice.AddressId = 1;
                Invoice.BranchId = 1;
                //Invoice.SystemTrackNo = ViewData["Address"];
                _context.Add(Invoice);
                await _context.SaveChangesAsync();
                int Invid = Invoice.Id; // Yes it's here
                var ShoppingCartItem = _context.ShoppingCartItems.Where(m => m.ApplicationUserId == _userManager.GetUserId(User));
                //StatementsController.CreateStatement(_context, Amount, _userManager.GetUserId(User), false, Invid);

                foreach (ShoppingCartItem sc in ShoppingCartItem)
                {
                    InvoiceItem invoiceItems = new InvoiceItem();
                    invoiceItems.ApplicationUserId = _userManager.GetUserId(User);
                    invoiceItems.DXNId = sc.DXNId;
                    invoiceItems.ProductId = sc.ProductId;
                    invoiceItems.Quantity = sc.Quantity;
                    //invoiceItems.Price  = sc.Price;
                    //invoiceItems.TotalPV = sc.TotalPV;
                    //invoiceItems.TotalSV = sc.TotalSV;
                    //invoiceItems.TotalWeight = sc.TotalWeight;
                    invoiceItems.InvoiceId = Invid;
                    _context.Add(invoiceItems);
                }

                await _context.SaveChangesAsync();

                var cartItems = _context
                .ShoppingCartItems
                 .Where(m => m.ApplicationUserId == _userManager.GetUserId(User));
                //.Where(cart => cart.ShoppingCartId == ShoppingCartId);
                // ReturnToStore();

                _context.ShoppingCartItems.RemoveRange(cartItems);
                _context.SaveChanges();
                return RedirectToAction("ShipToAddress", "Addresses", new { @id = Invid }); //https://stackoverflow.com/questions/1257482/redirecttoaction-with-parameter
            }

            else
            { ViewData["Status"] = "سلة التسوق فارغة وعليه لايمكن تقديم طلب الشراء"; }

            return RedirectToAction(nameof(Index));

            //-------------------------------

            //var cartitems = new ShoppingCarts() = _context.ShoppingCarts;
            //.Where (m => m.Id == 1);
            //cartitems. = Invid;
            //cartitems.Isprocessed = true;


            //_context.ShoppingCarts.Update (shoppingCarts);
            //await _context.SaveChangesAsync();

            //ShoppingCarts shoppingCarts = new ShoppingCarts();

        }


        public static void AddItemToshoppingCart(ApplicationDbContext _context, int id, string DxnId, int quantity, string userId)
        {
            var products = _context.Products.SingleOrDefault(m => m.Id == id);
            var ShoppingCartItem = _context.ShoppingCartItems.Where(m => m.ProductId == id && m.ApplicationUserId == userId && m.DXNId == DxnId).Count();
            if (ShoppingCartItem == 0)
            {
                ShoppingCartItem shoppingCarts = new ShoppingCartItem
                {
                    ApplicationUserId = userId,
                    DXNId = DxnId,
                    ProductId = id,
                    Quantity = quantity,
                    //TotalPrice = (products.Price) * quantity,
                    //TotalPV = (products.PV) * quantity,
                    //TotalSV = (products.SV) * quantity,
                    //TotalWeight = (products.Weight) * quantity
                };
                _context.Add(shoppingCarts);
                _context.SaveChanges();
            }
            else
            {
                var shoppingCartExistItem =
         _context.ShoppingCartItems.Where(m => m.ProductId == id && m.ApplicationUserId == userId && m.DXNId == DxnId);

                shoppingCartExistItem.FirstOrDefault().Quantity = (shoppingCartExistItem.FirstOrDefault().Quantity) + quantity;
                _context.SaveChanges();
            }
        }

    }
}
