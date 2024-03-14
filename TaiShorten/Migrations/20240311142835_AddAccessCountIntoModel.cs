using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaiShorten.Migrations
{
    public partial class AddAccessCountIntoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessCount",
                table: "shortUrls",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessCount",
                table: "shortUrls");
        }
    }
}
