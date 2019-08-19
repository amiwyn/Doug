namespace Doug.Items.WeaponType
{
    public abstract class Staff : Weapon
    {
        protected Staff()
        {
            IsDualWield = true;
            Slot = EquipmentSlot.RightHand;
        }
    }
}
