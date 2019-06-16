using Doug.Models;
using System.Collections.Generic;
using System.Linq;

namespace Doug.Repositories
{
    public interface ISlurRepository
    {
        ICollection<Slur> GetSlurs();
        Slur GetSlur(int slurId);
        ICollection<Slur> GetSlursFrom(string userId);
        void AddSlur(Slur slur);
        void RemoveSlur(int slurId);
        Slur GetSlurByText(string text);
        ICollection<RecentFlame> GetRecentSlurs();
        void LogRecentSlur(int slurId, string timestamp);
        void ClearRecentSlurs();
        void IncrementFat();
        int GetFat();
    }
    public class SlurRepository : ISlurRepository
    {
        private const int MaxHistoryLength = 8;
        private readonly DougContext _db;

        public SlurRepository(DougContext dougContext)
        {
            _db = dougContext;
        }

        public void AddSlur(Slur slur)
        {
            _db.Slurs.Add(slur);
        }

        public void ClearRecentSlurs()
        {
            _db.RecentSlurs.RemoveRange(_db.RecentSlurs);
            _db.SaveChanges();
        }

        public int GetFat()
        {
            return _db.Channel.Single().FatCounter;
        }

        public ICollection<RecentFlame> GetRecentSlurs()
        {
            return _db.RecentSlurs.ToList();
        }

        public Slur GetSlur(int slurId)
        {
            return _db.Slurs.Single(slur => slur.Id == slurId);
        }

        public Slur GetSlurByText(string text)
        {
            return _db.Slurs.FirstOrDefault(slur => slur.Text == text);
        }

        public ICollection<Slur> GetSlurs()
        {
            return _db.Slurs.Where(slur => slur.Active).ToList();
        }

        public ICollection<Slur> GetSlursFrom(string userId)
        {
            return _db.Slurs.Where(slur => slur.CreatedBy == userId).ToList();
        }

        public void IncrementFat()
        {
            _db.Channel.Single().FatCounter++;
            _db.SaveChanges();
        }

        public void LogRecentSlur(int slurId, string timestamp)
        {
            if (_db.RecentSlurs.Count() >= MaxHistoryLength)
            {
                var id = _db.RecentSlurs.Min(slur => slur.Id);
                var slurToDelete = _db.RecentSlurs.Single(slur => slur.Id == id);
                _db.RecentSlurs.Remove(slurToDelete);
            }
            _db.RecentSlurs.Add(new RecentFlame() { SlurId = slurId, TimeStamp = timestamp });
            _db.SaveChanges();
        }

        public void RemoveSlur(int slurId)
        {
            var slur = _db.Slurs.Single(slr => slr.Id == slurId);
            slur.Active = false;
            _db.SaveChanges();
        }
    }
}
