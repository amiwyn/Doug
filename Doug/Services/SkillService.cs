using System.Threading.Tasks;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Models.User;
using Doug.Skills;

namespace Doug.Services
{
    public interface ISkillService
    {
        Task<DougResponse> ActivateSkill(User user, ICombatable target, string channel);
    }

    public class SkillService : ISkillService
    {
        private readonly ISkillFactory _skillFactory;

        public SkillService(ISkillFactory skillFactory)
        {
            _skillFactory = skillFactory;
        }

        public async Task<DougResponse> ActivateSkill(User user, ICombatable target, string channel)
        {
            if (user.Loadout.Skillbook == null)
            {
                return new DougResponse(DougMessages.NoSkillEquipped);
            }

            return await user.Loadout.Skillbook.Activate(_skillFactory, user, target, channel);
        }
    }
}
