using Microsoft.EntityFrameworkCore.Migrations;

namespace CMCore.Migrations
{
    public partial class addPropertieToFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PathName",
                table: "Files",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathName",
                table: "Files");
        }
    }
}
