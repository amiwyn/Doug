using Doug.Effects;
using Doug.Models.Combat;
using Doug.Models.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.UserTests
{
    [TestClass]
    public class UserCombatTest
    {
        private User _user;
        private readonly Mock<ICombatable> _target = new Mock<ICombatable>();
        private readonly Mock<IEventDispatcher> _eventDispatcher = new Mock<IEventDispatcher>();

        [TestInitialize]
        public void Setup()
        {
            _user = new User {Id = "robert" };
        }

        [TestMethod]
        public void WhenAttacking_DamageIsAppliedToTarget()
        {
            _user.AttackTarget(_target.Object, _eventDispatcher.Object);
            _target.Verify(target => target.ReceiveAttack(It.IsAny<Attack>(), It.IsAny<IEventDispatcher>()));
        }
    }
}
