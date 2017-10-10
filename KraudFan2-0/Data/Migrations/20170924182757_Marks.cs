using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KraudFan2_0.Data.Migrations
{
    public partial class Marks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_AspNetUsers_ToUserId",
                table: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Rates_ToUserId",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "Rates");

            migrationBuilder.AddColumn<int>(
                name: "Mark",
                table: "Rates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mark",
                table: "Rates");

            migrationBuilder.AddColumn<string>(
                name: "ToUserId",
                table: "Rates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rates_ToUserId",
                table: "Rates",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_AspNetUsers_ToUserId",
                table: "Rates",
                column: "ToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
