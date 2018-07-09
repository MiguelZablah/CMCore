using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CMCore.Migrations
{
    public partial class AddThumbUrlAndAwsRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwsRegion",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbUrl",
                table: "Files",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwsRegion",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ThumbUrl",
                table: "Files");
        }
    }
}
