using Doug.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.UserTests
{
    [TestClass]
    public class UserTest
    {
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _user = new User {Id = "testuser", Experience = 540};
        }

        [TestMethod]
        public void WhenHealing_HealthDoNotGoOverMax()
        {
            _user.Health += 69699696;
            Assert.AreEqual(_user.TotalHealth(), _user.Health);
        }

        [TestMethod]
        public void WhenLosingHealth_HealthDoNotGoUnderZero()
        {
            _user.Health -= 69699696;
            Assert.AreEqual(0, _user.Health);
        }

        [TestMethod]
        public void WhenGainingEnergy_EnergyDoNotGoOverMax()
        {
            _user.Energy += 69699696;
            Assert.AreEqual(_user.TotalEnergy(), _user.Energy);
        }

        [TestMethod]
        public void WhenLosingEnergy_EnergyDoNotGoUnderZero()
        {
            _user.Energy -= 69699696;
            Assert.AreEqual(0, _user.Energy);
        }

        [TestMethod]
        public void WhenDying_UserLoseAllEnergy()
        {
            _user.Dies();
            Assert.AreEqual(0, _user.Energy);
        }

        [TestMethod]
        public void WhenDying_UserIsAtOneHealth()
        {
            _user.Dies();
            Assert.AreEqual(1, _user.Health);
        }

        [TestMethod]
        public void WhenDying_UserLosesTenPercentOfNextLevel()
        {
            _user.Dies();
            Assert.AreEqual(420, _user.Experience);
        }

        [TestMethod]
        public void GivenUserIsLessThan10PercentLevelProgression_WhenDying_UserDoNotDowngradeLevel()
        {
            _user.Experience = 420;
            _user.Dies();
            Assert.AreEqual(400, _user.Experience);
        }
    }
}
