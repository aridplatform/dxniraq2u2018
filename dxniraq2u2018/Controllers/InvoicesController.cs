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
using Microsoft.AspNetCore.Http;
using dxniraq2u2018.AuxiliaryClasses;
using Microsoft.AspNetCore.Hosting;



namespace dxniraq2u2018.Controllers
{
    [Authorize]
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _environment;
        private int PagSize = 100;
        public InvoicesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: Invoices
        public async Task<IActionResult> IndexAdmin(string SearchString, int productPage = 1)
        {
            InvoiceViewModel InvoiceViewModel = new InvoiceViewModel();

            if (string.IsNullOrEmpty(SearchString))
            {
                InvoiceViewModel = new InvoiceViewModel()
                { Invoices = _context.Invoices.Where(a => a.AddressId != 1).Include(a => a.ApplicationUser).OrderByDescending(a => a.Id) };
            }
            else if (!string.IsNullOrEmpty(SearchString))
            {
                if (SearchString.All(char.IsDigit) == true)
                {
                    if (_context.Invoices.SingleOrDefault(a => a.Id == Convert.ToInt32(SearchString)) != null)
                    {
                        return RedirectToAction("Edit", "Invoices", new { id = Convert.ToInt32(SearchString) });
                    }
                    else
                    {
                        InvoiceViewModel = new InvoiceViewModel()
                        {
                            Invoices = _context.Invoices.Include(a => a.ApplicationUser).OrderByDescending(a => a.Id).Where(a => a.ApplicationUser.DxnId.Contains(SearchString) || a.ApplicationUser.Email.Contains(SearchString) || a.ApplicationUser.PhoneNumber.Contains(SearchString) || a.ApplicationUser.ArName.Contains(SearchString) || a.ApplicationUser.EnName.Contains(SearchString))
                        };
                    }
                }
                else
                {
                    InvoiceViewModel = new InvoiceViewModel()
                    {
                        Invoices = _context.Invoices.Include(a => a.ApplicationUser).OrderByDescending(a => a.Id).Where(a => a.ApplicationUser.DxnId.Contains(SearchString) || a.ApplicationUser.Email.Contains(SearchString) || a.ApplicationUser.PhoneNumber.Contains(SearchString) || a.ApplicationUser.ArName.Contains(SearchString) || a.ApplicationUser.EnName.Contains(SearchString))
                    };
                }
            }

            var count = InvoiceViewModel.Invoices.Count();
            InvoiceViewModel.Invoices = InvoiceViewModel.Invoices.OrderByDescending(p => p.Id)
                .Skip((productPage - 1) * PagSize)
                .Take(PagSize).ToList();

            InvoiceViewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PagSize,
                TotalItem = count
            };
            return View(InvoiceViewModel);
        }


        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Invoices.Where(a => a.ApplicationUserId == _userManager.GetUserId(User)).OrderByDescending(z => z.Id)
                               .Include(i => i.Accountant).Include(i => i.Address).Include(i => i.ApplicationUser).Include(i => i.Branch).Include(i => i.Driver).Include(i => i.PaymentMethod);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //  var applicationDbContext = _context.InvoiceItems .Where(m => m.ApplicationUserId == _userManager.GetUserId(User)).Include(s => s.ApplicationUser).Include(s => s.Product);
            ViewData["TotalPV"] = _context.InvoiceItems.Where(m => m.InvoiceId == id).Select(m => m.Product.PV * m.Quantity).Sum();
            ViewData["Totalprice"] = _context.InvoiceItems.Where(m => m.InvoiceId == id).Select(m => m.Product.MemberPrice * m.Quantity).Sum();
            var InvoiceViewModel = new InvoiceViewModel
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
.Where(m => m.InvoiceId == id)
            };
            if (InvoiceViewModel == null)
            {
                return NotFound();
            }

            return View(InvoiceViewModel);
        }

        public async Task<IActionResult> DetailsAdmin(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            //  var applicationDbContext = _context.InvoiceItems .Where(m => m.ApplicationUserId == _userManager.GetUserId(User)).Include(s => s.ApplicationUser).Include(s => s.Product);
            ViewData["TotalPV"] = _context.InvoiceItems.Where(m => m.InvoiceId == id).Select(m => m.Product.PV * m.Quantity).Sum();
            ViewData["Totalprice"] = _context.InvoiceItems.Where(m => m.InvoiceId == id).Select(m => m.Product.MemberPrice * m.Quantity).Sum();


            var InvoiceViewModel = new InvoiceViewModel
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
            .Where(m => m.InvoiceId == id)
            };

            if (InvoiceViewModel == null)
            {
                return NotFound();
            }
            return View(InvoiceViewModel);
        }

        public async Task<IActionResult> Print(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            //  var applicationDbContext = _context.InvoiceItems .Where(m => m.ApplicationUserId == _userManager.GetUserId(User)).Include(s => s.ApplicationUser).Include(s => s.Product);
            ViewData["TotalPV"] = _context.InvoiceItems.Where(m => m.InvoiceId == id).Select(m => m.Product.PV * m.Quantity).Sum();
            ViewData["Totalprice"] = _context.InvoiceItems.Where(m => m.InvoiceId == id).Select(m => m.Product.MemberPrice * m.Quantity).Sum();


            var InvoiceViewModel = new InvoiceViewModel
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
            .Where(m => m.InvoiceId == id)
            };

            if (InvoiceViewModel == null)
            {
                return NotFound();
            }

            return View(InvoiceViewModel);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["AccountantId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "ContactPersonName");
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Id");
            ViewData["DriverId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Id");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,ApplicationUserId,PaymentStatus,PaymentMethodId,PaymentMethodReference,AccountantId,IssuingDate,PaidDate,DriverId,DeliveredDate,IsCalled,IsloggedToDxnSystem,SystemTrackNo,AddressId,BranchId,CustomerSatisfaction,ReceivedByName,ReceivedByMobile,ReceivedByRelationshipId")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                invoice.ApplicationUserId = _userManager.GetUserId(User);
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountantId"] = new SelectList(_context.ApplicationUser, "Id", "Id", invoice.AccountantId);
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "ContactPersonName", invoice.AddressId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", invoice.ApplicationUserId);
            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Id", invoice.BranchId);
            ViewData["DriverId"] = new SelectList(_context.ApplicationUser, "Id", "Id", invoice.DriverId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Id", invoice.PaymentMethodId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.SingleOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }
            //ViewData["AccountantId"] = new SelectList(_context.ApplicationUser, "Id", "Id", invoice.AccountantId);
            ViewData["AddressId"] = new SelectList(_context.Addresses.Where(a => a.ApplicationUserId == invoice.ApplicationUserId), "Id", "AddressName", invoice.AddressId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "ArName", invoice.ApplicationUserId);
            //ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Name", invoice.BranchId);
            ViewData["DriverId"] = new SelectList(_context.ApplicationUser.Where(a => a.IsDriver == true), "Id", "ArName", invoice.DriverId);
            //ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Id", invoice.PaymentMethodId);

            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,ApplicationUserId,PaymentStatus,PaymentMethodId,PaymentMethodReference,AccountantId,IssuingDate,PaidDate,DriverId,DeliveredDate,IsCalled,IsloggedToDxnSystem,SystemTrackNo,AddressId,BranchId,CustomerSatisfaction,ReceivedByName,ReceivedByMobile,ReceivedByRelationshipId")] Invoice invoice, IFormFile myfile)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    invoice.AccountantId = _userManager.GetUserId(User);
                    invoice.BranchId = 1;
                    invoice.PaymentMethodId = 1;
                    invoice.PaidDate = invoice.DeliveredDate;


                    invoice.InvoicePDF = await UserFile.UploadeNewFileAsync(invoice.InvoicePDF,
myfile, _environment.WebRootPath, Properties.Resources.FileFolder);




                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if ((invoice.PaymentStatus) == true)
                {
                    StatementsController.CreateStatement(_context, invoice.Amount, invoice.ApplicationUserId, true, invoice.Id);
                }
                return RedirectToAction(nameof(IndexAdmin));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses.Where(a => a.ApplicationUserId == invoice.ApplicationUserId), "Id", "AddressName", invoice.AddressId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", " ArName", invoice.ApplicationUserId);
            //ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "Name", invoice.BranchId);
            ViewData["DriverId"] = new SelectList(_context.ApplicationUser.Where(a => a.IsDriver == true), "Id", "ArName", invoice.DriverId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.Where(m => m.ApplicationUserId == _userManager.GetUserId(User))
                .Include(i => i.Accountant)
                .Include(i => i.Address)
                .Include(i => i.ApplicationUser)
                .Include(i => i.Branch)
                .Include(i => i.Driver)
                .Include(i => i.PaymentMethod)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.SingleOrDefaultAsync(m => m.Id == id);

            _context.Invoices.Remove(invoice);

            await _context.SaveChangesAsync();

            var InvoiceItems = _context
              .InvoiceItems
               .Where(m => m.InvoiceId == id);
            _context.InvoiceItems.RemoveRange(InvoiceItems);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> AdminDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Accountant)
                .Include(i => i.Address)
                .Include(i => i.ApplicationUser)
                .Include(i => i.Branch)
                .Include(i => i.Driver)
                .Include(i => i.PaymentMethod)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("AdminDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.SingleOrDefaultAsync(m => m.Id == id);

            _context.Invoices.Remove(invoice);

            await _context.SaveChangesAsync();

            var InvoiceItems = _context
              .InvoiceItems
               .Where(m => m.InvoiceId == id);
            _context.InvoiceItems.RemoveRange(InvoiceItems);
            _context.SaveChanges();

            return RedirectToAction(nameof(IndexAdmin));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
