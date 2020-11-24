using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class ConnEnrolEnrolAnswerTypeTestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMultipleChoice",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "IsMultipleChoice",
                table: "Questions");
        }
    }
}
