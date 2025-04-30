using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GloryScout.Data.Migrations
{
    /// <inheritdoc />
    public partial class editingplayerandcoachtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileDescription",
                table: "Scouts");

            migrationBuilder.DropColumn(
                name: "ProfileDescription",
                table: "Players");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileDescription",
                table: "Scouts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileDescription",
                table: "Players",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
