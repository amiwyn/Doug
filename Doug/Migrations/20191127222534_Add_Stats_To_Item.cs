using Microsoft.EntityFrameworkCore.Migrations;

namespace Doug.Migrations
{
    public partial class Add_Stats_To_Item : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxHealth",
                table: "Monsters");

            migrationBuilder.AddColumn<int>(
                name: "CriticalFactor",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefenseFactor",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnergyFactor",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HealthFactor",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CriticalFactor",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DefenseFactor",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "EnergyFactor",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "HealthFactor",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "MaxHealth",
                table: "Monsters",
                nullable: false,
                defaultValue: 0);
        }
    }
}
