using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addPropertyProfileImageName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImageName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 16, 15, 49, 11, 657, DateTimeKind.Local).AddTicks(5505));

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 16, 15, 49, 11, 657, DateTimeKind.Local).AddTicks(5677));

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 16, 15, 49, 11, 657, DateTimeKind.Local).AddTicks(5734));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 16, 12, 4, 40, 44, DateTimeKind.Local).AddTicks(9185));

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 16, 12, 4, 40, 44, DateTimeKind.Local).AddTicks(9280));

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 16, 12, 4, 40, 44, DateTimeKind.Local).AddTicks(9303));
        }
    }
}
