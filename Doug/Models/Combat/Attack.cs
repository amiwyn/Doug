namespace Doug.Models.Combat
{
    public abstract class Attack
    {
        public ICombatable Attacker { get; set; }
        public int Damage { get; set; }
        public AttackStatus Status { get; set; }

        protected Attack(int damage, ICombatable attacker)
        {
            Damage = damage;
            Attacker = attacker;
        }
    }
}
