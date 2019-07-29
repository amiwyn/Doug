namespace Doug.Models.Combat
{
    public class MagicAttack : Attack
    {
        public MagicAttack(ICombatable attacker, int intelligence) : base(intelligence * 2, attacker)
        {
        }
    }
}
