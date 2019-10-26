using Doug.Models.Combat;
using Doug.Models.User;
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
            Description = "Im positively sure this is an actual leech worm. Heals 5% of damage dealt.";
            Rarity = Rarity.Rare;
            Icon = ":leech_amulet:";
            Slot = EquipmentSlot.Neck;
            Price = 2750;
            LevelRequirement = 30;

            Stats.Health = 120;
        }

        public override int OnAttacking(User attacker, ICombatable target, int damage)
        {
            attacker.Health += (int)(damage * 0.05);

            _statsRepository.UpdateHealth(attacker.Id, attacker.Health);

            return base.OnAttacking(attacker, target, damage);
        }
    }
}
