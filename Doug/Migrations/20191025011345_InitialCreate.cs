using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Doug.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeBreak",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BotToken = table.Column<string>(nullable: true),
                    UserToken = table.Column<string>(nullable: true),
                    CoffeeRemindJobId = table.Column<string>(nullable: true),
                    FatCounter = table.Column<int>(nullable: false),
                    IsCoffeeBreak = table.Column<bool>(nullable: false),
                    LastCoffee = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeBreak", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GambleChallenges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequesterId = table.Column<string>(nullable: true),
                    TargetId = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GambleChallenges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Government",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ruler = table.Column<string>(nullable: true),
                    TaxRate = table.Column<double>(nullable: false),
                    RevolutionLeader = table.Column<string>(nullable: true),
                    RevolutionTimestamp = table.Column<string>(nullable: true),
                    RevolutionCooldown = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Government", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecentSlurs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TimeStamp = table.Column<string>(nullable: true),
                    SlurId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecentSlurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roster",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsReady = table.Column<bool>(nullable: false),
                    IsSkipping = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PriceMultiplier = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Slurs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpawnedMonsters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MonsterId = table.Column<string>(nullable: true),
                    Channel = table.Column<string>(nullable: true),
                    Health = table.Column<int>(nullable: false),
                    AttackCooldown = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpawnedMonsters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Head = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Boots = table.Column<string>(nullable: true),
                    Gloves = table.Column<string>(nullable: true),
                    LeftHand = table.Column<string>(nullable: true),
                    RightHand = table.Column<string>(nullable: true),
                    Neck = table.Column<string>(nullable: true),
                    Skill = table.Column<string>(nullable: true),
                    Credits = table.Column<int>(nullable: false),
                    Experience = table.Column<long>(nullable: false),
                    AttackCooldown = table.Column<DateTime>(nullable: false),
                    SkillCooldown = table.Column<DateTime>(nullable: false),
                    Luck = table.Column<int>(nullable: false),
                    Agility = table.Column<int>(nullable: false),
                    Strength = table.Column<int>(nullable: false),
                    Constitution = table.Column<int>(nullable: false),
                    Intelligence = table.Column<int>(nullable: false),
                    Health = table.Column<int>(nullable: false),
                    Energy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionMonster",
                columns: table => new
                {
                    ChannelId = table.Column<string>(nullable: false),
                    MonsterId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionMonster", x => new { x.ChannelId, x.MonsterId });
                    table.ForeignKey(
                        name: "FK_RegionMonster_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopItem",
                columns: table => new
                {
                    ShopId = table.Column<string>(nullable: false),
                    ItemId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItem", x => new { x.ShopId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_ShopItem_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItem",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    InventoryPosition = table.Column<int>(nullable: false),
                    ItemId = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItem", x => new { x.UserId, x.InventoryPosition });
                    table.ForeignKey(
                        name: "FK_InventoryItem_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonsterAttacker",
                columns: table => new
                {
                    SpawnedMonsterId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    DamageDealt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterAttacker", x => new { x.UserId, x.SpawnedMonsterId });
                    table.ForeignKey(
                        name: "FK_MonsterAttacker_SpawnedMonsters_SpawnedMonsterId",
                        column: x => x.SpawnedMonsterId,
                        principalTable: "SpawnedMonsters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonsterAttacker_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEffect",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    EffectId = table.Column<string>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEffect", x => new { x.UserId, x.EffectId });
                    table.ForeignKey(
                        name: "FK_UserEffect_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonsterAttacker_SpawnedMonsterId",
                table: "MonsterAttacker",
                column: "SpawnedMonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_MonsterAttacker_UserId",
                table: "MonsterAttacker",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeBreak");

            migrationBuilder.DropTable(
                name: "GambleChallenges");

            migrationBuilder.DropTable(
                name: "Government");

            migrationBuilder.DropTable(
                name: "InventoryItem");

            migrationBuilder.DropTable(
                name: "MonsterAttacker");

            migrationBuilder.DropTable(
                name: "RecentSlurs");

            migrationBuilder.DropTable(
                name: "RegionMonster");

            migrationBuilder.DropTable(
                name: "Roster");

            migrationBuilder.DropTable(
                name: "ShopItem");

            migrationBuilder.DropTable(
                name: "Slurs");

            migrationBuilder.DropTable(
                name: "UserEffect");

            migrationBuilder.DropTable(
                name: "SpawnedMonsters");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
