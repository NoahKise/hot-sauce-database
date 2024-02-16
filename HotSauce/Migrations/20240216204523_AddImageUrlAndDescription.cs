using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotSauce.Migrations
{
    public partial class AddImageUrlAndDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sauces",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Sauces",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sauces");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Sauces");
        }
    }
}
