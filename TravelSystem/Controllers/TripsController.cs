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
    public class TripsController : Controller
    {
        private readonly AppDbContext _context;

        public TripsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Trips.Include(t => t.Driver).Include(t => t.TripKind).Include(t => t.Bus);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Trips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .Include(t => t.Driver)
                .Include(t => t.TripKind)
                .Include(t => t.Bus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // GET: Trips/Create
        public IActionResult Create()
        {
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "FullName");
            ViewData["TripKindId"] = new SelectList(_context.TripKinds, "TripKindId", "Name");
            ViewData["BusId"] = new SelectList(_context.Buss, "Bus_Plate", "Bus_Plate");
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BusId,DriverId,TripKindId,AvailableSeats,ReservedSeats,TripDate")] Trip trip)
        {
            //can not make old date on the trap
            if (trip.TripDate < DateTime.Now)
            {
                ModelState.AddModelError(nameof(trip.TripDate), "Trip Date must not be a past date");
            }
            #region    //can not assign packed driver
            var driverTripsDates = await _context.Trips.Where(a => a.DriverId == trip.DriverId
            && a.TripDate.Date >= DateTime.Today)
                .Select(a =>

                    a.TripDate
                )
                .ToListAsync();
            foreach (var driverTrip in driverTripsDates)
            {
                if (trip.TripDate.Date == driverTrip.Date)
                {
                    ModelState.AddModelError(nameof(trip.DriverId), "The driver is busy in this Date");
                    break;
                }
            }
            #endregion
            #region    //can not assign packed bus
            var busTripsDates = await _context.Trips.Where(a => a.BusId == trip.BusId
            && a.TripDate.Date >= DateTime.Today)
                .Select(a =>

                    a.TripDate
                )
                .ToListAsync();
            foreach (var busTrip in busTripsDates)
            {
                if (trip.TripDate.Date == busTrip.Date)
                {
                    ModelState.AddModelError(nameof(trip.DriverId), "The bus is busy in this Date");
                }
            }
            #endregion
            if (ModelState.IsValid)
            {
                _context.Add(trip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "FullName", trip.DriverId);
            ViewData["TripKindId"] = new SelectList(_context.TripKinds, "TripKindId", "Name", trip.TripKindId);
            ViewData["BusId"] = new SelectList(_context.Buss, "Bus_Plate", "Bus_Plate", trip.BusId);
            return View(trip);
        }

        // GET: Trips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "FullName", trip.DriverId);
            ViewData["TripKindId"] = new SelectList(_context.TripKinds, "TripKindId", "Name", trip.TripKindId);
            ViewData["BusId"] = new SelectList(_context.Buss, "Bus_Plate", "Bus_Plate", trip.BusId);

            return View(trip);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BusId,DriverId,TripKindId,AvailableSeats,ReservedSeats,TripDate")] Trip trip)
        {
            if (id != trip.Id)
            {
                return NotFound();
            }
            #region //can not make old date on the trap
            if (trip.TripDate < DateTime.Now)
            {
                ModelState.AddModelError(nameof(trip.TripDate), "Trip Date must not be a past date");
            }
            #endregion
            #region    //can not assign packed driver
            var driverTripsDates = await _context.Trips.Where(a=>a.DriverId ==  trip.DriverId 
            && a.TripDate.Date >= DateTime.Today && a.Id != id)
                .Select(a => 
                
                    a.TripDate
                )
                .ToListAsync();
            foreach (var driverTrip in driverTripsDates)
            {
                if (trip.TripDate.Date == driverTrip.Date)
                {
                    ModelState.AddModelError(nameof(trip.DriverId), "The driver is busy in this Date");
                    break;
                }
            }
            #endregion
            #region    //can not assign packed bus
            var busTripsDates = await _context.Trips.Where(a=>a.BusId ==  trip.BusId 
            && a.TripDate.Date >= DateTime.Today && a.Id != id)
                .Select(a => 
                
                    a.TripDate
                )
                .ToListAsync();
            foreach (var busTrip in busTripsDates)
            {
                if (trip.TripDate.Date == busTrip.Date)
                {
                    ModelState.AddModelError(nameof(trip.DriverId), "The bus is busy in this Date");
                }
            }
            #endregion
           
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(trip.Id))
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
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "FullName", trip.DriverId);
            ViewData["TripKindId"] = new SelectList(_context.TripKinds, "TripKindId", "Name", trip.TripKindId);
            ViewData["BusId"] = new SelectList(_context.Buss, "Bus_Plate", "Bus_Plate", trip.BusId);
            return View(trip);
        }

        // GET: Trips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .Include(t => t.Driver)
                .Include(t => t.TripKind)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripExists(int id)
        {
            return _context.Trips.Any(e => e.Id == id);
        }
    }
}
