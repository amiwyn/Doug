namespace Doug.Items.WeaponType
{
    public abstract class Shield : Weapon
    {
        protected Shield()
        {
            IsDualWield = false;
            Slot = EquipmentSlot.LeftHand;
        }
    }
}
