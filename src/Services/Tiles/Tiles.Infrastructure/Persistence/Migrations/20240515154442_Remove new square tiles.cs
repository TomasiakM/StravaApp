using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tiles.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Removenewsquaretiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewSquareTiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewSquareTiles",
                columns: table => new
                {
                    ActivityTilesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Z = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewSquareTiles", x => new { x.ActivityTilesId, x.X, x.Y, x.Z });
                    table.ForeignKey(
                        name: "FK_NewSquareTiles_ActivityTiles_ActivityTilesId",
                        column: x => x.ActivityTilesId,
                        principalTable: "ActivityTiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
