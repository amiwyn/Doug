using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ItemMaker
{
    class Program
    {
        private const string ItemTemplate = "namespace Doug.Items.Equipment.Sets\r\n" +
                                              "{{\r\n" +
                                              "    public class {9} : EquipmentItem\r\n" +
                                              "    {{\r\n" +
                                              "        public const string ItemId = \"{0}\";\r\n" +
                                              "\r\n" +
                                              "        public {9}()\r\n" +
                                              "        {{\r\n" +
                                              "            Id = ItemId;\r\n" +
                                              "            Name = \"{1}\";\r\n" +
                                              "            Description = \"{2}\";\r\n" +
                                              "            Rarity = Rarity.Common;\r\n" +
                                              "            Icon = \"{3}\";\r\n" +
                                              "            Slot = EquipmentSlot.{4};\r\n" +
                                              "            Price = {5};\r\n" +
                                              "            LevelRequirement = {6};\r\n{7}" +
                                              "\r\n{8}" +
                                              "        }}\r\n" +
                                              "    }}\r\n" +
                                              "}}";

        private const string IntelligenceRequirement = "            IntelligenceRequirement = {0};\r\n";
        private const string AgilityRequirement = "            AgilityRequirement = {0};\r\n";
        private const string StrengthRequirement = "            StrengthRequirement = {0};\r\n";

        private const string Health = "            Stats.Health = {0};\r\n";
        private const string Energy = "            Stats.Energy = {0};\r\n";
        private const string MaxAttack = "            Stats.MaxAttack = {0};\r\n";
        private const string MinAttack = "            Stats.MinAttack = {0};\r\n";
        private const string AttackSpeed = "            Stats.AttackSpeed = {0};\r\n";
        private const string Hitrate = "            Stats.Hitrate = {0};\r\n";
        private const string Dodge = "            Stats.Dodge = {0};\r\n";
        private const string Defense = "            Stats.Defense = {0};\r\n";
        private const string Resistance = "            Stats.Resistance = {0};\r\n";
        private const string HealthRegen = "            Stats.HealthRegen = {0};\r\n";
        private const string EnergyRegen = "            Stats.EnergyRegen = {0};\r\n";
        private const string Luck = "            Stats.Luck = {0};\r\n";
        private const string Agility = "            Stats.Agility = {0};\r\n";
        private const string Strength = "            Stats.Strength = {0};\r\n";
        private const string Constitution = "            Stats.Constitution = {0};\r\n";
        private const string Intelligence = "            Stats.Intelligence = {0};\r\n";

        // id, name, desc, icon, slot, price, level, stat req, stats, classname

        static void Main()
        {
            // for armors
            var lines = File.ReadLines("armor.csv");
            var classes = lines.Select(CreateItemClass).ToList();
            classes.ForEach(file => File.WriteAllText("classes/" + GetFilenameFromClass(file), file));
        }

        static string CreateItemClass(string line)
        {
            var values = line.Split(",");

            var statsRequirements = CreateField(AgilityRequirement, values[3]) + CreateField(IntelligenceRequirement, values[4]) + CreateField(StrengthRequirement, values[5]);
            var stats = CreateField(Health, values[7]) +
                        CreateField(Energy, values[8]) +
                        CreateField(MaxAttack, values[9]) +
                        CreateField(MinAttack, values[10]) +
                        CreateField(AttackSpeed, values[11]) +
                        CreateField(Hitrate, values[12]) +
                        CreateField(Dodge, values[13]) +
                        CreateField(Defense, values[14]) +
                        CreateField(Resistance, values[15]) +
                        CreateField(HealthRegen, values[16]) +
                        CreateField(EnergyRegen, values[17]) +
                        CreateField(Luck, values[18]) +
                        CreateField(Agility, values[19]) +
                        CreateField(Strength, values[20]) +
                        CreateField(Constitution, values[21]) +
                        CreateField(Intelligence, values[22]);
            var className = values[0].Replace(" ", "");

            return string.Format(ItemTemplate, values[26], values[0], values[24], values[25], values[2], values[6], values[1], statsRequirements, stats, className);
        }

        static string GetFilenameFromClass(string file)
        {
            var regex = new Regex(@"class ([A-z]*) : Equipment");
            var filename = regex.Match(file).Groups[1].Value;
            return filename + ".cs";
        }

        static string CreateField(string fieldString, string data)
        {
            return string.IsNullOrWhiteSpace(data) ? string.Empty : string.Format(fieldString, data);
        }
    }
}
