using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class changerelationUserOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_orders_User_Id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "Order_Id",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 17, 15, 13, 32, 631, DateTimeKind.Local).AddTicks(1269));

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 17, 15, 13, 32, 631, DateTimeKind.Local).AddTicks(1363));

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 17, 15, 13, 32, 631, DateTimeKind.Local).AddTicks(1387));

            migrationBuilder.CreateIndex(
                name: "IX_orders_User_Id",
                table: "orders",
                column: "User_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_orders_User_Id",
                table: "orders");

            migrationBuilder.AddColumn<int>(
                name: "Order_Id",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 17, 10, 29, 45, 442, DateTimeKind.Local).AddTicks(1254));

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 17, 10, 29, 45, 442, DateTimeKind.Local).AddTicks(1358));

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 17, 10, 29, 45, 442, DateTimeKind.Local).AddTicks(1385));

            migrationBuilder.CreateIndex(
                name: "IX_orders_User_Id",
                table: "orders",
                column: "User_Id",
                unique: true);
        }
    }
}
