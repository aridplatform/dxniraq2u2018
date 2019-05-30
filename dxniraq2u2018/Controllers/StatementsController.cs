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
    public class StatementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatementsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public static void CreateStatement(ApplicationDbContext context, decimal Amount, string userId, bool Destination, int InvoiceId)
        {
            if (!HasBeenCreated(context, InvoiceId, Destination))
            {
                context.Statements.Add(
    new Statement
    {
        Amount = Amount,
        ApplicationUserId = userId,
        Date = DateTime.Today.Date,
        Destination = Destination,
        InvoiceId = InvoiceId
    });
            }
            context.SaveChangesAsync();
        }

        public static bool HasBeenCreated(ApplicationDbContext context, int InvoiceId, bool Destination)
        {
            int statementItem = context.Statements
                .Where(m => m.InvoiceId == InvoiceId && m.Destination == Destination).Count();

            if (statementItem == 0)
            {
                return false;
            }
            return true;
        }


        // GET: Statements
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Statements
                 .Where(m => m.ApplicationUserId == _userManager.GetUserId(User))
                .Include(s => s.ApplicationUser)
                .Include(s => s.Invoice);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Statements/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statement = await _context.Statements
                .Include(s => s.ApplicationUser)
                .Include(s => s.Invoice)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (statement == null)
            {
                return NotFound();
            }

            return View(statement);
        }

        // GET: Statements/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id");
            return View();
        }

        // POST: Statements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,InvoiceId,Amount,Destination,ApplicationUserId")] Statement statement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", statement.ApplicationUserId);
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", statement.InvoiceId);
            return View(statement);
        }

        // GET: Statements/Edit/5

        // POST: Statements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Date,InvoiceId,Amount,Destination,ApplicationUserId")] Statement statement)
        {
            if (id != statement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatementExists(statement.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", statement.ApplicationUserId);
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", statement.InvoiceId);
            return View(statement);
        }

        // GET: Statements/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statement = await _context.Statements
                .Include(s => s.ApplicationUser)
                .Include(s => s.Invoice)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (statement == null)
            {
                return NotFound();
            }

            return View(statement);
        }

        // POST: Statements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var statement = await _context.Statements.SingleOrDefaultAsync(m => m.Id == id);
            _context.Statements.Remove(statement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatementExists(Guid id)
        {
            return _context.Statements.Any(e => e.Id == id);
        }
    }
}
