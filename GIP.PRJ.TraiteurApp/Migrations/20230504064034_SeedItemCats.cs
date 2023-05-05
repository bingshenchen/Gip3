using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GIP.PRJ.TraiteurApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedItemCats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MenuItemCategories",
                columns: new[] { "Id", "Name", "VAT" },
                values: new object[,]
                {
                    { -3, "Alcoholische dranken", 21m },
                    { -2, "Niet-alcoholische dranken", 6m },
                    { -1, "Afhaalgerechten", 6m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItemCategories",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "MenuItemCategories",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "MenuItemCategories",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
