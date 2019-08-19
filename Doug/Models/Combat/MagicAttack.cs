namespace Doug.Models.Combat
{
    public class MagicAttack : Attack
    {
        public MagicAttack(ICombatable attacker, int damage) : base(damage, attacker)
        {
        }
    }
}
