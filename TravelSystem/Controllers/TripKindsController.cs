using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelSystem;
using TravelSystem.Models;

namespace TravelSystem.Controllers
{
    public class TripKindsController : Controller
    {
        private readonly AppDbContext _context;

        public TripKindsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TripKinds
        public async Task<IActionResult> Index()
        {
            return View(await _context.TripKinds.ToListAsync());
        }

        // GET: TripKinds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tripKind = await _context.TripKinds
                .FirstOrDefaultAsync(m => m.TripKindId == id);
            if (tripKind == null)
            {
                return NotFound();
            }

            return View(tripKind);
        }

        // GET: TripKinds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TripKinds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TripKindId,Name")] TripKind tripKind)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tripKind);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tripKind);
        }

        // GET: TripKinds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tripKind = await _context.TripKinds.FindAsync(id);
            if (tripKind == null)
            {
                return NotFound();
            }
            return View(tripKind);
        }

        // POST: TripKinds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TripKindId,Name")] TripKind tripKind)
        {
            if (id != tripKind.TripKindId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tripKind);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripKindExists(tripKind.TripKindId))
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
            return View(tripKind);
        }

        // GET: TripKinds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tripKind = await _context.TripKinds
                .FirstOrDefaultAsync(m => m.TripKindId == id);
            if (tripKind == null)
            {
                return NotFound();
            }

            return View(tripKind);
        }

        // POST: TripKinds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tripKind = await _context.TripKinds.FindAsync(id);
            if (tripKind != null)
            {
                _context.TripKinds.Remove(tripKind);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripKindExists(int id)
        {
            return _context.TripKinds.Any(e => e.TripKindId == id);
        }
    }
}
