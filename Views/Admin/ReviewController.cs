using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HotelBooking.Data;
using HotelBooking.Models;

namespace HotelBooking.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ReviewController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int roomId, int rating, string comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var review = new Review
            {
                UserId = userId ?? "",
                RoomId = roomId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Room", new { id = roomId });
        }
    }
}