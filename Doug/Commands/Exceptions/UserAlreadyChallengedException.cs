using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Commands
{
    public class UserAlreadyChallengedException : ApplicationException
    {
        public UserAlreadyChallengedException() : base(DougMessages.AlreadyChallenged)
        {
        }
    }
}
