using Doug.Items.Consumables;
using Doug.Items.Consumables.Resets;
using Doug.Items.Equipment;
using Doug.Items.Equipment.Sets.Noob;
using Doug.Items.Equipment.Sets.StartingWeapons;
using Doug.Items.Lootboxes;
using Doug.Items.Misc;
using Doug.Items.Misc.Drops;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Items
{
    public interface IItemFactory
    {
        Item CreateItem(string itemId);
    }
    public class ItemFactory : IItemFactory
    {
        private readonly ISlackWebApi _slack;
        private readonly IStatsRepository _statsRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUserService _userService;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IEffectRepository _effectRepository;
        private readonly IRandomService _randomService;

        public ItemFactory(ISlackWebApi slack, IStatsRepository statsRepository, IInventoryRepository inventoryRepository, IUserService userService, IEventDispatcher eventDispatcher, IEffectRepository effectRepository, IRandomService randomService)
        {
            _slack = slack;
            _statsRepository = statsRepository;
            _inventoryRepository = inventoryRepository;
            _userService = userService;
            _eventDispatcher = eventDispatcher;
            _effectRepository = effectRepository;
            _randomService = randomService;
        }

        public Item CreateItem(string itemId)
        {
            switch (itemId)
            {
                // Consumables
                case CoffeeCup.ItemId: return new CoffeeCup(_statsRepository, _inventoryRepository);
                case Apple.ItemId: return new Apple(_statsRepository, _inventoryRepository);
                case Bread.ItemId: return new Bread(_statsRepository, _inventoryRepository);
                case McdoFries.ItemId: return new McdoFries(_statsRepository, _inventoryRepository, _userService);

                // Utility Consumables
                case SuicidePill.ItemId: return new SuicidePill(_inventoryRepository, _userService);
                case KickTicket.ItemId: return new KickTicket(_inventoryRepository, _slack, _userService, _eventDispatcher);
                case InviteTicket.ItemId: return new InviteTicket(_inventoryRepository, _slack);

                // Effect Consumables
                case BigMac.ItemId: return new BigMac(_inventoryRepository, _effectRepository);
                case Cigarette.ItemId: return new Cigarette(_inventoryRepository, _effectRepository);
                case HolyWater.ItemId: return new HolyWater(_inventoryRepository, _effectRepository);
                case PicklePickle.ItemId: return new PicklePickle(_inventoryRepository, _effectRepository, _statsRepository);
                case LuckyClover.ItemId: return new LuckyClover(_inventoryRepository, _effectRepository);

                // Resets
                case AgilityReset.ItemId: return new AgilityReset(_statsRepository, _inventoryRepository);
                case StrengthReset.ItemId: return new StrengthReset(_statsRepository, _inventoryRepository);
                case ConstitutionReset.ItemId: return new ConstitutionReset(_statsRepository, _inventoryRepository);
                case LuckReset.ItemId: return new LuckReset(_statsRepository, _inventoryRepository);
                case IntelligenceReset.ItemId: return new IntelligenceReset(_statsRepository, _inventoryRepository);

                // Special Equipment
                case AwakeningOrb.ItemId: return new AwakeningOrb(_slack, _userService);
                case LuckyCoin.ItemId: return new LuckyCoin();
                case BurglarBoots.ItemId: return new BurglarBoots();
                case GreedyGloves.ItemId: return new GreedyGloves();
                case DevilsContract.ItemId: return new DevilsContract();
                case PimentSword.ItemId: return new PimentSword();
                case CloakOfSpikes.ItemId: return new CloakOfSpikes();
                case StraightEdge.ItemId: return new StraightEdge();
                case Crown.ItemId: return new Crown();

                // Noob Set
                case FarmersArmor.ItemId: return new FarmersArmor();
                case FarmersBoots.ItemId: return new FarmersBoots();
                case FarmersGloves.ItemId: return new FarmersGloves();
                case PeasantBoots.ItemId: return new PeasantBoots();
                case PeasantShirt.ItemId: return new PeasantShirt();
                case ShortBlade.ItemId: return new ShortBlade();
                case ShortSword.ItemId: return new ShortSword();
                case WoodenShield.ItemId: return new WoodenShield();

                // Starting Weapons
                case LightSword.ItemId: return new LightSword();
                case SmallAxe.ItemId: return new SmallAxe();
                case LargeSword.ItemId: return new LargeSword();
                case ShortBow.ItemId: return new ShortBow();
                case SmallClaw.ItemId: return new SmallClaw();
                case WoodenStaff.ItemId: return new WoodenStaff();

                // Misc
                case BachelorsDegree.ItemId: return new BachelorsDegree();
                case MysteryBox.ItemId: return new MysteryBox(_inventoryRepository, _randomService, _slack, _userService, this);

                // Monster Drops
                case GullFeather.ItemId: return new GullFeather();
                case SharpBeak.ItemId: return new SharpBeak();
                case IronIngot.ItemId: return new IronIngot();
                case BikerCocaine.ItemId: return new BikerCocaine();
                default: return new Default();
            }
        }
    }
}
