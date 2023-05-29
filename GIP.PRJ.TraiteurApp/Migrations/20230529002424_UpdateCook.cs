using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GIP.PRJ.TraiteurApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HolidayEndTime",
                table: "Cooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "HolidayStartTime",
                table: "Cooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsHoliday",
                table: "Cooks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSick",
                table: "Cooks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SickEndTime",
                table: "Cooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SickStartTime",
                table: "Cooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreateRolesViewModelId",
                table: "AspNetRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CreateRolesViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateRolesViewModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_CreateRolesViewModelId",
                table: "AspNetRoles",
                column: "CreateRolesViewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_CreateRolesViewModel_CreateRolesViewModelId",
                table: "AspNetRoles",
                column: "CreateRolesViewModelId",
                principalTable: "CreateRolesViewModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_CreateRolesViewModel_CreateRolesViewModelId",
                table: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CreateRolesViewModel");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_CreateRolesViewModelId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "HolidayEndTime",
                table: "Cooks");

            migrationBuilder.DropColumn(
                name: "HolidayStartTime",
                table: "Cooks");

            migrationBuilder.DropColumn(
                name: "IsHoliday",
                table: "Cooks");

            migrationBuilder.DropColumn(
                name: "IsSick",
                table: "Cooks");

            migrationBuilder.DropColumn(
                name: "SickEndTime",
                table: "Cooks");

            migrationBuilder.DropColumn(
                name: "SickStartTime",
                table: "Cooks");

            migrationBuilder.DropColumn(
                name: "CreateRolesViewModelId",
                table: "AspNetRoles");
        }
    }
}
