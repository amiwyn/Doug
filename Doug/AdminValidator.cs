using Doug.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug
{
    public interface IAdminValidator
    {
        Task ValidateUserIsAdmin(string userId);
    }

    public class AdminValidator : IAdminValidator
    {
        private IUserRepository _userRepository;

        public async Task ValidateUserIsAdmin(string userId)
        {
            bool isAdmin = await _userRepository.IsAdmin(userId);

            if (!isAdmin)
            {
                throw new Exception(DougMessages.NotAnAdmin);
            }
        }
    }
}
