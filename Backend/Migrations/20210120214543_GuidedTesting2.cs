using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class GuidedTesting2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "statePosibility",
                table: "Problems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "statePosibility",
                table: "Problems");
        }
    }
}
