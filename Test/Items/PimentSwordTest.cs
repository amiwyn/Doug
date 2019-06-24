using Doug.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Doug.Items.Equipment;
using Doug.Slack;

namespace Test.Items
{
    [TestClass]
    public class PimentSwordTest
    {
        private const string CommandText = "<@otherUserid|username>";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command _command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private PimentSword _pimentSword;

        [TestInitialize]
        public void Setup()
        {
            _pimentSword = new PimentSword();
        }

        [TestMethod]
        public void WhenFlaming_ReflectSlursBack()
        {
            var result = _pimentSword.OnFlaming(_command, "<@otherUserid> is a bitch", _slack.Object);

            Assert.AreEqual("<@testuser> is a bitch", result);
        }
    }
}
