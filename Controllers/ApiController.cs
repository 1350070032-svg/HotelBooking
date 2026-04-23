using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public RoomsApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_db.Rooms.ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var room = await _db.Rooms.FindAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Room room)
        {
            _db.Rooms.Add(room);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = room.Id }, room);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Room room)
        {
            if (id != room.Id) return BadRequest();
            _db.Update(room);
            await _db.SaveChangesAsync();
            return Ok(room);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _db.Rooms.FindAsync(id);
            if (room == null) return NotFound();
            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}