using Microsoft.EntityFrameworkCore.Migrations;

namespace Doug.Migrations
{
    public partial class Add_Dodge_Factor_To_Items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DodgeFactor",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DodgeFactor",
                table: "Items");
        }
    }
}
