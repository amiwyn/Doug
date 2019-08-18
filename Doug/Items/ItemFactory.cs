﻿using Doug.Items.Consumables;
using Doug.Items.Consumables.Resets;
using Doug.Items.Equipment;
using Doug.Items.Equipment.Necklaces;
using Doug.Items.Equipment.Sets.Tier1.Cloth;
using Doug.Items.Equipment.Sets.Tier1.Leather;
using Doug.Items.Equipment.Sets.Noob;
using Doug.Items.Equipment.Sets.Tier1.Plate;
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
                case McdoFries.ItemId: return new McdoFries(_statsRepository, _inventoryRepository, _userService);

                // Utility Consumables
                case SuicidePill.ItemId: return new SuicidePill(_inventoryRepository, _userService);
                case KickTicket.ItemId: return new KickTicket(_inventoryRepository, _slack, _userService, _eventDispatcher);
                case InviteTicket.ItemId: return new InviteTicket(_inventoryRepository, _slack);
                case StRochTicket.ItemId: return new StRochTicket(_inventoryRepository, _slack);
                case VanierTicket.ItemId: return new VanierTicket(_inventoryRepository, _slack);

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
                case IronIngot.ItemId: return new IronIngot();
                case BikerCocaine.ItemId: return new BikerCocaine();


                // Noob Set
                case FarmersArmor.ItemId: return new FarmersArmor();
                case FarmersBoots.ItemId: return new FarmersBoots();
                case FarmersGloves.ItemId: return new FarmersGloves();
                case PeasantBoots.ItemId: return new PeasantBoots();
                case PeasantShirt.ItemId: return new PeasantShirt();
                case ShortBlade.ItemId: return new ShortBlade();
                case ShortSword.ItemId: return new ShortSword();
                case WoodenShield.ItemId: return new WoodenShield();

                // Cloth Set 10-15
                case ClothArmor.ItemId: return new ClothArmor();
                case ClothBoots.ItemId: return new ClothBoots();
                case ClothGloves.ItemId: return new ClothGloves();
                case FurHat.ItemId: return new FurHat();
                case WoodenStaff.ItemId: return new WoodenStaff();

                // Leather Set 10-15
                case LeatherArmor.ItemId: return new LeatherArmor();
                case LeatherBoots.ItemId: return new LeatherBoots();
                case LeatherGloves.ItemId: return new LeatherGloves();
                case ShortBow.ItemId: return new ShortBow();
                case SmallClaw.ItemId: return new SmallClaw();

                // Plate Set 10-15
                case PlateArmor.ItemId: return new PlateArmor();
                case PlateBoots.ItemId: return new PlateBoots();
                case PlateGloves.ItemId: return new PlateGloves();
                case LightSword.ItemId: return new LightSword();
                case SmallAxe.ItemId: return new SmallAxe();
                case LargeSword.ItemId: return new LargeSword();



                default: return new Default();
            }
        }
    }
}
