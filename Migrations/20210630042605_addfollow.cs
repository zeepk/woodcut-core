using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet5_webapp.Migrations
{
    public partial class addfollow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_User_UserId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_StatRecord_User_UserId",
                table: "StatRecord");

            migrationBuilder.DropIndex(
                name: "IX_StatRecord_UserId",
                table: "StatRecord");

            migrationBuilder.DropIndex(
                name: "IX_Activity_UserId",
                table: "Activity");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "StatRecord",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "Activity",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Follow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follow_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Follow_User_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatRecord_PlayerId",
                table: "StatRecord",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_PlayerId",
                table: "Activity",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Follow_PlayerId",
                table: "Follow",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Follow_UserId",
                table: "Follow",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_User_PlayerId",
                table: "Activity",
                column: "PlayerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StatRecord_User_PlayerId",
                table: "StatRecord",
                column: "PlayerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_User_PlayerId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_StatRecord_User_PlayerId",
                table: "StatRecord");

            migrationBuilder.DropTable(
                name: "Follow");

            migrationBuilder.DropIndex(
                name: "IX_StatRecord_PlayerId",
                table: "StatRecord");

            migrationBuilder.DropIndex(
                name: "IX_Activity_PlayerId",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "StatRecord");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Activity");

            migrationBuilder.CreateIndex(
                name: "IX_StatRecord_UserId",
                table: "StatRecord",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_UserId",
                table: "Activity",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_User_UserId",
                table: "Activity",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StatRecord_User_UserId",
                table: "StatRecord",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
