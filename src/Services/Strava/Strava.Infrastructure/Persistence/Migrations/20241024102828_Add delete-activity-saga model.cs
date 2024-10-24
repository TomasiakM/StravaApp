using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Strava.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Adddeleteactivitysagamodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SagaDatas",
                table: "SagaDatas");

            migrationBuilder.RenameTable(
                name: "SagaDatas",
                newName: "ProcessActivitySagaDatas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessActivitySagaDatas",
                table: "ProcessActivitySagaDatas",
                column: "CorrelationId");

            migrationBuilder.CreateTable(
                name: "DeleteActivitySagaDatas",
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
                    table.PrimaryKey("PK_DeleteActivitySagaDatas", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeleteActivitySagaDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessActivitySagaDatas",
                table: "ProcessActivitySagaDatas");

            migrationBuilder.RenameTable(
                name: "ProcessActivitySagaDatas",
                newName: "SagaDatas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SagaDatas",
                table: "SagaDatas",
                column: "CorrelationId");
        }
    }
}
