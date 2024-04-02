using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addsomecol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "FactorNumber",
                table: "orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PayDate",
                table: "orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TrackingCode",
                table: "orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "FactorNumber",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "PayDate",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "TrackingCode",
                table: "orders");

            migrationBuilder.InsertData(
                table: "commnets",
                columns: new[] { "Id", "Description", "IsHide", "LastUpdate", "Product_Id", "Reply_Id", "Ticket_Id", "Title", "User_Id" },
                values: new object[,]
                {
                    { 1, "shop supporting was very verrrrrrrrrrrrrrry gooooooooooooooooooooooood happy", false, new DateTime(2024, 2, 26, 18, 32, 20, 475, DateTimeKind.Local).AddTicks(9683), null, null, null, "shop supporting", "217440e4-9164-443b-aa76-ab6d847aaace" },
                    { 2, "shop supporting was very verrrrrrrrrrrrry    baaaaaaaaaaaaaaaaaad sad", false, new DateTime(2024, 2, 26, 18, 32, 20, 475, DateTimeKind.Local).AddTicks(9996), null, null, null, "shop supporting", "c74e93b8-9649-4c91-b459-4e9e16f2db74" },
                    { 3, "no comment ", false, new DateTime(2024, 2, 26, 18, 32, 20, 476, DateTimeKind.Local).AddTicks(43), null, null, null, null, "217440e4-9164-443b-aa76-ab6d847aaace" }
                });
        }
    }
}
