using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class changerelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_Categories_SubCategory_Id",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_SubCategory_Id",
                table: "products");

            migrationBuilder.DropColumn(
                name: "SubCategory_Id",
                table: "products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubCategory_Id",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_products_SubCategory_Id",
                table: "products",
                column: "SubCategory_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_products_Categories_SubCategory_Id",
                table: "products",
                column: "SubCategory_Id",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
