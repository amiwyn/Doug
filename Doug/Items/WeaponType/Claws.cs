namespace Doug.Items.WeaponType
{
    public abstract class Claws : Weapon
    {
        protected Claws()
        {
            IsDualWield = true;
            Stats.AttackSpeed = 80;
        }
    }
}
