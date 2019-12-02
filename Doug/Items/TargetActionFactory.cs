using Doug.Effects;
using Doug.Items.TargetActions;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Items
{
    public interface ITargetActionFactory
    {
        TargetAction CreateTargetAction(string targetActionId);
    }

    public class TargetActionFactory : ITargetActionFactory
    {
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ISlackWebApi _slack;
        private readonly ICombatService _combatService;
        private readonly IInventoryRepository _inventoryRepository;

        public TargetActionFactory(IEventDispatcher eventDispatcher, ISlackWebApi slack, ICombatService combatService, IInventoryRepository inventoryRepository)
        {
            _eventDispatcher = eventDispatcher;
            _slack = slack;
            _combatService = combatService;
            _inventoryRepository = inventoryRepository;
        }

        public TargetAction CreateTargetAction(string targetActionId)
        {
            switch (targetActionId)
            {
                case "kick": return new Kick(_eventDispatcher, _slack, _inventoryRepository).Activate;
                case "antirogue": return new AntiRogue(_eventDispatcher, _combatService, _inventoryRepository).Activate;
                default: return (_, __, ___, ____) => DougMessages.ItemCantBeUsed;
            }
        }
    }
}
