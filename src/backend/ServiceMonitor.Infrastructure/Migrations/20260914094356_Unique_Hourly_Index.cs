using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceMonitor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Unique_Hourly_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HourlyServiceChecks_ServiceId_Hour",
                table: "HourlyServiceChecks",
                columns: new[] { "ServiceId", "Hour" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HourlyServiceChecks_ServiceId_Hour",
                table: "HourlyServiceChecks");
        }
    }
}
