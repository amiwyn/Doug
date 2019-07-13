using System;
using System.Linq;
using Doug.Models;

namespace Doug.Repositories
{
    public interface IEffectRepository
    {
        void AddEffect(User user, string effectId, int durationMinutes);
        void RemoveAllEffects(User user);
    }

    public class EffectRepository : IEffectRepository
    {
        private readonly DougContext _db;

        public EffectRepository(DougContext db)
        {
            _db = db;
        }

        public void AddEffect(User user, string effectId, int durationMinutes)
        {
            var effect = _db.UserEffect.SingleOrDefault(eff => eff.UserId == user.Id && eff.EffectId == effectId);

            if (effect != null)
            {
                _db.UserEffect.Remove(effect);
            }

            user.Effects.Add(new UserEffect
            {
                EffectId = effectId,
                UserId = user.Id,
                EndTime = DateTime.UtcNow.AddMinutes(durationMinutes)
            });

            _db.SaveChanges();
        }

        public void RemoveAllEffects(User user)
        {
            user.Effects.Clear();
            _db.SaveChanges();
        }
    }
}
