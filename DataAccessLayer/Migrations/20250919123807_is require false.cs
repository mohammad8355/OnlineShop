using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class isrequirefalse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "56764fbe-670b-4b30-b218-36a02be4850c", "bf1cbf03-bfa0-4f72-9fe3-60d602f5696c" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bf1cbf03-bfa0-4f72-9fe3-60d602f5696c");

            migrationBuilder.AlterColumn<string>(
                name: "Cover",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsEnable", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "ProfileImageName", "SecurityStamp", "TwoFactorEnabled", "UserName", "brithDate", "city" },
                values: new object[] { "fb51f09a-8ba7-4631-922b-ebd6b402c1b1", 0, "address", "f2cbc7fe-b5c6-4010-82f9-d051f540aca2", "almasikhanobusiness@gmail.com", true, null, true, false, null, null, null, "hashed", "09112076352", true, null, null, "adf381fb-94fa-4cd8-8604-438788bd7151", false, "admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "56764fbe-670b-4b30-b218-36a02be4850c", "fb51f09a-8ba7-4631-922b-ebd6b402c1b1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "56764fbe-670b-4b30-b218-36a02be4850c", "fb51f09a-8ba7-4631-922b-ebd6b402c1b1" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fb51f09a-8ba7-4631-922b-ebd6b402c1b1");

            migrationBuilder.AlterColumn<string>(
                name: "Cover",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsEnable", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "ProfileImageName", "SecurityStamp", "TwoFactorEnabled", "UserName", "brithDate", "city" },
                values: new object[] { "bf1cbf03-bfa0-4f72-9fe3-60d602f5696c", 0, "address", "8f9593a2-dae7-4015-b7f5-dd8f2473dea1", "almasikhanobusiness@gmail.com", true, null, true, false, null, null, null, "hashed", "09112076352", true, null, null, "cc194b45-18a2-4ef0-902f-74121ae3140d", false, "admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "56764fbe-670b-4b30-b218-36a02be4850c", "bf1cbf03-bfa0-4f72-9fe3-60d602f5696c" });
        }
    }
}
