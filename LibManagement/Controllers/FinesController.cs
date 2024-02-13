using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibManagement.Data;
using LibManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace LibManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FinesController : Controller
    {
        private readonly LibManagementContext _context;

        public FinesController(LibManagementContext context)
        {
            _context = context;
        }

        // GET: Fines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fine.ToListAsync());
        }

        // GET: Fines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fine = await _context.Fine.FindAsync(id);
            if (fine == null)
            {
                return NotFound();
            }
            return View(fine);
        }

        // POST: Fines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LateDayRate,MaxFine")] Fine fine)
        {
            if (id != fine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FineExists(fine.Id))
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
            return View(fine);
        }

        private bool FineExists(int id)
        {
            return _context.Fine.Any(e => e.Id == id);
        }
    }
}
