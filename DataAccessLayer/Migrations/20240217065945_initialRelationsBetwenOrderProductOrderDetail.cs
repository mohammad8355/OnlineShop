using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class initialRelationsBetwenOrderProductOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_products_ProductId",
                table: "orderDetails");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "orderDetails",
                newName: "Product_Id");

            migrationBuilder.RenameIndex(
                name: "IX_orderDetails_ProductId",
                table: "orderDetails",
                newName: "IX_orderDetails_Product_Id");

            migrationBuilder.AddColumn<string>(
                name: "User_Id",
                table: "orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_products_Product_Id",
                table: "orderDetails",
                column: "Product_Id",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_AspNetUsers_User_Id",
                table: "orders",
                column: "User_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_products_Product_Id",
                table: "orderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_AspNetUsers_User_Id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_User_Id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "User_Id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "Order_Id",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Product_Id",
                table: "orderDetails",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_orderDetails_Product_Id",
                table: "orderDetails",
                newName: "IX_orderDetails_ProductId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_products_ProductId",
                table: "orderDetails",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
