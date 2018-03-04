using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GLTV.Data.Migrations
{
    public partial class ClientEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientEvent",
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
                    table.PrimaryKey("PK_ClientEvent", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClientEvent_TvItemFile_TvItemFileId",
                        column: x => x.TvItemFileId,
                        principalTable: "TvItemFile",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientEvent_TvItemFileId",
                table: "ClientEvent",
                column: "TvItemFileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientEvent");
        }
    }
}
