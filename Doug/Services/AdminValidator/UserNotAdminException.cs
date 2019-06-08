using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Services
{
    public class UserNotAdminException : Exception
    {
        public UserNotAdminException() : base(DougMessages.NotAnAdmin)
        {
        }
    }
}
