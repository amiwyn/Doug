using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

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
        public async Task GivenUserIsAdmin_WhenValidatingUser_GetInformationFromRepository()
        {
            _slack.Setup(slack => slack.GetUserInfo(User)).Returns(Task.FromResult(new UserInfo { IsAdmin = true }));

            await _adminValidator.IsUserSlackAdmin(User);

            _slack.Verify(slack => slack.GetUserInfo(User));
        }

        [TestMethod]
        public async Task GivenUserIsNotAdmin_WhenValidatingUser_ThrowException()
        {
            _slack.Setup(repo => repo.GetUserInfo(User)).Returns(Task.FromResult(new UserInfo { IsAdmin = false }));

            //await Assert.ThrowsExceptionAsync<UserNotAdminException>(async () => await _adminValidator.IsUserSlackAdmin(User));
        }

    }
}
