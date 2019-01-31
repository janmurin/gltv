using Microsoft.EntityFrameworkCore.Migrations;

namespace GLTV.Data.Migrations
{
    public partial class LogEventUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "LogEvent",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "TvItemId",
                table: "LogEvent",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogEvent_TvItemId",
                table: "LogEvent",
                column: "TvItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogEvent_TvItem_TvItemId",
                table: "LogEvent",
                column: "TvItemId",
                principalTable: "TvItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogEvent_TvItem_TvItemId",
                table: "LogEvent");

            migrationBuilder.DropIndex(
                name: "IX_LogEvent_TvItemId",
                table: "LogEvent");

            migrationBuilder.DropColumn(
                name: "TvItemId",
                table: "LogEvent");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "LogEvent",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
