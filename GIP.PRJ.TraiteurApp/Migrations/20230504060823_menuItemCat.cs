using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GIP.PRJ.TraiteurApp.Migrations
{
    /// <inheritdoc />
    public partial class menuItemCat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuItemCategoryId",
                table: "MenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MenuItemCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VAT = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_MenuItemCategoryId",
                table: "MenuItems",
                column: "MenuItemCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_MenuItemCategories_MenuItemCategoryId",
                table: "MenuItems",
                column: "MenuItemCategoryId",
                principalTable: "MenuItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_MenuItemCategories_MenuItemCategoryId",
                table: "MenuItems");

            migrationBuilder.DropTable(
                name: "MenuItemCategories");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_MenuItemCategoryId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "MenuItemCategoryId",
                table: "MenuItems");
        }
    }
}
