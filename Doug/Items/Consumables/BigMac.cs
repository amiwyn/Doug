using Doug.Effects.Buffs;
using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class BigMac : ConsumableItem
    {
        private readonly IEffectRepository _effectRepository;
        private const string Effect = "troll_blessing";
        private const int DurationInMinutes = 5;

        public BigMac(IInventoryRepository inventoryRepository, IEffectRepository effectRepository) : base(inventoryRepository)
        {
            _effectRepository = effectRepository;
            Id = ItemFactory.BigMac;
            Name = "Big Mac";
            Description = "On va tu au Mc Donald? Gives *Troll's Blessing* for 5 minutes";
            Rarity = Rarity.Uncommon;
            Icon = ":hamburger:";
            Price = 100;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            var effect = new TrollBlessing(null);

            _effectRepository.AddEffect(user, Effect, DurationInMinutes);

            return string.Format(DougMessages.AddedEffect, effect.Name, DurationInMinutes);
        }
    }
}
