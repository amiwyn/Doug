using Doug.Repositories;
using Doug.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class AdminValidatorTest
    {
        private AdminValidator _adminValidator;

        private const string User = "robert";

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();

        [TestInitialize]
        public void Setup()
        {
            _adminValidator = new AdminValidator(_userRepository.Object);
        }

        [TestMethod]
        public async Task GivenUserIsAdmin_WhenValidatingUser_GetInformationFromRepository()
        {
            _userRepository.Setup(repo => repo.IsAdmin(User)).Returns(Task.FromResult(true));

            await _adminValidator.ValidateUserIsAdmin(User);

            _userRepository.Verify(userRepo => userRepo.IsAdmin(User));
        }

        [TestMethod]
        public async Task GivenUserIsNotAdmin_WhenValidatingUser_ThrowException()
        {
            _userRepository.Setup(repo => repo.IsAdmin(User)).Returns(Task.FromResult(false));

            await Assert.ThrowsExceptionAsync<UserNotAdminException>(async () => await _adminValidator.ValidateUserIsAdmin(User));
        }

    }
}
