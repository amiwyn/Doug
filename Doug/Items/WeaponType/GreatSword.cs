namespace Doug.Items.WeaponType
{
    public abstract class GreatSword : Weapon
    {
        protected GreatSword()
        {
            IsDualWield = true;
            Stats.AttackSpeed = -20;
            Slot = EquipmentSlot.RightHand;
        }
    }
}
