using Doug.Models.User;
using Doug.Repositories;

namespace Doug.Items.ItemActions
{
    public class Reset : ItemAction
    {
        private readonly IStatsRepository _statsRepository;

        public Reset(IInventoryRepository inventoryRepository, IStatsRepository statsRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
        }

        public override string Activate(int itemPos, User user, string channel)
        {
            _statsRepository.ResetStats(user.Id);

            return DougMessages.Cleansed;
        }
    }
}
