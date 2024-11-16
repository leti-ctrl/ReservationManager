using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedColumnsInClosingCalendarTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ClosingCalendar");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "ClosingCalendar");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ClosingCalendar");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "ClosingCalendar");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Day",
                table: "ClosingCalendar",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "ResourceId",
                table: "ClosingCalendar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClosingCalendar_ResourceId",
                table: "ClosingCalendar",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClosingCalendar_Resource_ResourceId",
                table: "ClosingCalendar",
                column: "ResourceId",
                principalTable: "Resource",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClosingCalendar_Resource_ResourceId",
                table: "ClosingCalendar");

            migrationBuilder.DropIndex(
                name: "IX_ClosingCalendar_ResourceId",
                table: "ClosingCalendar");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "ClosingCalendar");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "ClosingCalendar");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "ClosingCalendar",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "ClosingCalendar",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "ClosingCalendar",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "ClosingCalendar",
                type: "time without time zone",
                nullable: true);
        }
    }
}
