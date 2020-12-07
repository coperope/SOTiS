using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class fixProblemMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_KnowledgeSpaces_KnowledgeSpaceId",
                table: "Problems");

            migrationBuilder.AlterColumn<int>(
                name: "KnowledgeSpaceId",
                table: "Problems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_KnowledgeSpaces_KnowledgeSpaceId",
                table: "Problems",
                column: "KnowledgeSpaceId",
                principalTable: "KnowledgeSpaces",
                principalColumn: "KnowledgeSpaceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_KnowledgeSpaces_KnowledgeSpaceId",
                table: "Problems");

            migrationBuilder.AlterColumn<int>(
                name: "KnowledgeSpaceId",
                table: "Problems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_KnowledgeSpaces_KnowledgeSpaceId",
                table: "Problems",
                column: "KnowledgeSpaceId",
                principalTable: "KnowledgeSpaces",
                principalColumn: "KnowledgeSpaceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
