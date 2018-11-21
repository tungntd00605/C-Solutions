using Microsoft.EntityFrameworkCore.Migrations;

namespace MyMusic.Migrations
{
    public partial class UpdateSongModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Singer",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Songs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Singer",
                table: "Songs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "Singer",
                table: "Songs");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Singer",
                table: "Categories",
                nullable: true);
        }
    }
}
