using Doug.Effects;
using Doug.Effects.Buffs;
using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class Cigarette : ConsumableItem
    {
        public const string ItemId = "cigarette";

        private readonly IEffectRepository _effectRepository;
        private const int DurationInMinutes = 5;

        public Cigarette(IInventoryRepository inventoryRepository, IEffectRepository effectRepository) : base(inventoryRepository)
        {
            _effectRepository = effectRepository;
            Id = ItemId;
            Name = "Cigarette";
            Description = "A cigarette that probably belonged to Vapane. Gives *Nicotine High* for 5 minutes";
            Rarity = Rarity.Uncommon;
            Icon = ":smoking:";
            Price = 10;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            var effect = new NicotineHigh();

            _effectRepository.AddEffect(user, EffectFactory.NicotineHigh, DurationInMinutes);

            return string.Format(DougMessages.AddedEffect, effect.Name, DurationInMinutes);
        }
    }
}
