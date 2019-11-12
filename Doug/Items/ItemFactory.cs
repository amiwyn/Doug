using Doug.Items.Consumables;
using Doug.Items.Consumables.Resets;
using Doug.Items.Equipment;
using Doug.Items.Equipment.Necklaces;
using Doug.Items.Equipment.Sets.Tier1.Cloth;
using Doug.Items.Equipment.Sets.Tier1.Leather;
using Doug.Items.Equipment.Sets.Noob;
using Doug.Items.Equipment.Sets.Tier1.Plate;
using Doug.Items.Equipment.Sets.Tier2.Fighter;
using Doug.Items.Equipment.Sets.Tier2.Hunter;
using Doug.Items.Equipment.Sets.Tier2.Mage;
using Doug.Items.Lootboxes;
using Doug.Items.Misc;
using Doug.Items.Misc.Drops;
using Doug.Items.SkillBooks;
using Doug.Items.Tickets;
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
        private readonly IChannelRepository _channelRepository;
        private readonly ICreditsRepository _creditsRepository;
        private readonly ICombatService _combatService;

        public ItemFactory(ISlackWebApi slack, IStatsRepository statsRepository, IInventoryRepository inventoryRepository, IUserService userService, IEventDispatcher eventDispatcher, IEffectRepository effectRepository, IRandomService randomService, IChannelRepository channelRepository, ICreditsRepository creditsRepository, ICombatService combatService)
        {
            _slack = slack;
            _statsRepository = statsRepository;
            _inventoryRepository = inventoryRepository;
            _userService = userService;
            _eventDispatcher = eventDispatcher;
            _effectRepository = effectRepository;
            _randomService = randomService;
            _channelRepository = channelRepository;
            _creditsRepository = creditsRepository;
            _combatService = combatService;
        }

        public Item CreateItem(string itemId)
        {
            switch (itemId)
            {
                // Consumables
                case CoffeeCup.ItemId: return new CoffeeCup(_statsRepository, _inventoryRepository);
                case Apple.ItemId: return new Apple(_statsRepository, _inventoryRepository);
                case Bread.ItemId: return new Bread(_statsRepository, _inventoryRepository);
                case AppleSandwich.ItemId: return new AppleSandwich(_statsRepository, _inventoryRepository);
                case McdoFries.ItemId: return new McdoFries(_statsRepository, _inventoryRepository, _userService);

                // Utility Consumables
                case SuicidePill.ItemId: return new SuicidePill(_inventoryRepository, _userService);
                case KickTicket.ItemId: return new KickTicket(_inventoryRepository, _slack, _userService, _eventDispatcher);
                case InviteTicket.ItemId: return new InviteTicket(_inventoryRepository, _slack);

                case StRochTicket.ItemId: return new StRochTicket(_inventoryRepository, _slack);
                case VanierTicket.ItemId: return new VanierTicket(_inventoryRepository, _slack);
                case BeauceTicket.ItemId: return new BeauceTicket(_inventoryRepository, _slack);
                case ChibougamauTicket.ItemId: return new ChibougamauTicket(_inventoryRepository, _slack);
                case JapanTicket.ItemId: return new JapanTicket(_inventoryRepository, _slack);
                case ParliamentTicket.ItemId: return new ParliamentTicket(_inventoryRepository, _slack);
                case UniversityTicket.ItemId: return new UniversityTicket(_inventoryRepository, _slack);

                // Combat Consumables
                case AntiRogueSpray.ItemId: return new AntiRogueSpray(_inventoryRepository, _eventDispatcher, _slack, _userService, _combatService);

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
                case IncognitoShades.ItemId: return new IncognitoShades();

                // Necklaces
                case EmeraldAmulet.ItemId: return new EmeraldAmulet();

                // Skill books
                case HealBook.ItemId: return new HealBook(_statsRepository, _slack, _userService);
                case StealBook.ItemId: return new StealBook(_statsRepository, _slack, _userService, _channelRepository, _eventDispatcher, _randomService, _creditsRepository);
                case FireballBook.ItemId: return new FireballBook(_statsRepository, _slack, _channelRepository, _userService, _combatService, _eventDispatcher);
                case MightyStrikeBook.ItemId: return new MightyStrikeBook(_statsRepository, _slack, _channelRepository, _userService, _combatService, _eventDispatcher);
                case LacerateBook.ItemId: return new LacerateBook(_statsRepository, _slack, _channelRepository, _userService, _combatService, _eventDispatcher);

                // Misc
                case BachelorsDegree.ItemId: return new BachelorsDegree();
                case MysteryBox.ItemId: return new MysteryBox(_inventoryRepository, _randomService, _slack, _userService, this);
                case PeasantBox.ItemId: return new PeasantBox(_inventoryRepository, _slack, this);

                // Monster Drops
                case GullFeather.ItemId: return new GullFeather();
                case SharpBeak.ItemId: return new SharpBeak();
                case BikerCocaine.ItemId: return new BikerCocaine();

                case IronIngot.ItemId: return new IronIngot();
                case SilverIngot.ItemId: return new SilverIngot();
                case GoldIngot.ItemId: return new GoldIngot();
                case MithrilIngot.ItemId: return new MithrilIngot();

                // Noob Set
                case FarmersArmor.ItemId: return new FarmersArmor();
                case FarmersBoots.ItemId: return new FarmersBoots();
                case FarmersGloves.ItemId: return new FarmersGloves();
                case PeasantBoots.ItemId: return new PeasantBoots();
                case PeasantShirt.ItemId: return new PeasantShirt();
                case ShortBlade.ItemId: return new ShortBlade();
                case ShortSword.ItemId: return new ShortSword();
                case WoodenShield.ItemId: return new WoodenShield();

                // Cloth Set 10
                case ClothArmor.ItemId: return new ClothArmor();
                case ClothBoots.ItemId: return new ClothBoots();
                case ClothGloves.ItemId: return new ClothGloves();
                case FurHat.ItemId: return new FurHat();
                case WoodenStaff.ItemId: return new WoodenStaff();

                // Leather Set 10
                case LeatherArmor.ItemId: return new LeatherArmor();
                case LeatherBoots.ItemId: return new LeatherBoots();
                case LeatherGloves.ItemId: return new LeatherGloves();
                case ShortBow.ItemId: return new ShortBow();
                case SmallClaw.ItemId: return new SmallClaw();

                // Plate Set 10
                case PlateArmor.ItemId: return new PlateArmor();
                case PlateBoots.ItemId: return new PlateBoots();
                case PlateGloves.ItemId: return new PlateGloves();
                case LightSword.ItemId: return new LightSword();
                case SmallAxe.ItemId: return new SmallAxe();
                case LargeSword.ItemId: return new LargeSword();

                // Cloth Set 15
                case ApprenticeArmor.ItemId: return new ApprenticeArmor();
                case ApprenticeBoots.ItemId: return new ApprenticeBoots();
                case ApprenticeGloves.ItemId: return new ApprenticeGloves();

                // Leather Set 15
                case BanditArmor.ItemId: return new BanditArmor();
                case BanditBoots.ItemId: return new BanditBoots();
                case BanditGloves.ItemId: return new BanditGloves();

                // Plate Set 15
                case ToughPlateArmor.ItemId: return new ToughPlateArmor();
                case ToughPlateBoots.ItemId: return new ToughPlateBoots();
                case ToughPlateGloves.ItemId: return new ToughPlateGloves();

                // Cloth Set 20
                case MageArmor.ItemId: return new MageArmor();
                case MageBoots.ItemId: return new MageBoots();
                case MageGloves.ItemId: return new MageGloves();
                case MagePlume.ItemId: return new MagePlume();

                // Leather Set 20
                case HunterArmor.ItemId: return new HunterArmor();
                case HunterBoots.ItemId: return new HunterBoots();
                case HunterGloves.ItemId: return new HunterGloves();
                case HunterBeret.ItemId: return new HunterBeret();

                // Plate Set 20
                case FighterArmor.ItemId: return new FighterArmor();
                case FighterBoots.ItemId: return new FighterBoots();
                case FighterGloves.ItemId: return new FighterGloves();
                case FighterHelm.ItemId: return new FighterHelm();

                // Cloth Set 25
                case BrightMageArmor.ItemId: return new BrightMageArmor();
                case BrightMageBoots.ItemId: return new BrightMageBoots();
                case BrightMageGloves.ItemId: return new BrightMageGloves();

                // Leather Set 25
                case SwiftHunterArmor.ItemId: return new SwiftHunterArmor();
                case SwiftHunterBoots.ItemId: return new SwiftHunterBoots();
                case SwiftHunterGloves.ItemId: return new SwiftHunterGloves();

                // Plate Set 25
                case StrongFighterArmor.ItemId: return new StrongFighterArmor();
                case StrongFighterBoots.ItemId: return new StrongFighterBoots();
                case StrongFighterGloves.ItemId: return new StrongFighterGloves();

                default: return new Default();
            }
        }
    }
}
