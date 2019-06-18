using Doug.Items.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Items
{
    [TestClass]
    public class GreedyGlovesTest
    {
        private GreedyGloves _greedyGloves;

        [TestMethod]
        public void WhenStealing_IncreaseAmountStolenBy5()
        {
            _greedyGloves = new GreedyGloves();

            var result = _greedyGloves.OnStealingAmount(1);

            Assert.AreEqual(6, result);
        }
    }
}
