using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;

namespace HotelBooking.Controllers;

public class RoomController : Controller
{
    private readonly ApplicationDbContext _db;

    public RoomController(ApplicationDbContext db)
    {
        _db = db;
    }

    
    public async Task<IActionResult> Index(DateTime? checkIn, DateTime? checkOut, int guests = 1)
    {
        var rooms = await _db.Rooms.ToListAsync();

        // Lọc theo số khách nếu có
        if (guests > 0)
        {
            rooms = rooms.Where(r => r.Capacity >= guests).ToList();
        }

        return View(rooms);
    }

    
    public async Task<IActionResult> Details(int id)
    {
        var room = await _db.Rooms
            .Include(r => r.Reviews)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room == null) return NotFound();
        return View(room);
    }
}