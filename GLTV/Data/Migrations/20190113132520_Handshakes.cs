using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GLTV.Data.Migrations
{
    public partial class Handshakes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TvScreen",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    IpAddress = table.Column<string>(maxLength: 100, nullable: false),
                    LastHandshake = table.Column<DateTime>(nullable: false),
                    Location = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvScreen", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WebClientLog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(maxLength: 500, nullable: true),
                    Source = table.Column<string>(maxLength: 100, nullable: false),
                    TimeInserted = table.Column<DateTime>(nullable: false),
                    TvItemFileId = table.Column<int>(nullable: true),
                    Type = table.Column<int>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebClientLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WebClientLog_TvItemFile_TvItemFileId",
                        column: x => x.TvItemFileId,
                        principalTable: "TvItemFile",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WebServerLog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Author = table.Column<string>(maxLength: 100, nullable: false),
                    Message = table.Column<string>(maxLength: 500, nullable: true),
                    TimeInserted = table.Column<DateTime>(nullable: false),
                    TvItemId = table.Column<int>(nullable: true),
                    Type = table.Column<int>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebServerLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WebServerLog_TvItem_TvItemId",
                        column: x => x.TvItemId,
                        principalTable: "TvItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TvScreenHandshake",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstHandshake = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastHandshake = table.Column<DateTime>(nullable: false),
                    TvScreenId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvScreenHandshake", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TvScreenHandshake_TvScreen_TvScreenId",
                        column: x => x.TvScreenId,
                        principalTable: "TvScreen",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TvScreenHandshake_TvScreenId",
                table: "TvScreenHandshake",
                column: "TvScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_WebClientLog_TvItemFileId",
                table: "WebClientLog",
                column: "TvItemFileId");

            migrationBuilder.CreateIndex(
                name: "IX_WebServerLog_TvItemId",
                table: "WebServerLog",
                column: "TvItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TvScreenHandshake");

            migrationBuilder.DropTable(
                name: "WebClientLog");

            migrationBuilder.DropTable(
                name: "WebServerLog");

            migrationBuilder.DropTable(
                name: "TvScreen");
        }
    }
}
