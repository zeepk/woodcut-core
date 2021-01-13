using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet5_webapp.Migrations
{
    public partial class SkillMinigameStatRecordId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Minigame_StatRecord_StatRecordId",
                table: "Minigame");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_StatRecord_StatRecordId",
                table: "Skill");

            migrationBuilder.AlterColumn<int>(
                name: "StatRecordId",
                table: "Skill",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatRecordId",
                table: "Minigame",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Minigame_StatRecord_StatRecordId",
                table: "Minigame",
                column: "StatRecordId",
                principalTable: "StatRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_StatRecord_StatRecordId",
                table: "Skill",
                column: "StatRecordId",
                principalTable: "StatRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Minigame_StatRecord_StatRecordId",
                table: "Minigame");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_StatRecord_StatRecordId",
                table: "Skill");

            migrationBuilder.AlterColumn<int>(
                name: "StatRecordId",
                table: "Skill",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "StatRecordId",
                table: "Minigame",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Minigame_StatRecord_StatRecordId",
                table: "Minigame",
                column: "StatRecordId",
                principalTable: "StatRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_StatRecord_StatRecordId",
                table: "Skill",
                column: "StatRecordId",
                principalTable: "StatRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
