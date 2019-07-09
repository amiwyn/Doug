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

        public const string TrollBlessing = "troll_blessing";
        public const string NicotineHigh = "nicotine_high";
        public const string FrenchCurse = "french_curse";

        public Effect CreateEffect(string effectId)
        {
            switch (effectId)
            {
                case TrollBlessing:
                    return new TrollBlessing(_slack);
                case NicotineHigh:
                    return new NicotineHigh();
                case FrenchCurse:
                    return new FrenchCurse(_slack, _userService);
                default:
                    return new UnknownEffect();
            }
        }
    }
}
