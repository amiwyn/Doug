using Doug.Effects;
using Doug.Effects.EquipmentEffects;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Items
{
    public interface IEquipmentEffectFactory
    {
        EquipmentEffect CreateEffect(string effectId);
    }

    public class EquipmentEffectFactory : IEquipmentEffectFactory
    {
        private readonly IUserService _userService;
        private readonly IStatsRepository _statsRepository;
        private readonly ISlackWebApi _slack;

        public EquipmentEffectFactory(IUserService userService, IStatsRepository statsRepository, ISlackWebApi slack)
        {
            _userService = userService;
            _statsRepository = statsRepository;
            _slack = slack;
        }

        public EquipmentEffect CreateEffect(string effectId)
        {
            switch (effectId)
            {
                case Leeching.EffectId: return new Leeching(_statsRepository);
                case Seer.EffectId: return new Seer(_userService, _slack);
                case Burglar.EffectId: return new Burglar();
                case Reflective.EffectId: return new Reflective();
                case Safekeeping.EffectId: return new Safekeeping();
                case Greedy.EffectId: return new Greedy();
                case Incognito.EffectId: return new Incognito();
                case Lucky.EffectId: return new Lucky();
                default: return new EquipmentEffect();
            }
        }
    }
}
