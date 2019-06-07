using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Commands
{
    public interface ISlursCommands
    {
        void Flame(Command command);
        string AddSlur(Command command);
        string Clean(Command command);
        string WhoLast(Command command);
        string Slurs(Command command);
    }

    public class SlursCommands
    {
        
    }
}
