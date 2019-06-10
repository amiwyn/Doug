using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Commands
{
    public class SlursAreCleanException : ApplicationException
    {
        public SlursAreCleanException() : base(DougMessages.SlursAreClean)
        {
        }
    }
}
