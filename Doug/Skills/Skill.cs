using Doug.Models;
using Doug.Models.Combat;
using Doug.Repositories;

namespace Doug.Skills
{
    public abstract class Skill
    {
        protected readonly IStatsRepository StatsRepository;

        protected Skill(IStatsRepository statsRepository)
        {
            StatsRepository = statsRepository;
        }

        public int EnergyCost { get; set; }

        public virtual DougResponse Activate(User user, ICombatable target, string channel)
        {
            return new DougResponse(DougMessages.SkillCannotBeActivated);
        }
    }
}
