using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Models;

namespace HotelBooking.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Room>().HasData(
                new Room {
                    Id = 1, Name = "Phòng Standard", PricePerNight = 500000,
                    Capacity = 2, IsAvailable = true, TotalRooms = 10, AvailableRooms = 10,
                    Description = "Phòng tiêu chuẩn",
                    ImageUrl = "https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=800&q=80"
                },
                new Room {
                    Id = 2, Name = "Phòng Deluxe", PricePerNight = 900000,
                    Capacity = 2, IsAvailable = true, TotalRooms = 6, AvailableRooms = 6,
                    Description = "Phòng cao cấp",
                    ImageUrl = "https://images.unsplash.com/photo-1618773928121-c32242e63f39?w=800&q=80"
                },
                new Room {
                    Id = 3, Name = "Phòng Suite", PricePerNight = 1500000,
                    Capacity = 4, IsAvailable = true, TotalRooms = 3, AvailableRooms = 3,
                    Description = "Phòng hạng sang",
                    ImageUrl = "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?w=800&q=80"
                }
            );
        }
    }
}