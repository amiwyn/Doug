using Doug.Models;
using Doug.Models.Combat;
using Doug.Repositories;

namespace Doug.Items.Equipment.Necklaces
{
    public class LeechAmulet : EquipmentItem
    {
        private readonly IStatsRepository _statsRepository;
        public const string ItemId = "leech_amulet";

        public LeechAmulet(IStatsRepository statsRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemId;
            Name = "Leech Amulet";
            Description = "Im positively sure this is an actual leech worm. Heals 10% of damage taken.";
            Rarity = Rarity.Rare;
            Icon = ":leech_amulet:";
            Slot = EquipmentSlot.Neck;
            Price = 2750;
            LevelRequirement = 30;

            Stats.Health = 120;
        }

        public override int OnGettingAttacked(ICombatable attacker, User target, int damage)
        {
            target.Health += (int)(damage * 0.1);

            _statsRepository.UpdateHealth(target.Id, target.Health);

            return base.OnGettingAttacked(attacker, target, damage);
        }
    }
}
