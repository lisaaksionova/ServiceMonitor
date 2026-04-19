using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceMonitor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CheckIntervalForService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CheckIntervalMinutes",
                table: "Services",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextCheckAt",
                table: "Services",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckIntervalMinutes",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "NextCheckAt",
                table: "Services");
        }
    }
}
