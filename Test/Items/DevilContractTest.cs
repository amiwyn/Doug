using Doug.Items.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Items
{
    [TestClass]
    public class DevilContractTest
    {
        private DevilContract _devilContract;

        [TestMethod]
        public void WhenStealing_DecreaseChancesEnormously()
        {
            _devilContract = new DevilContract();

            var result = _devilContract.OnStealingChance(0.25);

            Assert.AreEqual(-69, result);
        }

        [TestMethod]
        public void WhenGettingStolen_DecreaseChancesEnormously()
        {
            _devilContract = new DevilContract();

            var result = _devilContract.OnGettingStolenChance(0.75);

            Assert.AreEqual(-69, result);
        }
    }
}
