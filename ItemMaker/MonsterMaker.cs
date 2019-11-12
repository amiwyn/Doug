using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Doug.Models.Combat;

namespace ItemMaker
{
    class MonsterMaker
    {
        private const string ItemTemplate = "using Doug.Monsters.Droptables;\r\n" +
                                            "\r\n" +
                                            "namespace Doug.Monsters\r\n" +
                                            "{{\r\n" +
                                            "    public class {0} : Monster\r\n" +
                                            "    {{\r\n" +
                                            "        public const string MonsterId = \"{1}\";\r\n" +
                                            "\r\n" +
                                            "        public {0}()\r\n" +
                                            "        {{\r\n" +
                                            "            Id = MonsterId;\r\n" +
                                            "            Name = \"{2}\";\r\n" +
                                            "            Description = \"{3}\";\r\n" +
                                            "            Image = \"{4}\";\r\n" +
                                            "            Level = {5};\r\n" +
                                            "            ExperienceValue = {6};\r\n" +
                                            "            MaxHealth = Health = {7};\r\n" +
                                            "            MinAttack = {8};\r\n" +
                                            "            MaxAttack = {9};\r\n" +
                                            "            Hitrate = {10};\r\n" +
                                            "            Dodge = {11};\r\n" +
                                            "            Defense = {12};\r\n" +
                                            "            Resistance = {13};\r\n" +
                                            "            AttackCooldown = {14};\r\n" +
                                            "            DamageType = {15};\r\n" +
                                            "            DropTable = StRochTable.Drops;\r\n" +
                                            "        }}\r\n" +
                                            "    }}\r\n" +
                                            "}}\r\n";

        public void GenerateClasses()
        {
            var lines = File.ReadLines("monsters.csv");
            var classes = lines.Select(CreateItemClass).ToList();
            classes.ForEach(file => File.WriteAllText("monsters/" + GetFilenameFromClass(file), file));
        }

        static string CreateItemClass(string line)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            var values = line.Split(",");
            var className = textInfo.ToTitleCase(values[0]).Replace(" ", "");
            var damageType = "Models.Combat.DamageType." + (DamageType) int.Parse(values[4]);
            return string.Format(ItemTemplate, className, values[2], values[0], values[14], values[15], values[1], values[3], values[5], values[6], values[7], values[9], values[10], values[12], values[11], values[8], damageType);
        }

        static string GetFilenameFromClass(string file)
        {
            var regex = new Regex(@"class ([A-z]*) : Monster");
            var filename = regex.Match(file).Groups[1].Value;
            return filename + ".cs";
        }
    }
}
