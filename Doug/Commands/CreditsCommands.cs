using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Commands
{
    public interface ICreditsCommands
    {
        string Balance(Command command);
        void Stats(Command command);
        void Give(Command command);
        string Gamble(Command command);
    }

    public class CreditsCommands
    {
    }
}
