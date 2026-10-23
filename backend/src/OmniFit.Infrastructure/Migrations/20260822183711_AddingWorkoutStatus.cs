using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmniFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingWorkoutStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CompletedOn",
                table: "Workouts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Workouts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedOn",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Workouts");
        }
    }
}
