using Microsoft.EntityFrameworkCore.Migrations;

namespace CMCore.Migrations
{
    public partial class seedDbManyToManyRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[ClubTypes] ([ClubId] ,[TypeId]) VALUES (19, 10)");
            migrationBuilder.Sql("INSERT INTO [dbo].[ClubRegions] ([ClubId] ,[RegionId]) VALUES (19, 12)");
            migrationBuilder.Sql("INSERT INTO [dbo].[FileClubs] ([FileId] ,[ClubId]) VALUES (7, 19)");
            migrationBuilder.Sql("INSERT INTO [dbo].[FileCompanies] ([FileId] ,[CompanieId]) VALUES (7, 16)");
            migrationBuilder.Sql("INSERT INTO [dbo].[FileTags] ([FileId] ,[TagId]) VALUES (7, 5)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
