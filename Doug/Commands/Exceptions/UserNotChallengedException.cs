﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Commands
{
    public class UserNotChallengedException : ApplicationException
    {
        public UserNotChallengedException() : base(DougMessages.NotChallenged)
        {
        }
    }
}