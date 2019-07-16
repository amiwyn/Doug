using Doug.Effects.Buffs;
using Doug.Effects.Debuffs;
using Doug.Services;
using Doug.Slack;

namespace Doug.Effects
{
    public interface IEffectFactory
    {
        Effect CreateEffect(string effectId);
    }

    public class EffectFactory : IEffectFactory
    {
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;

        public EffectFactory(ISlackWebApi slack, IUserService userService)
        {
            _slack = slack;
            _userService = userService;
        }

        public Effect CreateEffect(string effectId)
        {
            switch (effectId)
            {
                case TrollBlessing.EffectId: return new TrollBlessing(_slack);
                case NicotineHigh.EffectId: return new NicotineHigh();
                case FrenchCurse.EffectId: return new FrenchCurse(_slack, _userService);
                case PickleBuff.EffectId: return new PickleBuff();
                case Luck.EffectId: return new Luck();
                default: return new UnknownEffect();
            }
        }
    }
}
