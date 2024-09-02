using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Strava.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SagaDatas",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StravaActivityId = table.Column<long>(type: "bigint", nullable: false),
                    StravaUserId = table.Column<long>(type: "bigint", nullable: false),
                    ActivitiesServiceProcessed = table.Column<bool>(type: "bit", nullable: false),
                    TilesServiceProcessed = table.Column<bool>(type: "bit", nullable: false),
                    AchievementsServiceProcessed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SagaDatas", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SagaDatas");
        }
    }
}
