using System;
using System.Threading.Tasks;
using Doug.Items.WeaponType;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Repositories;

namespace Doug.Skills
{
    public abstract class Skill
    {
        public string Name { get; set; }
        public int EnergyCost { get; set; }
        public int Cooldown { get; set; }
        public Type RequiredWeapon { get; set; }

        protected readonly IStatsRepository StatsRepository;

        protected Skill(IStatsRepository statsRepository)
        {
            StatsRepository = statsRepository;
            RequiredWeapon = typeof(Weapon);
        }

        public virtual async Task<DougResponse> Activate(User user, ICombatable target, string channel)
        {
            return await Task.FromResult(new DougResponse(DougMessages.SkillCannotBeActivated));
        }

        protected virtual bool CanActivateSkill(User user, ICombatable target, string channel, out DougResponse response)
        {
            if (!user.HasWeaponType(RequiredWeapon))
            {
                response = new DougResponse(string.Format(DougMessages.WrongWeaponForSkill, RequiredWeapon.Name));
                return false;
            }

            if (user.IsSkillOnCooldown())
            {
                response = new DougResponse(string.Format(DougMessages.CommandOnCooldown, user.CalculateStealCooldownRemaining()));
                return false;
            }

            if (!user.HasEnoughEnergyForCost(EnergyCost))
            {
                response = new DougResponse(DougMessages.NotEnoughEnergy);
                return false;
            }

            user.Energy -= EnergyCost;
            StatsRepository.FireSkill(user.Id, TimeSpan.FromSeconds(Cooldown), user.Energy);
            response = new DougResponse();
            return true;
        }
    }
}
