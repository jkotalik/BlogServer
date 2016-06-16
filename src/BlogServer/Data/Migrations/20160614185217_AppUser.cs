using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogServer.Data.Migrations
{
    public partial class AppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "BlogPost");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "BlogPost",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_OwnerId",
                table: "BlogPost",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPost_AspNetUsers_OwnerId",
                table: "BlogPost",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPost_AspNetUsers_OwnerId",
                table: "BlogPost");

            migrationBuilder.DropIndex(
                name: "IX_BlogPost_OwnerId",
                table: "BlogPost");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "BlogPost");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "BlogPost",
                nullable: true);
        }
    }
}
