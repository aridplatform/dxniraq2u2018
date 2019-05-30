using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dxniraq2u2018.Data;
using dxniraq2u2018.Models;
using System.Threading;

namespace dxniraq2u2018.Controllers
{
    public class BranchAdvertismentScreenksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BranchAdvertismentScreenksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BranchAdvertismentScreenks
        public async Task<IActionResult> Index()
        {
            return View(await _context.BranchAdvertismentScreenk.ToListAsync());
        }

        // GET: BranchAdvertismentScreenks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branchAdvertismentScreenk = await _context.BranchAdvertismentScreenk
                .SingleOrDefaultAsync(m => m.Id == id);
            if (branchAdvertismentScreenk == null)
            {
                return NotFound();
            }

            return View(branchAdvertismentScreenk);
        }

        public async Task<IActionResult> Display()
        {
            int total = _context.BranchAdvertismentScreenk.Where(a => a.Status == true).Count();
            Random r = new Random();
            int offset = r.Next(0, total);

            var result =  _context.BranchAdvertismentScreenk.Where(a => a.Status == true).Skip(offset).FirstOrDefault();

           // var branchAdvertismentScreenk = await _context.BranchAdvertismentScreenk.Where(a=>a.Status == true).First();

            return View(result);

            //foreach (var item in branchAdvertismentScreenk)
            //{


              //  await Task.Delay(6000);

                //RedirectToAction("Display", "BranchAdvertismentScreenks");
                //return View(item);
                //Task.Delay(2000);

            //}
          
              
            //return View(branchAdvertismentScreenk);
        }

        // GET: BranchAdvertismentScreenks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BranchAdvertismentScreenks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,Timer,Subject,Body,PostDate")] BranchAdvertismentScreenk branchAdvertismentScreenk)
        {
            if (ModelState.IsValid)
            {
                branchAdvertismentScreenk.Status = true;
                branchAdvertismentScreenk.Timer = 0;
                branchAdvertismentScreenk.PostDate = DateTime.Now;

                _context.Add(branchAdvertismentScreenk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(branchAdvertismentScreenk);
        }

        // GET: BranchAdvertismentScreenks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branchAdvertismentScreenk = await _context.BranchAdvertismentScreenk.SingleOrDefaultAsync(m => m.Id == id);
            if (branchAdvertismentScreenk == null)
            {
                return NotFound();
            }
            return View(branchAdvertismentScreenk);
        }

        // POST: BranchAdvertismentScreenks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,Timer,Subject,Body,PostDate")] BranchAdvertismentScreenk branchAdvertismentScreenk)
        {
            if (id != branchAdvertismentScreenk.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(branchAdvertismentScreenk);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchAdvertismentScreenkExists(branchAdvertismentScreenk.Id))
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
            return View(branchAdvertismentScreenk);
        }

        // GET: BranchAdvertismentScreenks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branchAdvertismentScreenk = await _context.BranchAdvertismentScreenk
                .SingleOrDefaultAsync(m => m.Id == id);
            if (branchAdvertismentScreenk == null)
            {
                return NotFound();
            }

            return View(branchAdvertismentScreenk);
        }

        // POST: BranchAdvertismentScreenks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branchAdvertismentScreenk = await _context.BranchAdvertismentScreenk.SingleOrDefaultAsync(m => m.Id == id);
            _context.BranchAdvertismentScreenk.Remove(branchAdvertismentScreenk);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchAdvertismentScreenkExists(int id)
        {
            return _context.BranchAdvertismentScreenk.Any(e => e.Id == id);
        }
    }
}
