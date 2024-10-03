using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addcolumntogeneral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "generals",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 25, 13, 33, 4, 909, DateTimeKind.Local).AddTicks(2868));

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 25, 13, 33, 4, 909, DateTimeKind.Local).AddTicks(2964));

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 25, 13, 33, 4, 909, DateTimeKind.Local).AddTicks(2987));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "generals");

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 21, 15, 52, 43, 510, DateTimeKind.Local).AddTicks(6669));

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 21, 15, 52, 43, 510, DateTimeKind.Local).AddTicks(6831));

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 21, 15, 52, 43, 510, DateTimeKind.Local).AddTicks(6864));
        }
    }
}
