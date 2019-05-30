using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dxniraq2u2018.Data;
using dxniraq2u2018.Models;

namespace dxniraq2u2018.Controllers
{
    public class StoreLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StoreLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StoreLogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.StoreLogs.ToListAsync());
        }

        // GET: StoreLogs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeLog = await _context.StoreLogs
                .SingleOrDefaultAsync(m => m.Id == id);
            if (storeLog == null)
            {
                return NotFound();
            }

            return View(storeLog);
        }

        // GET: StoreLogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StoreLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,BranchId,Quantity,Date,Status")] StoreLog storeLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(storeLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(storeLog);
        }

        // GET: StoreLogs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeLog = await _context.StoreLogs.SingleOrDefaultAsync(m => m.Id == id);
            if (storeLog == null)
            {
                return NotFound();
            }
            return View(storeLog);
        }

        // POST: StoreLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ProductId,BranchId,Quantity,Date,Status")] StoreLog storeLog)
        {
            if (id != storeLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storeLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreLogExists(storeLog.Id))
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
            return View(storeLog);
        }

        // GET: StoreLogs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeLog = await _context.StoreLogs
                .SingleOrDefaultAsync(m => m.Id == id);
            if (storeLog == null)
            {
                return NotFound();
            }

            return View(storeLog);
        }

        // POST: StoreLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var storeLog = await _context.StoreLogs.SingleOrDefaultAsync(m => m.Id == id);
            _context.StoreLogs.Remove(storeLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreLogExists(Guid id)
        {
            return _context.StoreLogs.Any(e => e.Id == id);
        }
    }
}
