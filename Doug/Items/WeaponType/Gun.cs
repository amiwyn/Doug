namespace Doug.Items.WeaponType
{
    public abstract class Gun : Weapon
    {
        protected Gun()
        {
            IsDualWield = true;
            Stats.AttackSpeed = -50;
            Slot = EquipmentSlot.RightHand;
        }
    }
}
