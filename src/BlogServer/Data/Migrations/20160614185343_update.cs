using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogServer.Data.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VisitCount",
                table: "BlogPost",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "edited",
                table: "BlogPost",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitCount",
                table: "BlogPost");

            migrationBuilder.DropColumn(
                name: "edited",
                table: "BlogPost");
        }
    }
}
