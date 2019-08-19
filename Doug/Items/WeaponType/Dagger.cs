namespace Doug.Items.WeaponType
{
    public abstract class Dagger : Weapon
    {
        protected Dagger()
        {
            IsDualWield = false;
            Slot = EquipmentSlot.LeftHand;
        }
    }
}
