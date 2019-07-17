using Doug.Effects.Buffs;
using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class PicklePickle : ConsumableItem
    {
        public const string ItemId = "pickle_pickle";

        private readonly IEffectRepository _effectRepository;
        private readonly IStatsRepository _statsRepository;
        private const int DurationInMinutes = 15;

        public PicklePickle(IInventoryRepository inventoryRepository, IEffectRepository effectRepository, IStatsRepository statsRepository) : base(inventoryRepository)
        {
            _effectRepository = effectRepository;
            _statsRepository = statsRepository;
            Id = ItemId;
            Name = "Pickle's pickle";
            Description = "Dill with it. Gives +100 HP and +15 Def for 15 minutes";
            Rarity = Rarity.Uncommon;
            Icon = ":pickle:";
            Price = 100;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            var effect = new PickleBuff();

            _effectRepository.AddEffect(user, PickleBuff.EffectId, DurationInMinutes);

            user.Health += 100;
            _statsRepository.UpdateHealth(user.Id, user.Health);

            return string.Format(DougMessages.AddedEffect, effect.Name, DurationInMinutes);
        }
    }
}
