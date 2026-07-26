using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartOrderManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShiftReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShiftReference",
                table: "Orders",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShiftReference",
                table: "Orders");
        }
    }
}
