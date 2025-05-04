using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletetionDateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "UserRole",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "ScheduleMember",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "Schedule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "Lesson",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "Institution",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "AuthToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "AppUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DeletionDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DeletionDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DeletionDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DeletionDate",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "ScheduleMember");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "Lesson");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "AuthToken");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "AppUser");
        }
    }
}
