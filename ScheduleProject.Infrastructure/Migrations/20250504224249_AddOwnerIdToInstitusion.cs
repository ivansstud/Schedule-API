using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerIdToInstitusion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OwnerId",
                table: "Institution",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Institution_OwnerId",
                table: "Institution",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_AppUser_OwnerId",
                table: "Institution",
                column: "OwnerId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Institution_AppUser_OwnerId",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_Institution_OwnerId",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Institution");
        }
    }
}
