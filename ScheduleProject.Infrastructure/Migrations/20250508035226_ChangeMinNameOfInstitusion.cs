using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMinNameOfInstitusion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Institution_ShortName",
                table: "Institution");

            migrationBuilder.CreateIndex(
                name: "IX_Institution_ShortName",
                table: "Institution",
                column: "ShortName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Institution_ShortName",
                table: "Institution");

            migrationBuilder.CreateIndex(
                name: "IX_Institution_ShortName",
                table: "Institution",
                column: "ShortName",
                unique: true);
        }
    }
}
