using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dxniraq2u2018.Data;
using dxniraq2u2018.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using dxniraq2u2018.AuxiliaryClasses;

namespace dxniraq2u2018.Controllers
{
    public class MemberTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;
      

        public MemberTypesController(ApplicationDbContext context,IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: MemberTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.MemberTypes.ToListAsync());
        }

        // GET: MemberTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberType = await _context.MemberTypes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (memberType == null)
            {
                return NotFound();
            }

            return View(memberType);
        }

        // GET: MemberTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MemberTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameAr,NameEng,TypeAbbreviation,Description,Image")] MemberType memberType,IFormFile myfile)
        {
            if (ModelState.IsValid)
            {
                memberType.Image = await UserFile.UploadeNewImageAsync(memberType.Image,
                myfile, _environment.WebRootPath, Properties.Resources.imgFolder, 300, 300);

                _context.Add(memberType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(memberType);
        }

        // GET: MemberTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberType = await _context.MemberTypes.SingleOrDefaultAsync(m => m.Id == id);
            if (memberType == null)
            {
                return NotFound();
            }
            return View(memberType);
        }

        // POST: MemberTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameAr,NameEng,TypeAbbreviation,Description,Image")] MemberType memberType)
        {
            if (id != memberType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberTypeExists(memberType.Id))
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
            return View(memberType);
        }

        // GET: MemberTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberType = await _context.MemberTypes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (memberType == null)
            {
                return NotFound();
            }

            return View(memberType);
        }

        // POST: MemberTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memberType = await _context.MemberTypes.SingleOrDefaultAsync(m => m.Id == id);
            _context.MemberTypes.Remove(memberType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberTypeExists(int id)
        {
            return _context.MemberTypes.Any(e => e.Id == id);
        }
    }
}
