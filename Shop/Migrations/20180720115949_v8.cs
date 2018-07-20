using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Shop.Migrations
{
    public partial class v8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "GroupProducts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CategoryProducts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserId",
                table: "Products",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupProducts_UserId",
                table: "GroupProducts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProducts_UserId",
                table: "CategoryProducts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProducts_AspNetUsers_UserId",
                table: "CategoryProducts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupProducts_AspNetUsers_UserId",
                table: "GroupProducts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_UserId",
                table: "Products",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProducts_AspNetUsers_UserId",
                table: "CategoryProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupProducts_AspNetUsers_UserId",
                table: "GroupProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_UserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_GroupProducts_UserId",
                table: "GroupProducts");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProducts_UserId",
                table: "CategoryProducts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GroupProducts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CategoryProducts");
        }
    }
}
