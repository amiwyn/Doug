using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Doug.Slack.Dto;

namespace Test
{
    [TestClass]
    public class AdminValidatorTest
    {
        private AuthorizationService _adminValidator;

        private const string User = "robert";

        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();

        [TestInitialize]
        public void Setup()
        {
            _adminValidator = new AuthorizationService(_slack.Object);
        }

        [TestMethod]
        public async Task GivenUserIsNotAdmin_WhenValidatingUser_ReturnTrue()
        {
            _slack.Setup(repo => repo.GetUserInfo(User)).Returns(Task.FromResult(new UserInfo { IsAdmin = true }));

            var result = await _adminValidator.IsUserSlackAdmin(User);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task GivenUserIsNotAdmin_WhenValidatingUser_ReturnFalse()
        {
            _slack.Setup(repo => repo.GetUserInfo(User)).Returns(Task.FromResult(new UserInfo { IsAdmin = false }));

            var result = await _adminValidator.IsUserSlackAdmin(User);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task GivenUserIsAdmin_WhenValidatingUser_GetInformationFromRepository()
        {
            _slack.Setup(slack => slack.GetUserInfo(User)).Returns(Task.FromResult(new UserInfo { IsAdmin = true }));

            await _adminValidator.IsUserSlackAdmin(User);

            _slack.Verify(slack => slack.GetUserInfo(User));
        }
    }
}
