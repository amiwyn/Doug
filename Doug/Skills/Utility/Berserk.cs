using System.Threading.Tasks;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Skills.Utility
{
    public class Berserk : BuffSkill
    {
        public const string SkillId = "berserk";

        public Berserk(IStatsRepository statsRepository, IEffectRepository effectRepository, ISlackWebApi slack, IUserService userService) : base(statsRepository, effectRepository, slack, userService)
        {
            Id = SkillId;
            Name = "Berserk";
            EnergyCost = 25;
            Cooldown = 30;
            BuffId = Effects.Buffs.Berserk.EffectId;
            BuffName = "Berserk!";
            Duration = 2;
        }

        public override async Task<DougResponse> Activate(User user, ICombatable target, string channel) {
            return await base.Activate(user, user, channel);
        }
    }
}
