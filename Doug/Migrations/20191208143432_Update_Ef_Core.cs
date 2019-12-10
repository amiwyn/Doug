using Microsoft.EntityFrameworkCore.Migrations;

namespace Doug.Migrations
{
    public partial class Update_Ef_Core : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_MonsterAttacker_SpawnedMonsterId_UserId",
                table: "MonsterAttacker");

            migrationBuilder.CreateIndex(
                name: "IX_MonsterAttacker_SpawnedMonsterId",
                table: "MonsterAttacker",
                column: "SpawnedMonsterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MonsterAttacker_SpawnedMonsterId",
                table: "MonsterAttacker");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_MonsterAttacker_SpawnedMonsterId_UserId",
                table: "MonsterAttacker",
                columns: new[] { "SpawnedMonsterId", "UserId" });
        }
    }
}
