namespace Doug.Items.WeaponType
{
    public class Gun : Weapon
    {
        public Gun()
        {
            IsDualWield = true;
            AttackSpeed = -50;
            Slot = EquipmentSlot.RightHand;
        }
    }
}
