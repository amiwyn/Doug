using Doug.Effects.Buffs;
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

        public EffectFactory(ISlackWebApi slack)
        {
            _slack = slack;
        }

        public const string TrollBlessing = "troll_blessing";

        public Effect CreateEffect(string effectId)
        {
            switch (effectId)
            {
                case TrollBlessing:
                    return new TrollBlessing(_slack);
                default:
                    return new UnknownEffect();
            }
        }
    }
}
