using Doug.Items.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Items
{
    [TestClass]
    public class BurglarBootsTest
    {
        private BurglarBoots _burglarBoots;

        [TestMethod]
        public void WhenStealing_IncreaseChancesBy20Percent()
        {
            _burglarBoots = new BurglarBoots();

            var result = _burglarBoots.OnStealingChance(0.25);

            Assert.AreEqual(0.45, result);
        }
    }
}
