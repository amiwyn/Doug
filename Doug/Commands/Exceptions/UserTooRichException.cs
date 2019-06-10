using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Commands
{
    public class UserTooRichException : ApplicationException
    {
        public UserTooRichException() : base(DougMessages.YouAreTooRich)
        {
        }
    }
}
