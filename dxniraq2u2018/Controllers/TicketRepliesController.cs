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

namespace dxniraq2u2018.Controllers
{
    public class TicketRepliesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public TicketRepliesController(ApplicationDbContext context, UserManager<ApplicationUser> userMrg, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userMrg;
            _signInManager = signInManager;
        }

        // GET: TicketReplies
        public async Task<IActionResult> Index(int? tid)
        {
            var applicationDbContext = _context.TicketReply.Include(t => t.SupportUser).Where(a => a.TicketId == tid).OrderBy(a => a.Id);
            ViewData["Subject"] = _context.Ticket.SingleOrDefault(a => a.Id == tid).Subject;
            ViewData["Body"] = _context.Ticket.SingleOrDefault(a => a.Id == tid).Body;
            ViewData["Id"] = tid;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TicketReplies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketReply = await _context.TicketReply
                .Include(t => t.SupportUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ticketReply == null)
            {
                return NotFound();
            }

            return View(ticketReply);
        }

        // GET: TicketReplies/Create
        public IActionResult Create(int tid)
        {
            ViewData["TicketId"] = new SelectList(_context.Set<Ticket>().Where(a => a.Id == tid).OrderByDescending(a => a.Id), "Id", "Subject");
            return View();
        }

        // POST: TicketReplies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int tid, [Bind("Id,Body,SupportUserId,TicketId,ReplyDate")] TicketReply ticketReply)
        {
            if (ModelState.IsValid)
            {

                var ticket = await _context.Ticket
                         .SingleOrDefaultAsync(m => m.Id == ticketReply.TicketId);
                if (User.IsInRole("Admins"))
                {
                                      ticket.Status = false;
                    _context.Update(ticket);
                }

                else
                {
                    ticket.Status = true;
                    _context.Update(ticket);
                }
                ticketReply.ReplyDate = DateTime.Now;
                ticketReply.SupportUserId = _userManager.GetUserId(User);

                _context.Add(ticketReply);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Tickets");
            }
            ViewData["TicketId"] = new SelectList(_context.Set<Ticket>().Where(a => a.Id == tid).OrderByDescending(a => a.Id), "Id", "Subject");
            return View(ticketReply);
        }

        // GET: TicketReplies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketReply = await _context.TicketReply.SingleOrDefaultAsync(m => m.Id == id);
            if (ticketReply == null)
            {
                return NotFound();
            }
            ViewData["SupportUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", ticketReply.SupportUserId);
            return View(ticketReply);
        }

        // POST: TicketReplies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Body,SupportUserId,TicketId,ReplyDate")] TicketReply ticketReply)
        {
            if (id != ticketReply.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketReply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketReplyExists(ticketReply.Id))
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
            ViewData["SupportUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", ticketReply.SupportUserId);
            return View(ticketReply);
        }

        // GET: TicketReplies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketReply = await _context.TicketReply
                .Include(t => t.SupportUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ticketReply == null)
            {
                return NotFound();
            }

            return View(ticketReply);
        }

        // POST: TicketReplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketReply = await _context.TicketReply.SingleOrDefaultAsync(m => m.Id == id);
            _context.TicketReply.Remove(ticketReply);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketReplyExists(int id)
        {
            return _context.TicketReply.Any(e => e.Id == id);
        }
    }
}
