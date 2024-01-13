using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class changefluentapi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_headCategories_headCategoryId",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_subCategories_categories_categoryId",
                table: "subCategories");

            migrationBuilder.DropIndex(
                name: "IX_subCategories_categoryId",
                table: "subCategories");

            migrationBuilder.DropIndex(
                name: "IX_categories_headCategoryId",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "subCategories");

            migrationBuilder.DropColumn(
                name: "headCategoryId",
                table: "categories");

            migrationBuilder.CreateIndex(
                name: "IX_subCategories_category_Id",
                table: "subCategories",
                column: "category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_categories_headCategory_Id",
                table: "categories",
                column: "headCategory_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_headCategories_headCategory_Id",
                table: "categories",
                column: "headCategory_Id",
                principalTable: "headCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_subCategories_categories_category_Id",
                table: "subCategories",
                column: "category_Id",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_headCategories_headCategory_Id",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_subCategories_categories_category_Id",
                table: "subCategories");

            migrationBuilder.DropIndex(
                name: "IX_subCategories_category_Id",
                table: "subCategories");

            migrationBuilder.DropIndex(
                name: "IX_categories_headCategory_Id",
                table: "categories");

            migrationBuilder.AddColumn<int>(
                name: "categoryId",
                table: "subCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "headCategoryId",
                table: "categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_subCategories_categoryId",
                table: "subCategories",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_categories_headCategoryId",
                table: "categories",
                column: "headCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_headCategories_headCategoryId",
                table: "categories",
                column: "headCategoryId",
                principalTable: "headCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_subCategories_categories_categoryId",
                table: "subCategories",
                column: "categoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
