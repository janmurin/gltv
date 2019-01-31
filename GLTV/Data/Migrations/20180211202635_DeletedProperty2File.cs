using Microsoft.EntityFrameworkCore.Migrations;

namespace GLTV.Data.Migrations
{
    public partial class DeletedProperty2File : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "TvItemFile",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "TvItemFile");
        }
    }
}
