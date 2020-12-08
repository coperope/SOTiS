using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class ConntectTestKSMgration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KnowledgeSpaces_Tests_TestId",
                table: "KnowledgeSpaces");

            migrationBuilder.DropIndex(
                name: "IX_KnowledgeSpaces_TestId",
                table: "KnowledgeSpaces");

            migrationBuilder.AddColumn<int>(
                name: "KnowledgeSpaceId",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KnowledgeSpaceId1",
                table: "Tests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProblemId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tests_KnowledgeSpaceId1",
                table: "Tests",
                column: "KnowledgeSpaceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_KnowledgeSpaces_KnowledgeSpaceId1",
                table: "Tests",
                column: "KnowledgeSpaceId1",
                principalTable: "KnowledgeSpaces",
                principalColumn: "KnowledgeSpaceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_KnowledgeSpaces_KnowledgeSpaceId1",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_KnowledgeSpaceId1",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "KnowledgeSpaceId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "KnowledgeSpaceId1",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "ProblemId",
                table: "Questions");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeSpaces_TestId",
                table: "KnowledgeSpaces",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_KnowledgeSpaces_Tests_TestId",
                table: "KnowledgeSpaces",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
