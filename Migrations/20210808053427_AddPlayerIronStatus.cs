using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet5_webapp.Migrations
{
    public partial class AddPlayerIronStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IronmanStatus",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IronmanStatus",
                table: "User");
        }
    }
}
