namespace Doug.Items.WeaponType
{
    public class Staff : Weapon
    {
        public Staff()
        {
            IsDualWield = true;
            Slot = EquipmentSlot.RightHand;
        }
    }
}
