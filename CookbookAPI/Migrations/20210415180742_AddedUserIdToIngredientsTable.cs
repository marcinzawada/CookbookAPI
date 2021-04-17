using Microsoft.EntityFrameworkCore.Migrations;

namespace CookbookAPI.Migrations
{
    public partial class AddedUserIdToIngredientsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Ingredients",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_UserId",
                table: "Ingredients",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Users_UserId",
                table: "Ingredients",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Users_UserId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_UserId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ingredients");
        }
    }
}
