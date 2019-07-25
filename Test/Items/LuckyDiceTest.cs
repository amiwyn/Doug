using Doug.Items.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Items
{
    [TestClass]
    public class LuckyDiceTest
    {
        private LuckyCoin _luckyDice;

        [TestMethod]
        public void WhenGambling_ChancesIncreasesByFivePercent()
        {
            _luckyDice = new LuckyCoin();

            var result = _luckyDice.OnGambling(0.5);

            Assert.AreEqual(0.55, result);
        }
    }
}
