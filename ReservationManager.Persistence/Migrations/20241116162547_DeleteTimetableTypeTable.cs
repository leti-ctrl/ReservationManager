using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReservationManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeleteTimetableTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstabilishmentTimetable_TimetableType_TypeId",
                table: "BuildingTimetable");

            migrationBuilder.DropTable(
                name: "TimetableType");

            migrationBuilder.DropIndex(
                name: "IX_EstabilishmentTimetable_TypeId",
                table: "BuildingTimetable");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "BuildingTimetable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "BuildingTimetable",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TimetableType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimetableType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstabilishmentTimetable_TypeId",
                table: "BuildingTimetable",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstabilishmentTimetable_TimetableType_TypeId",
                table: "BuildingTimetable",
                column: "TypeId",
                principalTable: "TimetableType",
                principalColumn: "Id");
        }
    }
}
