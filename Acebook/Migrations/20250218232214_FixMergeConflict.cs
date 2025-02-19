using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace acebook.Migrations
{
    /// <inheritdoc />
    public partial class FixMergeConflict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeOfPost",
                table: "Posts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeOfPost",
                table: "Posts",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");
        }
    }
}
