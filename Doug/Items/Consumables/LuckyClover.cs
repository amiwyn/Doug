using Doug.Effects.Buffs;
using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class LuckyClover : ConsumableItem
    {
        public const string ItemId = "lucky_clover";

        private readonly IEffectRepository _effectRepository;
        private const int DurationInMinutes = 5;

        public LuckyClover(IInventoryRepository inventoryRepository, IEffectRepository effectRepository) : base(inventoryRepository)
        {
            _effectRepository = effectRepository;
            Id = ItemId;
            Name = "Lucky Clover";
            Description = "A rare four-leafed clover. Legends says if you eat it, you will steal its luck. For a limited time.";
            Rarity = Rarity.Rare;
            Icon = ":clover:";
            Price = 300;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            var effect = new PickleBuff();

            _effectRepository.AddEffect(user, PickleBuff.EffectId, DurationInMinutes);

            return string.Format(DougMessages.AddedEffect, effect.Name, DurationInMinutes);
        }
    }
}
