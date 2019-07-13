using Doug.Items.Consumables;
using Doug.Items.Consumables.Resets;
using Doug.Items.Equipment;
using Doug.Items.Misc;
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
        public const string AwakeningOrb = "awakening_orb";
        public const string LuckyDice = "lucky_dice";
        public const string BurglarBoots = "burglar_boots";
        public const string GreedyGloves = "greedy_gloves";
        public const string CoffeeCup = "coffee_cup";
        public const string PimentSword = "piment_sword";
        public const string Apple = "apple";
        public const string DevilsContract = "devil_contract";
        public const string CloakOfSpikes = "cloak_spikes";
        public const string Bread = "bread";
        public const string McdoFries = "mcdo_fries";
        public const string SteelSword = "steel_sword";
        public const string ClothArmor = "cloth_armor";
        public const string KickTicket = "kick_ticket";
        public const string InviteTicket = "invite_ticket";
        public const string BigMac = "big_mac";
        public const string AgilityReset = "agi_reset";
        public const string LuckReset = "luck_reset";
        public const string StrengthReset = "str_reset";
        public const string ConstitutionReset = "con_reset";
        public const string StaminaReset = "stam_reset";
        public const string BachelorsDegree = "bachelor_degree";
        public const string Cigarette = "cigarette";
        public const string SuicidePill = "suicide_pill";
        public const string Crown = "crown";
        public const string HolyWater = "holy_water";
        public const string StraightEdge = "straight_edge";

        private readonly ISlackWebApi _slack;
        private readonly IStatsRepository _statsRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUserService _userService;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IEffectRepository _effectRepository;

        public ItemFactory(ISlackWebApi slack, IStatsRepository statsRepository, IInventoryRepository inventoryRepository, IUserService userService, IEventDispatcher eventDispatcher, IEffectRepository effectRepository)
        {
            _slack = slack;
            _statsRepository = statsRepository;
            _inventoryRepository = inventoryRepository;
            _userService = userService;
            _eventDispatcher = eventDispatcher;
            _effectRepository = effectRepository;
        }

        public Item CreateItem(string itemId)
        {
            switch (itemId)
            {
                case AwakeningOrb:
                    return new AwakeningOrb(_slack, _userService);
                case LuckyDice:
                    return new LuckyDice();
                case BurglarBoots:
                    return new BurglarBoots();
                case GreedyGloves:
                    return new GreedyGloves();
                case CoffeeCup:
                    return new CoffeeCup(_statsRepository, _inventoryRepository);
                case PimentSword:
                    return new PimentSword();
                case SteelSword:
                    return new SteelSword();
                case ClothArmor:
                    return new ClothArmor();
                case Apple:
                    return new Apple(_statsRepository, _inventoryRepository);
                case DevilsContract:
                    return new DevilContract();
                case CloakOfSpikes:
                    return new CloakOfSpikes();
                case KickTicket:
                    return new KickTicket(_inventoryRepository, _slack, _userService, _eventDispatcher);
                case InviteTicket:
                    return new InviteTicket(_inventoryRepository, _slack); 
                case Bread:
                    return new Bread(_statsRepository, _inventoryRepository);
                case McdoFries:
                    return new McdoFries(_statsRepository, _inventoryRepository, _userService);
                case BigMac:
                    return new BigMac(_inventoryRepository, _effectRepository);
                case AgilityReset:
                    return new AgilityReset(_statsRepository, _inventoryRepository);
                case StrengthReset:
                    return new StrengthReset(_statsRepository, _inventoryRepository);
                case ConstitutionReset:
                    return new ConstitutionReset(_statsRepository, _inventoryRepository);
                case LuckReset:
                    return new LuckReset(_statsRepository, _inventoryRepository);
                case StaminaReset:
                    return new StaminaReset(_statsRepository, _inventoryRepository);
                case Cigarette:
                    return new Cigarette(_inventoryRepository, _effectRepository);
                case BachelorsDegree:
                    return new BachelorsDegree();
                case SuicidePill:
                    return new SuicidePill(_inventoryRepository, _userService);
                case Crown:
                    return new Crown();
                case HolyWater:
                    return new HolyWater(_inventoryRepository, _effectRepository);
                case StraightEdge:
                    return new StraightEdge();
                default:
                    return new Default();
            }
        }
    }
}
