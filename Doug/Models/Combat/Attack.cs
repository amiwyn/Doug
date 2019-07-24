namespace Doug.Models.Combat
{
    public abstract class Attack
    {
        public int Damage { get; set; }
        public AttackStatus Status { get; set; }

        protected Attack(int damage)
        {
            Damage = damage;
        }
    }
}
