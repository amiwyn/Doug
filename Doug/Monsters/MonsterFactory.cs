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
            throw new System.NotImplementedException();
        }
    }
}
