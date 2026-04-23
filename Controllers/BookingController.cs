using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HotelBooking.Data;
using HotelBooking.Models;

namespace HotelBooking.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BookingController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var room = await _db.Rooms.FindAsync(roomId);
            if (room == null) return NotFound();

            if (room.AvailableRooms <= 0)
            {
                TempData["Error"] = "Phòng này đã hết chỗ!";
                return RedirectToAction("Details", "Room", new { id = roomId });
            }

            var nights = (checkOut - checkIn).Days;
            if (nights <= 0) nights = 1;

            var booking = new Booking
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "",
                RoomId = roomId,
                CheckIn = checkIn,
                CheckOut = checkOut,
                TotalPrice = room.PricePerNight * nights,
                Status = "Pending"
            };

            room.AvailableRooms -= 1;
            if (room.AvailableRooms <= 0)
                room.IsAvailable = false;

            _db.Bookings.Add(booking);
            _db.Rooms.Update(room);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Đặt phòng thành công! Chờ xác nhận.";
            return RedirectToAction("MyBookings");
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _db.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (booking.UserId != userId) return Forbid();

            booking.Status = "Cancelled";
            if (booking.Room != null)
            {
                booking.Room.AvailableRooms += 1;
                booking.Room.IsAvailable = true;
                _db.Rooms.Update(booking.Room);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("MyBookings");
        }

        public async Task<IActionResult> MyBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bookings = await _db.Bookings
                .Include(b => b.Room)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            return View(bookings);
        }
    }
}