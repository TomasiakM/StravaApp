using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tiles.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Updateaggregatepropertyname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Coordinates",
                table: "Coordinates",
                newName: "LatLngs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LatLngs",
                table: "Coordinates",
                newName: "Coordinates");
        }
    }
}
