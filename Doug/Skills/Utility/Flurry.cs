using System.Threading.Tasks;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Skills.Utility
{
    public class Flurry : BuffSkill
    {
        public const string SkillId = "flurry";

        public Flurry(IStatsRepository statsRepository, IEffectRepository effectRepository, ISlackWebApi slack, IUserService userService) : base(statsRepository, effectRepository, slack, userService)
        {
            Id = SkillId;
            Name = "Flurry";
            EnergyCost = 25;
            Cooldown = 60;
            BuffId = Effects.Buffs.Flurry.EffectId;
            BuffName = "Flurry";
            Duration = 1;
        }
        public override async Task<DougResponse> Activate(User user, ICombatable target, string channel)
        {
            return await base.Activate(user, user, channel);
        }
    }
}
