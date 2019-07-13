using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class HolyWater : ConsumableItem
    {
        private readonly IEffectRepository _effectRepository;

        public HolyWater(IInventoryRepository inventoryRepository, IEffectRepository effectRepository) : base(inventoryRepository)
        {
            _effectRepository = effectRepository;
            Id = ItemFactory.HolyWater;
            Name = "Holy Water";
            Description = "Water used to purify your sins. Removes all active effects on you.";
            Rarity = Rarity.Unique;
            Icon = ":holy_water:";
            Price = 100;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            _effectRepository.RemoveAllEffects(user);

            return DougMessages.Cleansed;
        }
    }
}
