using Doug.Items.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Items
{
    [TestClass]
    public class DevilContractTest
    {
        private DevilsContract _devilsContract;

        [TestMethod]
        public void WhenStealing_DecreaseChancesEnormously()
        {
            _devilsContract = new DevilsContract();

            var result = _devilsContract.OnStealingChance(0.25);

            Assert.AreEqual(-69, result);
        }

        [TestMethod]
        public void WhenGettingStolen_DecreaseChancesEnormously()
        {
            _devilsContract = new DevilsContract();

            var result = _devilsContract.OnGettingStolenChance(0.75);

            Assert.AreEqual(-69, result);
        }
    }
}
