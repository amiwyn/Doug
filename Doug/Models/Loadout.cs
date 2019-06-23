using System.Collections.Generic;
using System.Linq;
using Doug.Items;

namespace Doug.Models
{
    public class Loadout
    {
        public string Id { get; set; }
        public string Head { get; set; }
        public string Body { get; set; }
        public string Legs { get; set; }
        public string Boots { get; set; }
        public string Gloves { get; set; }
        public string Neck { get; set; }

        public List<EquipmentItem> Equipment { get; }

        public Loadout(string head, string body, string legs, string boots, string gloves, string neck)
        {
            Equipment = new List<EquipmentItem>();

            AddEquipment(head);
            AddEquipment(body);
            AddEquipment(legs);
            AddEquipment(boots);
            AddEquipment(gloves);
            AddEquipment(neck);
        }

        private void AddEquipment(string itemId)
        {
            if (!string.IsNullOrEmpty(itemId))
            {
                Equipment.Add((EquipmentItem)ItemFactory.CreateItem(itemId));
            }
        }

        public int Luck => Equipment.Sum(equip => equip.Luck);
        public int Agility => Equipment.Sum(equip => equip.Agility);
        public int Charisma => Equipment.Sum(equip => equip.Charisma);
        public int Constitution => Equipment.Sum(equip => equip.Constitution);
        public int Stamina => Equipment.Sum(equip => equip.Stamina);
    }
}
