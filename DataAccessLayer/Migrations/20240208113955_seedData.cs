using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_commnets_Reply_Id",
                table: "commnets");

            migrationBuilder.AlterColumn<int>(
                name: "Ticket_Id",
                table: "commnets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Reply_Id",
                table: "commnets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "commnets",
                columns: new[] { "Id", "Description", "IsHide", "LastUpdate", "Reply_Id", "Ticket_Id", "Title", "User_Id" },
                values: new object[,]
                {
                    { 1, "shop supporting was very verrrrrrrrrrrrrrry gooooooooooooooooooooooood happy", false, new DateTime(2024, 2, 8, 15, 9, 54, 687, DateTimeKind.Local).AddTicks(39), null, null, "shop supporting", "217440e4-9164-443b-aa76-ab6d847aaace" },
                    { 2, "shop supporting was very verrrrrrrrrrrrry    baaaaaaaaaaaaaaaaaad sad", false, new DateTime(2024, 2, 8, 15, 9, 54, 687, DateTimeKind.Local).AddTicks(958), null, null, "shop supporting", "c74e93b8-9649-4c91-b459-4e9e16f2db74" },
                    { 3, "no comment ", false, new DateTime(2024, 2, 8, 15, 9, 54, 687, DateTimeKind.Local).AddTicks(1351), null, null, null, "217440e4-9164-443b-aa76-ab6d847aaace" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_commnets_Reply_Id",
                table: "commnets",
                column: "Reply_Id",
                unique: true,
                filter: "[Reply_Id] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_commnets_Reply_Id",
                table: "commnets");

            migrationBuilder.DeleteData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<int>(
                name: "Ticket_Id",
                table: "commnets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Reply_Id",
                table: "commnets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_commnets_Reply_Id",
                table: "commnets",
                column: "Reply_Id",
                unique: true);
        }
    }
}
