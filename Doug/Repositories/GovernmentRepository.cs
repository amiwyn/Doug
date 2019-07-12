using System.Linq;
using Doug.Models;

namespace Doug.Repositories
{
    public interface IGovernmentRepository
    {
        User GetRuler();
        double GetTaxRate();
        void AddTaxesToRuler(int amount);
        void StartRevolutionVote(string userId, string timestamp);
        void Revolution();
        Government GetGovernment();
    }

    public class GovernmentRepository : IGovernmentRepository
    {
        private readonly DougContext _db;

        public GovernmentRepository(DougContext db)
        {
            _db = db;
        }

        public User GetRuler()
        {
            var government = _db.Government.Single();
            return _db.Users.Single(user => user.Id == government.Ruler);
        }

        public double GetTaxRate()
        {
            return _db.Government.Single().TaxRate;
        }

        public void AddTaxesToRuler(int amount)
        {
            var ruler = GetRuler();
            ruler.Credits += amount;
            _db.SaveChanges();
        }

        public void StartRevolutionVote(string userId, string timestamp)
        {
            var government = _db.Government.Single();
            government.RevolutionLeader = userId;
            government.RevolutionTimestamp = timestamp;
            _db.SaveChanges();
        }

        public void Revolution()
        {
            var government = _db.Government.Single();
            government.Ruler = government.RevolutionLeader;
            _db.SaveChanges();
        }

        public Government GetGovernment()
        {
            return _db.Government.Single();
        }
    }
}
