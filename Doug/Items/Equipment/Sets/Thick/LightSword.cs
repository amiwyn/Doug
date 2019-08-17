using Doug.Items.WeaponType;

namespace Doug.Items.Equipment.Sets.Thick
{
    public class LightSword : Sword
    {
        public const string ItemId = "light_sword";

        public LightSword()
        {
            Id = ItemId;
            Name = "Light Sword";
            Description = "A sword made of lightweight material.";
            Rarity = Rarity.Common;
            Icon = ":sword1:";
            Slot = EquipmentSlot.RightHand;
            Price = 190;
            LevelRequirement = 10;

            Stats.MinAttack = 42;
            Stats.MaxAttack = 55;
            Stats.AttackSpeed = 20;
        }
    }
}
