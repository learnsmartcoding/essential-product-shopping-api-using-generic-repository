using Microsoft.EntityFrameworkCore.Migrations;

namespace LearnSmartCoding.EssentialProducts.Data.Migrations
{
    public partial class productowner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductOwnerId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductOwnerId",
                table: "Product",
                column: "ProductOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductOwner_ProductOwnerId",
                table: "Product",
                column: "ProductOwnerId",
                principalTable: "ProductOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductOwner_ProductOwnerId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductOwnerId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductOwnerId",
                table: "Product");
        }
    }
}
