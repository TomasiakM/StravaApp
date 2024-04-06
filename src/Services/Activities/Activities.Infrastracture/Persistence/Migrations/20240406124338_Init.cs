using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Activities.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StravaId = table.Column<long>(type: "bigint", nullable: false),
                    StravaUserId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SportType = table.Column<int>(type: "int", nullable: false),
                    Private = table.Column<bool>(type: "bit", nullable: false),
                    Distance = table.Column<float>(type: "real", nullable: false),
                    TotalElevationGain = table.Column<float>(type: "real", nullable: false),
                    AverageCadence = table.Column<float>(type: "real", nullable: false),
                    Kilojoules = table.Column<float>(type: "real", nullable: false),
                    Calories = table.Column<float>(type: "real", nullable: false),
                    MaxSpeed = table.Column<float>(type: "real", nullable: false),
                    AverageSpeed = table.Column<float>(type: "real", nullable: false),
                    MovingTime = table.Column<int>(type: "int", nullable: false),
                    ElapsedTime = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDateLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeviceWatts = table.Column<bool>(type: "bit", nullable: false),
                    MaxWatts = table.Column<int>(type: "int", nullable: false),
                    AverageWatts = table.Column<float>(type: "real", nullable: false),
                    HasHeartrate = table.Column<bool>(type: "bit", nullable: false),
                    MaxHeartrate = table.Column<float>(type: "real", nullable: false),
                    AverageHeartrate = table.Column<float>(type: "real", nullable: false),
                    StartLat = table.Column<float>(type: "real", nullable: true),
                    StartLng = table.Column<float>(type: "real", nullable: true),
                    EndLat = table.Column<float>(type: "real", nullable: true),
                    EndLng = table.Column<float>(type: "real", nullable: true),
                    Polyline = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummaryPolyline = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_StravaId",
                table: "Activities",
                column: "StravaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_StravaUserId",
                table: "Activities",
                column: "StravaUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");
        }
    }
}
