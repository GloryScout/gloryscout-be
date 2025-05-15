using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GloryScout.Data.Migrations
{
    /// <inheritdoc />
    public partial class addingpayments2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Users_UserId1",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Users_UserId2",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_UserId1",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_UserId2",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "UserId2",
                table: "Subscriptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Subscriptions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId2",
                table: "Subscriptions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId1",
                table: "Subscriptions",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId2",
                table: "Subscriptions",
                column: "UserId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Users_UserId1",
                table: "Subscriptions",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Users_UserId2",
                table: "Subscriptions",
                column: "UserId2",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
