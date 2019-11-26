using Microsoft.EntityFrameworkCore.Migrations;

namespace Doug.Migrations
{
    public partial class Add_DroptableId_To_Lootitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LootItem_Droptables_DropTableId",
                table: "LootItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LootItem",
                table: "LootItem");

            migrationBuilder.AlterColumn<string>(
                name: "DropTableId",
                table: "LootItem",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LootItem",
                table: "LootItem",
                columns: new[] { "Id", "DropTableId" });

            migrationBuilder.AddForeignKey(
                name: "FK_LootItem_Droptables_DropTableId",
                table: "LootItem",
                column: "DropTableId",
                principalTable: "Droptables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LootItem_Droptables_DropTableId",
                table: "LootItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LootItem",
                table: "LootItem");

            migrationBuilder.AlterColumn<string>(
                name: "DropTableId",
                table: "LootItem",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_LootItem",
                table: "LootItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LootItem_Droptables_DropTableId",
                table: "LootItem",
                column: "DropTableId",
                principalTable: "Droptables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
