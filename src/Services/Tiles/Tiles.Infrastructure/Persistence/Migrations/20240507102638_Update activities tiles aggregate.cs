using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tiles.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Updateactivitiestilesaggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewSquare",
                table: "ActivityTiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "NewClusterTiles",
                columns: table => new
                {
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Z = table.Column<int>(type: "int", nullable: false),
                    ActivityTilesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewClusterTiles", x => new { x.ActivityTilesId, x.X, x.Y, x.Z });
                    table.ForeignKey(
                        name: "FK_NewClusterTiles_ActivityTiles_ActivityTilesId",
                        column: x => x.ActivityTilesId,
                        principalTable: "ActivityTiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewSquareTiles",
                columns: table => new
                {
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Z = table.Column<int>(type: "int", nullable: false),
                    ActivityTilesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "NewTiles",
                columns: table => new
                {
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Z = table.Column<int>(type: "int", nullable: false),
                    ActivityTilesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewTiles", x => new { x.ActivityTilesId, x.X, x.Y, x.Z });
                    table.ForeignKey(
                        name: "FK_NewTiles_ActivityTiles_ActivityTilesId",
                        column: x => x.ActivityTilesId,
                        principalTable: "ActivityTiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewClusterTiles");

            migrationBuilder.DropTable(
                name: "NewSquareTiles");

            migrationBuilder.DropTable(
                name: "NewTiles");

            migrationBuilder.DropColumn(
                name: "NewSquare",
                table: "ActivityTiles");
        }
    }
}
