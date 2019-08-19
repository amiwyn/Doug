namespace Doug.Items.WeaponType
{
    public abstract class Bow : Weapon
    {
        protected Bow()
        {
            IsDualWield = true;
            Stats.AttackSpeed = 50;
            Slot = EquipmentSlot.RightHand;
        }
    }
}
