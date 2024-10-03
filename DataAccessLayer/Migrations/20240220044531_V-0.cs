using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class V0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Brand_Id",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Discount",
                table: "products",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "products",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecial",
                table: "keyToProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Product_Id",
                table: "commnets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    rate = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "LastUpdate", "Product_Id" },
                values: new object[] { new DateTime(2024, 2, 20, 8, 15, 30, 609, DateTimeKind.Local).AddTicks(9709), null });

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "LastUpdate", "Product_Id" },
                values: new object[] { new DateTime(2024, 2, 20, 8, 15, 30, 609, DateTimeKind.Local).AddTicks(9817), null });

            migrationBuilder.UpdateData(
                table: "commnets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "LastUpdate", "Product_Id" },
                values: new object[] { new DateTime(2024, 2, 20, 8, 15, 30, 609, DateTimeKind.Local).AddTicks(9848), null });

            migrationBuilder.CreateIndex(
                name: "IX_products_Brand_Id",
                table: "products",
                column: "Brand_Id");

            migrationBuilder.CreateIndex(
                name: "IX_commnets_Product_Id",
                table: "commnets",
                column: "Product_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_commnets_products_Product_Id",
                table: "commnets",
                column: "Product_Id",
                principalTable: "products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_products_brands_Brand_Id",
                table: "products",
                column: "Brand_Id",
                principalTable: "brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_commnets_products_Product_Id",
                table: "commnets");

            migrationBuilder.DropForeignKey(
                name: "FK_products_brands_Brand_Id",
                table: "products");

            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropIndex(
                name: "IX_products_Brand_Id",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_commnets_Product_Id",
                table: "commnets");

            migrationBuilder.DropColumn(
                name: "Brand_Id",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "products");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "products");

            migrationBuilder.DropColumn(
                name: "IsSpecial",
                table: "keyToProducts");

            migrationBuilder.DropColumn(
                name: "Product_Id",
                table: "commnets");

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
        }
    }
}
