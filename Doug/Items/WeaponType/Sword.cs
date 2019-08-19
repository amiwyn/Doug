namespace Doug.Items.WeaponType
{
    public abstract class Sword : Weapon
    {
        protected Sword()
        {
            IsDualWield = false;
            Slot = EquipmentSlot.RightHand;
        }
    }
}
