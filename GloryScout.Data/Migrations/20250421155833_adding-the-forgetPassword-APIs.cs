using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GloryScout.Data.Migrations
{
    /// <inheritdoc />
    public partial class addingtheforgetPasswordAPIs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                table: "VerificationCodes",
                newName: "CreateadAt");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "VerificationCodes",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "VerificationCodes");

            migrationBuilder.RenameColumn(
                name: "CreateadAt",
                table: "VerificationCodes",
                newName: "ExpiresAt");
        }
    }
}
