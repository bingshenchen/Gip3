using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GIP.PRJ.TraiteurApp.Migrations
{
    /// <inheritdoc />
    public partial class Cooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CookId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChefName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cooks_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CookId",
                table: "Orders",
                column: "CookId");

            migrationBuilder.CreateIndex(
                name: "IX_Cooks_IdentityUserId",
                table: "Cooks",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cooks_CookId",
                table: "Orders",
                column: "CookId",
                principalTable: "Cooks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cooks_CookId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Cooks");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CookId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CookId",
                table: "Orders");
        }
    }
}
