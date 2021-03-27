using Microsoft.EntityFrameworkCore.Migrations;

namespace CookbookAPI.Migrations
{
    public partial class AddedDescriptionToIngredient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Ingredients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Ingredients");
        }
    }
}
