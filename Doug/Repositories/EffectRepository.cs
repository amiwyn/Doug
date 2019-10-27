using System;
using System.Linq;
using Doug.Effects;
using Doug.Models.User;

namespace Doug.Repositories
{
    public interface IEffectRepository
    {
        void AddEffect(User user, string effectId, int durationMinutes);
        void RemoveAllEffects(User user);
        void RemoveEffect(User user, string effectId);
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
                Effect = new UnknownEffect(),
                EndTime = DateTime.UtcNow.AddMinutes(durationMinutes)
            });

            _db.SaveChanges();
        }

        public void RemoveAllEffects(User user)
        {
            user.Effects.Clear();
            _db.SaveChanges();
        }

        public void RemoveEffect(User user, string effectId)
        {
            var effect = user.Effects.SingleOrDefault(eff => eff.EffectId == effectId);

            if (effect != null)
            {
                user.Effects.Remove(effect);
            }

            _db.SaveChanges();
        }
    }
}
