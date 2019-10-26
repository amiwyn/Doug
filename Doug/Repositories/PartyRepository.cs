using System.Collections.Generic;
using System.Linq;
using Doug.Models;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Doug.Repositories
{
    public interface IPartyRepository
    {
        Party GetParty(int id);
        Party GetPartyByUser(string user);
        List<Party> GetUniquePartiesFromUsers(IEnumerable<string> userIds);
        Party CreateParty(User host);
        void AddUserToParty(int partyId, string userId);
        void RemoveUserFromParty(int partyId, string userId);
    }

    public class PartyRepository : IPartyRepository
    {
        private readonly DougContext _db;

        public PartyRepository(DougContext db)
        {
            _db = db;
        }

        public Party GetParty(int id)
        {
            return _db.Parties
                .Include(party => party.Users)
                .Single(party => party.Id == id);
        }

        public Party GetPartyByUser(string user)
        {
            return _db.Parties
                .Include(party => party.Users)
                .SingleOrDefault(party => party.Users.Any(usr => usr.Id == user));
        }

        public List<Party> GetUniquePartiesFromUsers(IEnumerable<string> userIds)
        {
            return _db.Parties
                .Include(party => party.Users)
                .Where(party => party.Users.Any(user => userIds.Contains(user.Id))).ToList();
        }

        public Party CreateParty(User host)
        {
            var party = new Party {UserId = host.Id, Users = new List<User> {host}};
            _db.Parties.Add(party);
            _db.SaveChanges();
            return party;
        }

        public void AddUserToParty(int partyId, string userId)
        {
            var party = GetParty(partyId);
            var userToAdd = _db.Users.Single(usr => usr.Id == userId);
            party.Users.Add(userToAdd);
            _db.SaveChanges();
        }

        public void RemoveUserFromParty(int partyId, string userId)
        {
            var party = GetParty(partyId);
            var userToRemove = party.Users.Single(usr => usr.Id == userId);
            party.Users.Remove(userToRemove);
            _db.SaveChanges();
        }
    }
}
