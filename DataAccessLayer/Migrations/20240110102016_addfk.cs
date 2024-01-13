using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addfk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adjValues_adjKeys_adjKeyId",
                table: "adjValues");

            migrationBuilder.DropForeignKey(
                name: "FK_blogSections_weblogs_WeblogId",
                table: "blogSections");

            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_orders_orderId",
                table: "orderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_products_subCategories_subCategoryId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_subCategoryId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_orderDetails_orderId",
                table: "orderDetails");

            migrationBuilder.DropIndex(
                name: "IX_blogSections_WeblogId",
                table: "blogSections");

            migrationBuilder.DropIndex(
                name: "IX_adjValues_adjKeyId",
                table: "adjValues");

            migrationBuilder.DropColumn(
                name: "subCategoryId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "orderId",
                table: "orderDetails");

            migrationBuilder.DropColumn(
                name: "WeblogId",
                table: "blogSections");

            migrationBuilder.DropColumn(
                name: "adjKeyId",
                table: "adjValues");

            migrationBuilder.CreateIndex(
                name: "IX_products_SubCategory_Id",
                table: "products",
                column: "SubCategory_Id");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_order_Id",
                table: "orderDetails",
                column: "order_Id");

            migrationBuilder.CreateIndex(
                name: "IX_blogSections_Weblog_Id",
                table: "blogSections",
                column: "Weblog_Id");

            migrationBuilder.CreateIndex(
                name: "IX_adjValues_adjkey_Id",
                table: "adjValues",
                column: "adjkey_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_adjValues_adjKeys_adjkey_Id",
                table: "adjValues",
                column: "adjkey_Id",
                principalTable: "adjKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_blogSections_weblogs_Weblog_Id",
                table: "blogSections",
                column: "Weblog_Id",
                principalTable: "weblogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_orders_order_Id",
                table: "orderDetails",
                column: "order_Id",
                principalTable: "orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_products_subCategories_SubCategory_Id",
                table: "products",
                column: "SubCategory_Id",
                principalTable: "subCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adjValues_adjKeys_adjkey_Id",
                table: "adjValues");

            migrationBuilder.DropForeignKey(
                name: "FK_blogSections_weblogs_Weblog_Id",
                table: "blogSections");

            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_orders_order_Id",
                table: "orderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_products_subCategories_SubCategory_Id",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_SubCategory_Id",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_orderDetails_order_Id",
                table: "orderDetails");

            migrationBuilder.DropIndex(
                name: "IX_blogSections_Weblog_Id",
                table: "blogSections");

            migrationBuilder.DropIndex(
                name: "IX_adjValues_adjkey_Id",
                table: "adjValues");

            migrationBuilder.AddColumn<int>(
                name: "subCategoryId",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "orderId",
                table: "orderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeblogId",
                table: "blogSections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "adjKeyId",
                table: "adjValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_products_subCategoryId",
                table: "products",
                column: "subCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_orderId",
                table: "orderDetails",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_blogSections_WeblogId",
                table: "blogSections",
                column: "WeblogId");

            migrationBuilder.CreateIndex(
                name: "IX_adjValues_adjKeyId",
                table: "adjValues",
                column: "adjKeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_adjValues_adjKeys_adjKeyId",
                table: "adjValues",
                column: "adjKeyId",
                principalTable: "adjKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_blogSections_weblogs_WeblogId",
                table: "blogSections",
                column: "WeblogId",
                principalTable: "weblogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_orders_orderId",
                table: "orderDetails",
                column: "orderId",
                principalTable: "orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_products_subCategories_subCategoryId",
                table: "products",
                column: "subCategoryId",
                principalTable: "subCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
