using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GloryScout.Data.Migrations
{
    /// <inheritdoc />
    public partial class finalSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClubName",
                table: "Scouts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClubName",
                table: "Scouts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
