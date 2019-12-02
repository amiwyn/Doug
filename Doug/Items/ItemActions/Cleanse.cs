using Doug.Models.User;
using Doug.Repositories;

namespace Doug.Items.ItemActions
{
    public class Cleanse : ItemAction
    {
        private readonly IEffectRepository _effectRepository;

        public Cleanse(IEffectRepository effectRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _effectRepository = effectRepository;
        }

        public override string Activate(int itemPos, User user, string channel)
        {
            base.Activate(itemPos, user, channel);
            _effectRepository.RemoveAllEffects(user);

            return DougMessages.Cleansed;
        }
    }
}
