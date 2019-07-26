namespace Doug.Models.Combat
{
    public class MagicAttack : Attack
    {
        public MagicAttack(int intelligence) : base(intelligence * 2)
        {
        }
    }
}
