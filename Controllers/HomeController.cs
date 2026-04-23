using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;

namespace HotelBooking.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var rooms = await _db.Rooms.ToListAsync();
        return View(rooms);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}