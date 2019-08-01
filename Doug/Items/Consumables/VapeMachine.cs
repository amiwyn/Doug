using Doug.Effects.Buffs;
using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class VapeMachine : ConsumableItem
    {
        public const string ItemId = "vape_machine";

        private readonly IEffectRepository _effectRepository;
        private const int DurationInMinutes = 5;

        public VapeMachine (IInventoryRepository inventoryRepository, IEffectRepository effectRepository) : base(inventoryRepository)
        {
            _effectRepository = effectRepository;
            Id = ItemId;
            Name = "Vape Machine";
            Description = "Some kind of hobo breathing mechanism";
            Rarity = Rarity.Uncommon;
            Icon = ":smoking:";
            Price = 15;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            var effect = new SmellSweet();

            _effectRepository.AddEffect(user, SmellSweet.EffectId, DurationInMinutes);

            return string.Format(DougMessages.AddedEffect, effect.Name, DurationInMinutes);
        }
    }
}
