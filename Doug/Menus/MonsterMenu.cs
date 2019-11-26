using System.Collections.Generic;
using Doug.Menus.Blocks;
using Doug.Menus.Blocks.Accessories;
using Doug.Menus.Blocks.Text;
using Doug.Models.Monsters;

namespace Doug.Menus
{
    public class MonsterMenu
    {
        public List<Block> Blocks { get; set; }

        public MonsterMenu(SpawnedMonster spawnedMonster)
        {
            var monster = spawnedMonster.Monster;
            var monsterText = new MarkdownText($"*{monster.Name}* `lvl {monster.Level}` \n *{spawnedMonster.Health}*/{monster.Health} \n {monster.Description}");
            var monsterDescription = new Section(monsterText, new Image(monster.Image, monster.Name));

            Blocks = new List<Block>
            {
                monsterDescription,
                new ActionList(new List<Accessory>
                {
                    new Button(DougMessages.AttackAction, spawnedMonster.Id.ToString(), Actions.Attack.ToString()),
                    new Button(DougMessages.SkillAction, spawnedMonster.Id.ToString(), Actions.Skill.ToString())
                })
            };
        }
    }
}
