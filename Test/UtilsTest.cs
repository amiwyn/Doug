using Doug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test
{
    [TestClass]
    public class UtilsTest
    {
        private const string User = "robert";

        [TestMethod]
        public void WhenMakingUserMention_ThenSlackDecorationIsAdded()
        {
            var mention = Utils.UserMention(User);

            Assert.AreEqual("<@robert>", mention);
        }

        [TestMethod]
        public void GivenTimeInTimespan_WhenCheckingTimespan_ThenResultIsPositive()
        {
            var currentTime = new DateTime(1, 1, 1, 12, 29, 0, DateTimeKind.Utc);
            var targetTime = new TimeSpan(12, 0, 0);

            var result = Utils.IsInTimespan(currentTime, targetTime, 30);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void GivenTimeNotInTimespan_WhenCheckingTimespan_ThenResultIsNegative()
        {
            var currentTime = new DateTime(1, 1, 1, 13, 0, 0, DateTimeKind.Utc);
            var targetTime = new TimeSpan(12, 0, 0);

            var result = Utils.IsInTimespan(currentTime, targetTime, 30);

            Assert.AreEqual(false, result);
        }
    }
}
