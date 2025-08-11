using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addcoverfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "56764fbe-670b-4b30-b218-36a02be4850c", "9cd229d7-5467-4365-be2f-faca4d26d1e9" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9cd229d7-5467-4365-be2f-faca4d26d1e9");

            migrationBuilder.AddColumn<string>(
                name: "Cover",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsEnable", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "ProfileImageName", "SecurityStamp", "TwoFactorEnabled", "UserName", "brithDate", "city" },
                values: new object[] { "9e703292-fe3e-4824-b076-f53beedb5126", 0, "address", "ea193998-51d3-47c3-92f1-a4f618a2dba8", "almasikhanobusiness@gmail.com", true, null, true, false, null, null, null, "hashed", "09112076352", true, null, null, "06cee54b-d917-4ad3-ae69-83b4e2040e3e", false, "admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "56764fbe-670b-4b30-b218-36a02be4850c", "9e703292-fe3e-4824-b076-f53beedb5126" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "56764fbe-670b-4b30-b218-36a02be4850c", "9e703292-fe3e-4824-b076-f53beedb5126" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e703292-fe3e-4824-b076-f53beedb5126");

            migrationBuilder.DropColumn(
                name: "Cover",
                table: "Categories");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsEnable", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "ProfileImageName", "SecurityStamp", "TwoFactorEnabled", "UserName", "brithDate", "city" },
                values: new object[] { "9cd229d7-5467-4365-be2f-faca4d26d1e9", 0, "address", "0747e9bf-2ef4-4eb3-a8f5-032bf1858c25", "almasikhanobusiness@gmail.com", true, null, true, false, null, null, null, "hashed", "09112076352", true, null, null, "8faa7a60-39c2-42d7-90db-b0a3555820dd", false, "admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "56764fbe-670b-4b30-b218-36a02be4850c", "9cd229d7-5467-4365-be2f-faca4d26d1e9" });
        }
    }
}
