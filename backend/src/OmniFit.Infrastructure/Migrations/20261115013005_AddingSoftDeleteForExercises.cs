using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmniFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingSoftDeleteForExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Exercises",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Exercises");
        }
    }
}
