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
    public class ReservationsController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Reservations.Include(r => r.Clint).Include(r => r.Trip);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Clint)
                .Include(r => r.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["ClintId"] = new SelectList(_context.Clints, "Id", "FullName");
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Id");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClintId,TripId,ReservationDate")] Reservation reservation)
        {
            reservation.ReservationDate = DateTime.UtcNow;
            //make sure there are avalibel seats in this trip
            var trip = await _context.Trips.FirstOrDefaultAsync(x => x.Id == reservation.TripId);
            if (trip!.AvailableSeats == 0)
            {
                ModelState.AddModelError(nameof(reservation.TripId), "There are no Available Seats in this trip");
            }
            #region    //can not assign clint in the same trip
            var clintTripsIds = await _context.Reservations.Where(a => a.ClintId ==
            reservation.ClintId)
                .Select(a =>

                    a.TripId
                )
                .ToListAsync();
            foreach (var clintTripsId in clintTripsIds)
            {
                if (reservation.TripId == clintTripsId)
                {
                    ModelState.AddModelError(nameof(reservation.TripId), "Can not assaign the same customer to the same trip in the same date");
                    break;
                }
            }
            #endregion
            if (ModelState.IsValid)
            {

                _context.Add(reservation);
                await _context.SaveChangesAsync();

                #region mins avalibal seate
                trip!.AvailableSeats -= 1;
                trip!.ReservedSeats += 1;
                _context.Trips.Update(trip);
                await _context.SaveChangesAsync();
                #endregion



                return RedirectToAction(nameof(Index));
            }
            ViewData["ClintId"] = new SelectList(_context.Clints, "Id", "FullName", reservation.ClintId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Id", reservation.TripId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["ClintId"] = new SelectList(_context.Clints, "Id", "FullName", reservation.ClintId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Id", reservation.TripId);
            ViewBag.oldtripId = reservation.TripId;
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromQuery] int tripId, [Bind("Id,ClintId,TripId,ReservationDate")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }
            //make sure there are avalibel seats in this trip
            var trip = await _context.Trips.FirstOrDefaultAsync(x => x.Id == reservation.TripId);
            if (tripId != reservation.TripId)
            {
                if (trip!.AvailableSeats == 0)
                {
                    ModelState.AddModelError(nameof(reservation.TripId), "There are no Available Seats in this trip");
                }
                #region    //can not assign clint in the same trip
                var clintTripsIds = await _context.Reservations.Where(a => a.ClintId ==
                reservation.ClintId && a.Id != id)
                    .Select(a => a.TripId)
                    .ToListAsync();
                foreach (var clintTripsId in clintTripsIds)
                {
                    if (reservation.TripId == clintTripsId)
                    {
                        ModelState.AddModelError(nameof(reservation.TripId), "Can not assaign the same customer to the same trip in the same date");
                        break;
                    }
                }
                #endregion
            }
            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                    if (tripId != reservation.TripId)
                    {
                        trip!.AvailableSeats -= 1;
                        trip!.ReservedSeats += 1;
                        _context.Trips.Update(trip);

                        //old Trip
                        var oldTrip = await _context.Trips.FirstOrDefaultAsync(x => x.Id == tripId);
                        oldTrip!.ReservedSeats -= 1;
                        oldTrip!.AvailableSeats += 1;
                        _context.Trips.Update(oldTrip);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["ClintId"] = new SelectList(_context.Clints, "Id", "FullName", reservation.ClintId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Id", reservation.TripId);
            ViewBag.oldtripId = tripId;
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Clint)
                .Include(r => r.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            // Trip seat
            var oldTrip = await _context.Trips.FirstOrDefaultAsync(x => x.Id == reservation!.TripId);
            oldTrip!.AvailableSeats += 1;
            oldTrip!.ReservedSeats -= 1;
            _context.Trips.Update(oldTrip);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
