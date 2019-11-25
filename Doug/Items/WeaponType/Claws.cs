namespace Doug.Items.WeaponType
{
    public class Claws : Weapon
    {
        public Claws()
        {
            IsDualWield = true;
            AttackSpeed = 80;
            Slot = EquipmentSlot.RightHand;
        }
    }
}
