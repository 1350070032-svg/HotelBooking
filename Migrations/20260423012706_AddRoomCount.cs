using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableRooms",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalRooms",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AvailableRooms", "ImageUrl", "TotalRooms" },
                values: new object[] { 10, "https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=800&q=80", 10 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AvailableRooms", "ImageUrl", "TotalRooms" },
                values: new object[] { 6, "https://images.unsplash.com/photo-1618773928121-c32242e63f39?w=800&q=80", 6 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AvailableRooms", "ImageUrl", "TotalRooms" },
                values: new object[] { 3, "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?w=800&q=80", 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableRooms",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "TotalRooms",
                table: "Rooms");

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "");
        }
    }
}
