using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GLTV.Data.Migrations
{
    public partial class TvItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.CreateTable(
                name: "TvItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Author = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    TimeInserted = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvItem", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TvItemFile",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(nullable: true),
                    Length = table.Column<long>(nullable: false),
                    TvItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvItemFile", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TvItemFile_TvItem_TvItemId",
                        column: x => x.TvItemId,
                        principalTable: "TvItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TvItemLocation",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Location = table.Column<int>(nullable: false),
                    TvItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvItemLocation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TvItemLocation_TvItem_TvItemId",
                        column: x => x.TvItemId,
                        principalTable: "TvItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TvItemFile_TvItemId",
                table: "TvItemFile",
                column: "TvItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TvItemLocation_TvItemId",
                table: "TvItemLocation",
                column: "TvItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TvItemFile");

            migrationBuilder.DropTable(
                name: "TvItemLocation");

            migrationBuilder.DropTable(
                name: "TvItem");
        }
    }
}
