namespace Doug.Items.WeaponType
{
    public class Bow : Weapon
    {
        public Bow()
        {
            IsDualWield = true;
            AttackSpeed = 50;
            Slot = EquipmentSlot.RightHand;
        }
    }
}
