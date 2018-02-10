using Microsoft.EntityFrameworkCore.Migrations;

namespace CMCore.Migrations
{
    public partial class seedDBMain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[Clubs] ([Name]) VALUES ('Bebes Paradise')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Types] ([Name]) VALUES ('Mainstream')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Regions] ([Name]) VALUES ('Cenam')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Companies] ([Name]) VALUES ('SVA')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Extensions] ([Name]) VALUES ('JPG')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Tags] ([Name]) VALUES ('Sexy')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Clubs] WHERE name='Bebes Paradise'");
            migrationBuilder.Sql("DELETE FROM [dbo].[Types] WHERE name='Mainstream'");
            migrationBuilder.Sql("DELETE FROM [dbo].[Regions] WHERE name='Cenam'");
            migrationBuilder.Sql("DELETE FROM [dbo].[Companies] WHERE name='SVA'");
            migrationBuilder.Sql("DELETE FROM [dbo].[Extensions] WHERE name='JPG'");
            migrationBuilder.Sql("DELETE FROM [dbo].[Tags] WHERE name='Sexy'");
        }
    }
}
