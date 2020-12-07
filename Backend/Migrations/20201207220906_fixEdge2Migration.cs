using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class fixEdge2Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Edges_KnowledgeSpaces_KnowledgeSpaceId",
                table: "Edges");

            migrationBuilder.AlterColumn<int>(
                name: "KnowledgeSpaceId",
                table: "Edges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Edges_KnowledgeSpaces_KnowledgeSpaceId",
                table: "Edges",
                column: "KnowledgeSpaceId",
                principalTable: "KnowledgeSpaces",
                principalColumn: "KnowledgeSpaceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Edges_KnowledgeSpaces_KnowledgeSpaceId",
                table: "Edges");

            migrationBuilder.AlterColumn<int>(
                name: "KnowledgeSpaceId",
                table: "Edges",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Edges_KnowledgeSpaces_KnowledgeSpaceId",
                table: "Edges",
                column: "KnowledgeSpaceId",
                principalTable: "KnowledgeSpaces",
                principalColumn: "KnowledgeSpaceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
