using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleProject.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTelegramNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleMembers_Users_TelegramUserId",
                table: "ScheduleMembers");

            migrationBuilder.RenameColumn(
                name: "TelegramUserId",
                table: "ScheduleMembers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleMembers_TelegramUserId",
                table: "ScheduleMembers",
                newName: "IX_ScheduleMembers_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleMembers_Users_UserId",
                table: "ScheduleMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleMembers_Users_UserId",
                table: "ScheduleMembers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ScheduleMembers",
                newName: "TelegramUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleMembers_UserId",
                table: "ScheduleMembers",
                newName: "IX_ScheduleMembers_TelegramUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleMembers_Users_TelegramUserId",
                table: "ScheduleMembers",
                column: "TelegramUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
