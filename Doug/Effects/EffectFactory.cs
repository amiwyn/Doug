using Doug.Effects.Buffs;
using Doug.Effects.Debuffs;
using Doug.Repositories;
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
        private readonly IEffectRepository _effectRepository;

        public EffectFactory(ISlackWebApi slack, IUserService userService, IEffectRepository effectRepository)
        {
            _slack = slack;
            _userService = userService;
            _effectRepository = effectRepository;
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
                case ArcaneIntellect.EffectId: return new ArcaneIntellect();
                case Berserk.EffectId: return new Berserk();
                case Flurry.EffectId: return new Flurry();
                case MortuaryGrace.EffectId: return new MortuaryGrace(_effectRepository);
                default: return new UnknownEffect();
            }
        }

        // yolo
        public static string GetEffectName(string effectId)
        {
            switch (effectId)
            {
                case TrollBlessing.EffectId: return new TrollBlessing(null).Name;
                case NicotineHigh.EffectId: return new NicotineHigh().Name;
                case FrenchCurse.EffectId: return new FrenchCurse(null, null).Name;
                case PickleBuff.EffectId: return new PickleBuff().Name;
                case ArcaneIntellect.EffectId: return new ArcaneIntellect().Name;
                case Berserk.EffectId: return new Berserk().Name;
                case Flurry.EffectId: return new Flurry().Name;
                case Luck.EffectId: return new Luck().Name;
                case MortuaryGrace.EffectId: return new MortuaryGrace(null).Name;
                default: return new UnknownEffect().Name;
            }
        }
    }
}
