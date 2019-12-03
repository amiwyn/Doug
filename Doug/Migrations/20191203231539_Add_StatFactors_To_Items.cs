using Microsoft.EntityFrameworkCore.Migrations;

namespace Doug.Migrations
{
    public partial class Add_StatFactors_To_Items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CriticalFactor",
                table: "Items",
                newName: "StrengthFactor");

            migrationBuilder.AddColumn<int>(
                name: "AgilityFactor",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttackSpeedFactor",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConstitutionFactor",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CooldownReduction",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CriticalDamageFactor",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CriticalHitChanceFactor",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlatEnergyRegen",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlatHealthRegen",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HitRateFactor",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntelligenceFactor",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LuckFactor",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Pierce",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PierceFactor",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgilityFactor",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "AttackSpeedFactor",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ConstitutionFactor",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CooldownReduction",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CriticalDamageFactor",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CriticalHitChanceFactor",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "FlatEnergyRegen",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "FlatHealthRegen",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "HitRateFactor",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IntelligenceFactor",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LuckFactor",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Pierce",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "PierceFactor",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "StrengthFactor",
                table: "Items",
                newName: "CriticalFactor");
        }
    }
}
