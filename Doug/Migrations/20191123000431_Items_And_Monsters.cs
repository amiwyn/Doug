using Microsoft.EntityFrameworkCore.Migrations;

namespace Doug.Migrations
{
    public partial class Items_And_Monsters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Boots",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Gloves",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Head",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LeftHand",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Neck",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RightHand",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Skill",
                table: "Users",
                newName: "LoadoutId");

            migrationBuilder.AlterColumn<string>(
                name: "LoadoutId",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MonsterId",
                table: "SpawnedMonsters",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "InventoryItem",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Channels",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Droptables",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Droptables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Rarity = table.Column<int>(nullable: false),
                    Icon = table.Column<string>(nullable: true),
                    MaxStack = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    IsTradable = table.Column<bool>(nullable: false),
                    IsSellable = table.Column<bool>(nullable: false),
                    ActionId = table.Column<string>(nullable: true),
                    TargetActionId = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    HealthAmount = table.Column<int>(nullable: true),
                    EnergyAmount = table.Column<int>(nullable: true),
                    SpecialFood_EffectId = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: true),
                    DropTableId = table.Column<string>(nullable: true),
                    Channel = table.Column<string>(nullable: true),
                    EffectId = table.Column<string>(nullable: true),
                    Slot = table.Column<int>(nullable: true),
                    LuckRequirement = table.Column<int>(nullable: true),
                    AgilityRequirement = table.Column<int>(nullable: true),
                    StrengthRequirement = table.Column<int>(nullable: true),
                    IntelligenceRequirement = table.Column<int>(nullable: true),
                    ConstitutionRequirement = table.Column<int>(nullable: true),
                    LevelRequirement = table.Column<int>(nullable: true),
                    Health = table.Column<int>(nullable: true),
                    Energy = table.Column<int>(nullable: true),
                    MaxAttack = table.Column<int>(nullable: true),
                    MinAttack = table.Column<int>(nullable: true),
                    Defense = table.Column<int>(nullable: true),
                    Dodge = table.Column<int>(nullable: true),
                    Hitrate = table.Column<int>(nullable: true),
                    AttackSpeed = table.Column<int>(nullable: true),
                    Resistance = table.Column<int>(nullable: true),
                    Luck = table.Column<int>(nullable: true),
                    Agility = table.Column<int>(nullable: true),
                    Strength = table.Column<int>(nullable: true),
                    Constitution = table.Column<int>(nullable: true),
                    Intelligence = table.Column<int>(nullable: true),
                    HealthRegen = table.Column<int>(nullable: true),
                    EnergyRegen = table.Column<int>(nullable: true),
                    SkillId = table.Column<string>(nullable: true),
                    IsDualWield = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Droptables_DropTableId",
                        column: x => x.DropTableId,
                        principalTable: "Droptables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LootItem",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Probability = table.Column<double>(nullable: false),
                    DropTableId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LootItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LootItem_Droptables_DropTableId",
                        column: x => x.DropTableId,
                        principalTable: "Droptables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Monsters",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Health = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    ExperienceValue = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    DamageType = table.Column<int>(nullable: false),
                    DropTableId = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MaxHealth = table.Column<int>(nullable: false),
                    MinAttack = table.Column<int>(nullable: false),
                    MaxAttack = table.Column<int>(nullable: false),
                    Hitrate = table.Column<int>(nullable: false),
                    Dodge = table.Column<int>(nullable: false),
                    Resistance = table.Column<int>(nullable: false),
                    Defense = table.Column<int>(nullable: false),
                    AttackCooldown = table.Column<int>(nullable: false),
                    CriticalHitChance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monsters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Monsters_Droptables_DropTableId",
                        column: x => x.DropTableId,
                        principalTable: "Droptables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Loadout",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HeadId = table.Column<string>(nullable: true),
                    BodyId = table.Column<string>(nullable: true),
                    BootsId = table.Column<string>(nullable: true),
                    GlovesId = table.Column<string>(nullable: true),
                    LeftHandId = table.Column<string>(nullable: true),
                    RightHandId = table.Column<string>(nullable: true),
                    NeckId = table.Column<string>(nullable: true),
                    LeftRingId = table.Column<string>(nullable: true),
                    RightRingId = table.Column<string>(nullable: true),
                    SkillbookId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loadout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loadout_Items_BodyId",
                        column: x => x.BodyId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loadout_Items_BootsId",
                        column: x => x.BootsId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loadout_Items_GlovesId",
                        column: x => x.GlovesId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loadout_Items_HeadId",
                        column: x => x.HeadId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loadout_Items_LeftHandId",
                        column: x => x.LeftHandId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loadout_Items_LeftRingId",
                        column: x => x.LeftRingId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loadout_Items_NeckId",
                        column: x => x.NeckId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loadout_Items_RightHandId",
                        column: x => x.RightHandId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loadout_Items_RightRingId",
                        column: x => x.RightRingId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loadout_Items_SkillbookId",
                        column: x => x.SkillbookId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_LoadoutId",
                table: "Users",
                column: "LoadoutId");

            migrationBuilder.CreateIndex(
                name: "IX_SpawnedMonsters_MonsterId",
                table: "SpawnedMonsters",
                column: "MonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_ItemId",
                table: "InventoryItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_DropTableId",
                table: "Items",
                column: "DropTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Loadout_BodyId",
                table: "Loadout",
                column: "BodyId");

            migrationBuilder.CreateIndex(
                name: "IX_Loadout_BootsId",
                table: "Loadout",
                column: "BootsId");

            migrationBuilder.CreateIndex(
                name: "IX_Loadout_GlovesId",
                table: "Loadout",
                column: "GlovesId");

            migrationBuilder.CreateIndex(
                name: "IX_Loadout_HeadId",
                table: "Loadout",
                column: "HeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Loadout_LeftHandId",
                table: "Loadout",
                column: "LeftHandId");

            migrationBuilder.CreateIndex(
                name: "IX_Loadout_LeftRingId",
                table: "Loadout",
                column: "LeftRingId");

            migrationBuilder.CreateIndex(
                name: "IX_Loadout_NeckId",
                table: "Loadout",
                column: "NeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Loadout_RightHandId",
                table: "Loadout",
                column: "RightHandId");

            migrationBuilder.CreateIndex(
                name: "IX_Loadout_RightRingId",
                table: "Loadout",
                column: "RightRingId");

            migrationBuilder.CreateIndex(
                name: "IX_Loadout_SkillbookId",
                table: "Loadout",
                column: "SkillbookId");

            migrationBuilder.CreateIndex(
                name: "IX_LootItem_DropTableId",
                table: "LootItem",
                column: "DropTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Monsters_DropTableId",
                table: "Monsters",
                column: "DropTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Items_ItemId",
                table: "InventoryItem",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SpawnedMonsters_Monsters_MonsterId",
                table: "SpawnedMonsters",
                column: "MonsterId",
                principalTable: "Monsters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Loadout_LoadoutId",
                table: "Users",
                column: "LoadoutId",
                principalTable: "Loadout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Items_ItemId",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_SpawnedMonsters_Monsters_MonsterId",
                table: "SpawnedMonsters");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Loadout_LoadoutId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Loadout");

            migrationBuilder.DropTable(
                name: "LootItem");

            migrationBuilder.DropTable(
                name: "Monsters");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Droptables");

            migrationBuilder.DropIndex(
                name: "IX_Users_LoadoutId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_SpawnedMonsters_MonsterId",
                table: "SpawnedMonsters");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_ItemId",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Channels");

            migrationBuilder.RenameColumn(
                name: "LoadoutId",
                table: "Users",
                newName: "Skill");

            migrationBuilder.AlterColumn<string>(
                name: "Skill",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Boots",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gloves",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Head",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LeftHand",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Neck",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RightHand",
                table: "Users",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MonsterId",
                table: "SpawnedMonsters",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "InventoryItem",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
