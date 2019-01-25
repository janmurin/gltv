using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GLTV.Data.Migrations
{
    public partial class WebClientLogTvScreenId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TvScreenId",
                table: "WebClientLog",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebClientLog_TvScreenId",
                table: "WebClientLog",
                column: "TvScreenId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebClientLog_TvScreen_TvScreenId",
                table: "WebClientLog",
                column: "TvScreenId",
                principalTable: "TvScreen",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebClientLog_TvScreen_TvScreenId",
                table: "WebClientLog");

            migrationBuilder.DropIndex(
                name: "IX_WebClientLog_TvScreenId",
                table: "WebClientLog");

            migrationBuilder.DropColumn(
                name: "TvScreenId",
                table: "WebClientLog");
        }
    }
}
