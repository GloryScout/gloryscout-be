using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GloryScout.Data.Migrations
{
    /// <inheritdoc />
    public partial class addingpayment2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthTokenResponsess",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthTokenResponsess", x => x.Token);
                });

            migrationBuilder.CreateTable(
                name: "OrderResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderResponses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paymentkeys",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paymentkeys", x => x.Token);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthTokenResponsess");

            migrationBuilder.DropTable(
                name: "OrderResponses");

            migrationBuilder.DropTable(
                name: "Paymentkeys");
        }
    }
}
