using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addTagAndTagToBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TagToBlogPosts",
                columns: table => new
                {
                    Tag_Id = table.Column<int>(type: "int", nullable: false),
                    BlogPost_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagToBlogPosts", x => new { x.Tag_Id, x.BlogPost_Id });
                    table.ForeignKey(
                        name: "FK_TagToBlogPosts_blogPost_BlogPost_Id",
                        column: x => x.BlogPost_Id,
                        principalTable: "blogPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagToBlogPosts_tags_Tag_Id",
                        column: x => x.Tag_Id,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagToBlogPosts_BlogPost_Id",
                table: "TagToBlogPosts",
                column: "BlogPost_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagToBlogPosts");

            migrationBuilder.DropTable(
                name: "tags");
        }
    }
}
