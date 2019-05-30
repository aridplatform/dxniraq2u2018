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
using System.Data.SqlClient;

namespace dxniraq2u2018.Controllers
{
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddressesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Addresses
                .Where(m => m.ApplicationUserId == _userManager.GetUserId(User))
                .Include(a => a.ApplicationUser).Include(a => a.City);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> ShipToAddress(int id)
        {
            //var applicationDbContext = _context.Addresses
            //    .Where(m => m.ApplicationUserId == _userManager.GetUserId(User))
            //    .Include(a => a.ApplicationUser).Include(a => a.City);

        

            var AddressViewModel = new AddressViewModel
            {
                Invoice = await _context.Invoices
              .Include(i => i.Accountant)
              .Include(i => i.Address)
               .Include(i => i.Address.City)
              .Include(i => i.ApplicationUser)
              .Include(i => i.Branch)
              .Include(i => i.Driver)
              .Include(i => i.PaymentMethod)
              .SingleOrDefaultAsync(m => m.Id == id),

                InvoiceItems = _context.InvoiceItems
          .Include(a => a.Product)
          .Where(m => m.InvoiceId == id),

                Addresses =   _context.Addresses
                .Where(m => m.ApplicationUserId == _userManager.GetUserId(User))
                        };

            if (AddressViewModel == null)
            {
                return NotFound();
            }

            return View(AddressViewModel);

            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                 .Where(m => m.ApplicationUserId == _userManager.GetUserId(User))
                .Include(a => a.ApplicationUser)
                .Include(a => a.City)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create()
        {
            //  ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Arname");
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "ArCityName");
                     return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CityId,FullAddress,ContactPersonName,Email,Mobile,UserComment,AlertMe,ApplicationUserId,AddressName")] Address address)
        {
            if (ModelState.IsValid)
            {
                address.Email = "";
                address.ApplicationUserId = _userManager.GetUserId(User);
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ShoppingCartItems");
            }
            //  ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "ArName", address.ApplicationUserId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "ArCityName", address.CityId);
            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                 .Where(m => m.ApplicationUserId == _userManager.GetUserId(User))
                .SingleOrDefaultAsync(m => m.Id == id);

            if (address == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "ArName", address.ApplicationUserId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "ArCityName", address.CityId);
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CityId,FullAddress,ContactPersonName,Email,Mobile,UserComment,AlertMe,ApplicationUserId,AddressName")] Address address)
        {
            if (id != address.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    address.Email = "";
                    address.ApplicationUserId = _userManager.GetUserId(User);
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.Id))
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
            //     ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "ArName", address.ApplicationUserId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "ArCityName", address.CityId);
            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                 .Where(m => m.ApplicationUserId == _userManager.GetUserId(User))
                .Include(a => a.ApplicationUser)
                .Include(a => a.City)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.Addresses.SingleOrDefaultAsync(m => m.Id == id);
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //[HttpPost]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Ship(int aid, int id)
        {
            var address = await _context.Addresses.SingleOrDefaultAsync(m => m.Id == id);
            //_context.Addresses.Remove(address);
            //await _context.SaveChangesAsync();

            // Execute Stored procedure in the database (Delete, Update)
            var con = _context.Database.GetDbConnection();
            con.Open();
            var comm = con.CreateCommand();
            comm.CommandText = "UpdateInvoiceAddressId"; //Stored procedure name
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.Parameters.Add(new SqlParameter("AddressId", aid));
            comm.Parameters.Add(new SqlParameter("Id", id));
            comm.ExecuteNonQuery();
            con.Close();
            // End of SP code


            //if (address.ApplicationUser.PhoneNumber != null)
            //{
            //  SendSms(address.ApplicationUser.PhoneNumber, "تم استلام طلبيتكم وسيتم اعلامكم حين الشحن. لاي استفسار الرجاء الاتصال بالرقم 009647710311083");
            //}

            return RedirectToAction("Details", "Invoices", new { @id = id });
        }



        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
