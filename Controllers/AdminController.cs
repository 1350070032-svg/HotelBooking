using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalBookings = _db.Bookings.Count();
            ViewBag.Revenue = _db.Bookings
                .Where(b => b.Status == "Confirmed")
                .Sum(b => (decimal?)b.TotalPrice) ?? 0;
            ViewBag.PendingBookings = _db.Bookings.Count(b => b.Status == "Pending");
            ViewBag.Bookings = _db.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            var rooms = _db.Rooms.ToList();
            return View(rooms);
        }

        [HttpPost]
        public async Task<IActionResult> EditRoom(Room room)
        {
            _db.Update(room);
            await _db.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> ConfirmBooking(int id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = "Confirmed";
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _db.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking != null)
            {
                booking.Status = "Cancelled";
                if (booking.Room != null)
                {
                    booking.Room.AvailableRooms += 1;
                    booking.Room.IsAvailable = true;
                    _db.Rooms.Update(booking.Room);
                }
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }

        public IActionResult Revenue()
        {
            var data = _db.Bookings
                .Where(b => b.Status == "Confirmed")
                .GroupBy(b => new { b.CreatedAt.Year, b.CreatedAt.Month })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Total = g.Sum(b => b.TotalPrice)
                }).ToList();
            return View(data);
        }
    }
}