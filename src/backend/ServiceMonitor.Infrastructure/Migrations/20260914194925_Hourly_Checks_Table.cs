using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceMonitor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Hourly_Checks_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HourlyServiceChecks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Hour = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalChecks = table.Column<int>(type: "integer", nullable: false),
                    SuccessfulChecks = table.Column<int>(type: "integer", nullable: false),
                    FailedChecks = table.Column<int>(type: "integer", nullable: false),
                    AverageResponseTimeMs = table.Column<double>(type: "double precision", nullable: false),
                    MinResponseTimeMs = table.Column<double>(type: "double precision", nullable: false),
                    MaxResponseTimeMs = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourlyServiceChecks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HourlyServiceChecks_ServiceId_Hour",
                table: "HourlyServiceChecks",
                columns: new[] { "ServiceId", "Hour" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HourlyServiceChecks");
        }
    }
}
