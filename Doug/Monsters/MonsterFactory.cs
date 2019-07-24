using Doug.Monsters.Seagulls;

namespace Doug.Monsters
{
    public interface IMonsterFactory
    {
        Monster CreateMonster(string monsterId);
    }

    public class MonsterFactory : IMonsterFactory
    {
        public Monster CreateMonster(string monsterId)
        {
            switch (monsterId)
            {
                case Seagull.MonsterId: return new Seagull();
                default: return new Seagull();
            }
        }
    }
}
