using Microsoft.EntityFrameworkCore.Migrations;

namespace CMCore.Migrations
{
    public partial class seedDBMissinRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[Countries] ([Name], [RegionId]) VALUES ('Costa Rica', 12)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Files] ([Name] ,[Description], [ExtensionId]) VALUES ('FileTest', 'Testing File', 14)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Countries] WHERE name='Costa Rica'");
            migrationBuilder.Sql("DELETE FROM [dbo].[Files] WHERE name='FileTest'");
        }
    }
}
