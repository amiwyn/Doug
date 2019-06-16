using Doug.Commands;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using Hangfire;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Test
{
    [TestClass]
    public class AwakeningOrbTest
    {
        private const string CommandText = "<@otherUserid|username>";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private AwakeningOrb _awakeningOrb;

        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();

        [TestMethod]
        public void WhenGettingFlamed_TargetIsNotifiedWithTheCallersName()
        {
            _awakeningOrb = new AwakeningOrb();

            _awakeningOrb.OnGettingFlamed(command, "hehehee", _slack.Object);

            _slack.Verify(slack => slack.SendEphemeralMessage(It.IsRegex("testuser"), "otherUserid", Channel));
        }

        [TestMethod]
        public void WhenGettingFlamed_SlurDoesNotChange()
        {
            _awakeningOrb = new AwakeningOrb();

            var result = _awakeningOrb.OnGettingFlamed(command, "hehehee", _slack.Object);

            Assert.AreEqual("hehehee", result);
        }
    }
}
