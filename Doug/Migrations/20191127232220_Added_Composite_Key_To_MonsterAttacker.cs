using Microsoft.EntityFrameworkCore.Migrations;

namespace Doug.Migrations
{
    public partial class Added_Composite_Key_To_MonsterAttacker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MonsterAttacker_SpawnedMonsterId",
                table: "MonsterAttacker");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_MonsterAttacker_SpawnedMonsterId_UserId",
                table: "MonsterAttacker",
                columns: new[] { "SpawnedMonsterId", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_MonsterAttacker_SpawnedMonsterId_UserId",
                table: "MonsterAttacker");

            migrationBuilder.CreateIndex(
                name: "IX_MonsterAttacker_SpawnedMonsterId",
                table: "MonsterAttacker",
                column: "SpawnedMonsterId");
        }
    }
}
