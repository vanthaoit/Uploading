using Microsoft.EntityFrameworkCore.Migrations;

namespace KAS.Uploading.DataAccess.Migrations
{
    public partial class newInterfaceActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "kas",
                table: "ProductCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "kas",
                table: "Product",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "kas",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "kas",
                table: "Product");
        }
    }
}
