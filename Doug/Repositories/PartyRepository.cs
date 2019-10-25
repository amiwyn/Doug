using System.Collections.Generic;
using System.Linq;
using Doug.Models;

namespace Doug.Repositories
{
    public interface IPartyRepository
    {
        Party GetParty(int id);
        Party GetPartyByUser(string user);
        Party CreateParty(User host);
        void AddUserToParty(int partyId, User user);
        void RemoveUserFromParty(int partyId, User user);
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
            return _db.Parties.Single(party => party.Id == id);
        }

        public Party GetPartyByUser(string user)
        {
            return _db.Parties.SingleOrDefault(party => party.Users.Any(usr => usr.Id == user));
        }

        public Party CreateParty(User host)
        {
            var party = new Party {UserId = host.Id, Users = new List<User> {host}};
            _db.Parties.Add(party);
            _db.SaveChanges();
            return party;
        }

        public void AddUserToParty(int partyId, User user)
        {
            var party = GetParty(partyId);
            party.Users.Add(user);
            _db.SaveChanges();
        }

        public void RemoveUserFromParty(int partyId, User user)
        {
            var party = GetParty(partyId);
            var userToRemove = party.Users.Single(usr => usr.Id == user.Id);
            party.Users.Remove(userToRemove);
            _db.SaveChanges();
        }
    }
}
