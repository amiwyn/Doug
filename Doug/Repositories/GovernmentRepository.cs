using System.Linq;
using Doug.Models;

namespace Doug.Repositories
{
    public interface IGovernmentRepository
    {
        User GetRuler();
        double GetTaxRate();
        void AddTaxesToRuler(int amount);
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
    }
}
