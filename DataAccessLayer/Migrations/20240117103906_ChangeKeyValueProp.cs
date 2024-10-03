using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeKeyValueProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoolValue",
                table: "adjValues");

            migrationBuilder.DropColumn(
                name: "NumericValue",
                table: "adjValues");

            migrationBuilder.DropColumn(
                name: "StringValue",
                table: "adjValues");

            migrationBuilder.RenameColumn(
                name: "DataType",
                table: "adjKeys",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "adjValues",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "adjValues");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "adjKeys",
                newName: "DataType");

            migrationBuilder.AddColumn<bool>(
                name: "BoolValue",
                table: "adjValues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumericValue",
                table: "adjValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StringValue",
                table: "adjValues",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }
    }
}
