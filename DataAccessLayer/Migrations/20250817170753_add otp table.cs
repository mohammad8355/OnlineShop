using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addotptable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "56764fbe-670b-4b30-b218-36a02be4850c", "9e703292-fe3e-4824-b076-f53beedb5126" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e703292-fe3e-4824-b076-f53beedb5126");

            migrationBuilder.CreateTable(
                name: "otpCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiry = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_otpCodes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsEnable", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "ProfileImageName", "SecurityStamp", "TwoFactorEnabled", "UserName", "brithDate", "city" },
                values: new object[] { "bf1cbf03-bfa0-4f72-9fe3-60d602f5696c", 0, "address", "8f9593a2-dae7-4015-b7f5-dd8f2473dea1", "almasikhanobusiness@gmail.com", true, null, true, false, null, null, null, "hashed", "09112076352", true, null, null, "cc194b45-18a2-4ef0-902f-74121ae3140d", false, "admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "56764fbe-670b-4b30-b218-36a02be4850c", "bf1cbf03-bfa0-4f72-9fe3-60d602f5696c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "otpCodes");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "56764fbe-670b-4b30-b218-36a02be4850c", "bf1cbf03-bfa0-4f72-9fe3-60d602f5696c" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bf1cbf03-bfa0-4f72-9fe3-60d602f5696c");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsEnable", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "ProfileImageName", "SecurityStamp", "TwoFactorEnabled", "UserName", "brithDate", "city" },
                values: new object[] { "9e703292-fe3e-4824-b076-f53beedb5126", 0, "address", "ea193998-51d3-47c3-92f1-a4f618a2dba8", "almasikhanobusiness@gmail.com", true, null, true, false, null, null, null, "hashed", "09112076352", true, null, null, "06cee54b-d917-4ad3-ae69-83b4e2040e3e", false, "admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "56764fbe-670b-4b30-b218-36a02be4850c", "9e703292-fe3e-4824-b076-f53beedb5126" });
        }
    }
}
