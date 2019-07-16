using Doug.Effects;
using Doug.Effects.Buffs;
using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class BigMac : ConsumableItem
    {
        public const string ItemId = "big_mac";

        private readonly IEffectRepository _effectRepository;
        private const int DurationInMinutes = 5;

        public BigMac(IInventoryRepository inventoryRepository, IEffectRepository effectRepository) : base(inventoryRepository)
        {
            _effectRepository = effectRepository;
            Id = ItemId;
            Name = "Big Mac";
            Description = "On va tu au Mc Donald? Gives *Troll's Blessing* for 5 minutes";
            Rarity = Rarity.Uncommon;
            Icon = ":bigmac:";
            Price = 100;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            var effect = new TrollBlessing(null);

            _effectRepository.AddEffect(user, TrollBlessing.EffectId, DurationInMinutes);

            return string.Format(DougMessages.AddedEffect, effect.Name, DurationInMinutes);
        }
    }
}
