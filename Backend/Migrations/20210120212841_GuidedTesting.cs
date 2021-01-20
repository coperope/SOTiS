using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class GuidedTesting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PossibleStatesWithPossibilitiesId",
                table: "Problems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PossibleStatesWithPossibilities",
                columns: table => new
                {
                    PossibleStatesWithPossibilitiesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KnowledgeSpaceId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PossibleStatesWithPossibilities", x => x.PossibleStatesWithPossibilitiesId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Problems_PossibleStatesWithPossibilitiesId",
                table: "Problems",
                column: "PossibleStatesWithPossibilitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_PossibleStatesWithPossibilities_PossibleStatesWithPossibilitiesId",
                table: "Problems",
                column: "PossibleStatesWithPossibilitiesId",
                principalTable: "PossibleStatesWithPossibilities",
                principalColumn: "PossibleStatesWithPossibilitiesId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_PossibleStatesWithPossibilities_PossibleStatesWithPossibilitiesId",
                table: "Problems");

            migrationBuilder.DropTable(
                name: "PossibleStatesWithPossibilities");

            migrationBuilder.DropIndex(
                name: "IX_Problems_PossibleStatesWithPossibilitiesId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "PossibleStatesWithPossibilitiesId",
                table: "Problems");
        }
    }
}
