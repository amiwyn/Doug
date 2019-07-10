using Doug.Models;
using Doug.Services;
using Doug.Slack;
using Doug.Repositories;

namespace Doug.Items.Equipment
{
    public class MainGauche : EquipmentItem
    {
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;
        private IInventoryRepository _inventoryRepository;
        private IEquipmentRepository _equipmentRepository;

        public MainGauche(ISlackWebApi slack, IUserService userService, IInventoryRepository inventoryRepository, IEquipmentRepository equipmentRepository)
        {
            _slack = slack;
            _userService = userService;
            _inventoryRepository = inventoryRepository;
            _equipmentRepository = equipmentRepository;

            Id = ItemFactory.MainGauche;
            Name = "Main Gauche";
            Description = "A dented, dulled brass shank. You won't be caught while /stealing, but any player that kills you steals this item.";
            Rarity = Rarity.Unique;
            Icon = ":dagger_knife:";
            Slot = EquipmentSlot.LeftHand;
            Price = 1000;

            Attack = 9;
            Agility = 2;
        }

        public override string OnStealingFailed(string response, string targetUserMention)
        {
            return "Someone failed to rob " + targetUserMention + ", but they were too sneaky to get caught...";
        }

        public override void OnDeathByUser(User killer)
        {
            _inventoryRepository.AddItem(killer, ItemFactory.MainGauche);
        }

        public override bool OnDeath(User user)
        {
            _equipmentRepository.DeleteEquippedItem(user, EquipmentSlot.LeftHand);
            return true;
        }
    }
}
