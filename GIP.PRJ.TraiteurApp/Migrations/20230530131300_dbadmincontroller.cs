using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GIP.PRJ.TraiteurApp.Migrations
{
    /// <inheritdoc />
    public partial class dbadmincontroller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_CreateRolesViewModel_CreateRolesViewModelId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CreateRolesViewModel");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "CreateRolesViewModel");

            migrationBuilder.RenameColumn(
                name: "CreateRolesViewModelId",
                table: "AspNetRoles",
                newName: "UserViewModelId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoles_CreateRolesViewModelId",
                table: "AspNetRoles",
                newName: "IX_AspNetRoles_UserViewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_CreateRolesViewModel_UserViewModelId",
                table: "AspNetRoles",
                column: "UserViewModelId",
                principalTable: "CreateRolesViewModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_CreateRolesViewModel_UserViewModelId",
                table: "AspNetRoles");

            migrationBuilder.RenameColumn(
                name: "UserViewModelId",
                table: "AspNetRoles",
                newName: "CreateRolesViewModelId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoles_UserViewModelId",
                table: "AspNetRoles",
                newName: "IX_AspNetRoles_CreateRolesViewModelId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CreateRolesViewModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "CreateRolesViewModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_CreateRolesViewModel_CreateRolesViewModelId",
                table: "AspNetRoles",
                column: "CreateRolesViewModelId",
                principalTable: "CreateRolesViewModel",
                principalColumn: "Id");
        }
    }
}
