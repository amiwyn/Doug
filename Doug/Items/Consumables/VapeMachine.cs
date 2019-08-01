using Doug.Effects.Buffs;
using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class VapeMachine : ConsumableItem
    {
        public const string ItemId = "vape_machine";

        private readonly IEffectRepository _effectRepository;
        private readonly IStatsRepository _statsRepository;
        private const int DurationInMinutes = 5;

        public VapeMachine (IInventoryRepository inventoryRepository, IEffectRepository effectRepository) : base(inventoryRepository)
        {
            _effectRepository = effectRepository;
            Id = ItemId;
            Name = "Vape Machine";
            Description = "Some kind of hobo breathing mechanism. Gives +40 Health";
            Rarity = Rarity.Uncommon;
            Icon = ":smoking:";
            Price = 40;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            var effect = new SmellSweet();

            _effectRepository.AddEffect(user, SmellSweet.EffectId, DurationInMinutes);

            user.Health += 40;
            _statsRepository.UpdateHealth(user.Id, user.Health);


            return string.Format(DougMessages.AddedEffect, effect.Name, DurationInMinutes);
        }
    }
}
