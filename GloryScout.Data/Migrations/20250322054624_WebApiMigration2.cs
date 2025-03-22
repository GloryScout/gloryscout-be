using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GloryScout.Data.Migrations
{
    /// <inheritdoc />
    public partial class WebApiMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5be38547-9a06-47cc-bac5-86fbe6877d4b"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d80ba2fa-395c-4b69-821f-92b6876372fc"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dbd47934-fb4a-4bf2-a8c2-cbaecedb4122"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("27d0e2e2-40e0-4cf2-8267-19f1ac77d53b"), "27D0E2E2-40E0-4CF2-8267-19F1AC77D53B", "Player", "PLAYER" },
                    { new Guid("a8d3c1e1-bcc3-4b3e-ab7c-a7f7fbd27231"), "A8D3C1E1-BCC3-4B3E-AB7C-A7F7FBD27231", "Admin", "ADMIN" },
                    { new Guid("e3f1286b-79d2-46c3-98e4-91f89e10e93d"), "E3F1286B-79D2-46C3-98E4-91F89E10E93D", "Scout", "SCOUT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("27d0e2e2-40e0-4cf2-8267-19f1ac77d53b"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a8d3c1e1-bcc3-4b3e-ab7c-a7f7fbd27231"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("e3f1286b-79d2-46c3-98e4-91f89e10e93d"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5be38547-9a06-47cc-bac5-86fbe6877d4b"), "fa1b0775-8999-4dd3-9567-57ccbfe46761", "Admin", "ADMIN" },
                    { new Guid("d80ba2fa-395c-4b69-821f-92b6876372fc"), "0ffd89a2-bd29-417c-940a-b6fc10359142", "Scout", "SCOUT" },
                    { new Guid("dbd47934-fb4a-4bf2-a8c2-cbaecedb4122"), "3ce43c69-40f7-4e54-9775-f9128da98155", "Player", "PLAYER" }
                });
        }
    }
}
