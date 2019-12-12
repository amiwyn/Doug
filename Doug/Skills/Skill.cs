using System;
using System.Threading.Tasks;
using Doug.Items.WeaponType;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Models.User;
using Doug.Repositories;

namespace Doug.Skills
{
    public class Skill
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int EnergyCost { get; set; }
        public int Cooldown { get; set; }
        public Type RequiredWeapon { get; set; }

        protected readonly IStatsRepository StatsRepository;

        public Skill(IStatsRepository statsRepository)
        {
            StatsRepository = statsRepository;
            RequiredWeapon = null;
        }

        public virtual async Task<DougResponse> Activate(User user, ICombatable target, string channel)
        {
            return await Task.FromResult(new DougResponse(DougMessages.SkillCannotBeActivated));
        }

        protected virtual bool CanActivateSkill(User user, ICombatable target, string channel, out DougResponse response)
        {
            var totalCooldown = Cooldown * (1 - user.CooldownReduction());
            if (!user.HasWeaponType(RequiredWeapon) && RequiredWeapon != null)
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
            StatsRepository.FireSkill(user.Id, TimeSpan.FromSeconds(totalCooldown), user.Energy);
            response = new DougResponse();
            return true;
        }
    }
}
