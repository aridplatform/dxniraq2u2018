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
    public class InvoiceItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
         

        public InvoiceItemsController(ApplicationDbContext context)
        {
            _context = context;
            
        }

        // GET: InvoiceItems
        public async Task<IActionResult> Index(int? Id)
        {
            var applicationDbContext = _context.InvoiceItems
                .Where(i=>i.InvoiceId == Id)
                .Include(i => i.ApplicationUser).Include(i => i.Invoice).Include(i => i.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: InvoiceItems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceItem = await _context.InvoiceItems
                .Include(i => i.ApplicationUser)
                .Include(i => i.Invoice)
                .Include(i => i.Product)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (invoiceItem == null)
            {
                return NotFound();
            }

            return View(invoiceItem);
        }

        // GET: InvoiceItems/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductCode");
            return View();
        }

        // POST: InvoiceItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,ApplicationUserId,Quantity,DXNId,InvoiceId")] InvoiceItem invoiceItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoiceItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", invoiceItem.ApplicationUserId);
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", invoiceItem.InvoiceId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductCode", invoiceItem.ProductId);
            return View(invoiceItem);
        }

        // GET: InvoiceItems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceItem = await _context.InvoiceItems.SingleOrDefaultAsync(m => m.Id == id);
            if (invoiceItem == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", invoiceItem.ApplicationUserId);
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", invoiceItem.InvoiceId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductCode", invoiceItem.ProductId);
            return View(invoiceItem);
        }

        // POST: InvoiceItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ProductId,ApplicationUserId,Quantity,DXNId,InvoiceId")] InvoiceItem invoiceItem)
        {
            if (id != invoiceItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoiceItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceItemExists(invoiceItem.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", invoiceItem.ApplicationUserId);
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", invoiceItem.InvoiceId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductCode", invoiceItem.ProductId);
            return View(invoiceItem);
        }

        // GET: InvoiceItems/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceItem = await _context.InvoiceItems
                .Include(i => i.ApplicationUser)
                .Include(i => i.Invoice)
                .Include(i => i.Product)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (invoiceItem == null)
            {
                return NotFound();
            }

            return View(invoiceItem);
        }

        // POST: InvoiceItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var invoiceItem = await _context.InvoiceItems.SingleOrDefaultAsync(m => m.Id == id);
            _context.InvoiceItems.Remove(invoiceItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceItemExists(Guid id)
        {
            return _context.InvoiceItems.Any(e => e.Id == id);
        }
    }
}
