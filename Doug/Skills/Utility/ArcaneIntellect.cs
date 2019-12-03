using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Skills.Utility
{
    public class ArcaneIntellect : BuffSkill
    {
        public const string SkillId = "arcaneIntellect";

        public ArcaneIntellect(IStatsRepository statsRepository, IEffectRepository effectRepository, ISlackWebApi slack, IUserService userService) : base(statsRepository, effectRepository, slack, userService)
        {
            Id = SkillId;
            Name = "ArcaneIntellect";
            EnergyCost = 35;
            Cooldown = 15;
            BuffId = Effects.Buffs.ArcaneIntellect.EffectId;
            BuffName = "Arcane Intellect";
            Duration = 120;
        }
    }
}
