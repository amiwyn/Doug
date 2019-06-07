using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Repositories
{
    public interface ISlurRepository
    {
        ICollection<Slur> GetSlurs();
        Slur GetSlur(int slurId);
        Slur GetSlursFrom(string userId);
        void AddSlur(Slur slur);
        void RemoveSlur(int slurId);
    }
    public class SlurRepository
    {
    }
}
