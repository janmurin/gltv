using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GLTV.Data.Migrations
{
    public partial class UserFilter2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilterData",
                table: "UserFilter");

            migrationBuilder.AddColumn<string>(
                name: "FilterDataJson",
                table: "UserFilter",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilterDataJson",
                table: "UserFilter");

            migrationBuilder.AddColumn<string>(
                name: "FilterData",
                table: "UserFilter",
                nullable: true);
        }
    }
}
