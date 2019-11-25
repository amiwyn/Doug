namespace Doug.Items.WeaponType
{
    public class GreatSword : Weapon
    {
        public GreatSword()
        {
            IsDualWield = true;
            AttackSpeed = -20;
            Slot = EquipmentSlot.RightHand;
        }
    }
}
