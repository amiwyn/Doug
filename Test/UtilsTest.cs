using Doug;
using Doug.Repositories;
using Doug.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

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

    }
}
