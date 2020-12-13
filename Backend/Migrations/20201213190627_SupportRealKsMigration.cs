using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class SupportRealKsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpectedKnowledgeSpace",
                table: "KnowledgeSpaces",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReal",
                table: "KnowledgeSpaces",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedKnowledgeSpace",
                table: "KnowledgeSpaces");

            migrationBuilder.DropColumn(
                name: "IsReal",
                table: "KnowledgeSpaces");
        }
    }
}
