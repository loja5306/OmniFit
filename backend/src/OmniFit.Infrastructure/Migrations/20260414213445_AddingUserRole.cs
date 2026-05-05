using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmniFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a4a244a0-8f9d-4c16-843b-fb121ef0bb88", "b75d4aee-0984-4b23-982a-3618decec41b", "Admin", "ADMIN" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a4a244a0-8f9d-4c16-843b-fb121ef0bb88");
        }
    }
}
