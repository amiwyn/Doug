using Doug.Items.Equipment;
using Doug.Models;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Items
{
    [TestClass]
    public class AwakeningOrbTest
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

        private AwakeningOrb _awakeningOrb;

        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();

        [TestMethod]
        public void WhenGettingFlamed_TargetIsNotifiedWithTheCallersName()
        {
            _userService.Setup(service => service.Mention(It.IsAny<User>())).Returns("<@testuser>");
            _awakeningOrb = new AwakeningOrb(_slack.Object, _userService.Object);

            _awakeningOrb.OnGettingFlamed(_command, "hehehee");

            _slack.Verify(slack => slack.SendEphemeralMessage(It.IsRegex("testuser"), "otherUserid", Channel));
        }

        [TestMethod]
        public void WhenGettingFlamed_SlurDoesNotChange()
        {
            _awakeningOrb = new AwakeningOrb(_slack.Object, _userService.Object);

            var result = _awakeningOrb.OnGettingFlamed(_command, "hehehee");

            Assert.AreEqual("hehehee", result);
        }
    }
}
