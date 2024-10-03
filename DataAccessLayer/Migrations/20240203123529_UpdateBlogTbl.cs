using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlogTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blogSections");

            migrationBuilder.DropTable(
                name: "weblogs");

            migrationBuilder.CreateTable(
                name: "blogPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdates = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadingTime = table.Column<int>(type: "int", nullable: false),
                    CoverLink = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogPost", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blogPost");

            migrationBuilder.CreateTable(
                name: "weblogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoverLink = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadingTime = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weblogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blogSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weblog_Id = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    photo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_blogSections_weblogs_Weblog_Id",
                        column: x => x.Weblog_Id,
                        principalTable: "weblogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_blogSections_Weblog_Id",
                table: "blogSections",
                column: "Weblog_Id");
        }
    }
}
