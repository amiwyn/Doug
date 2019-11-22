using System.Threading.Tasks;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Models.User;
using Doug.Skills;

namespace Doug.Items
{
    public class SkillBook : EquipmentItem
    {
        public string SkillId { get; set; }

        public SkillBook()
        {
            Icon = ":skillbook:";
            Slot = EquipmentSlot.Skill;
        }

        public async Task<DougResponse> Activate(ISkillFactory skillFactory, User user, ICombatable target, string channel)
        {
            return await skillFactory.CreateSkill(SkillId).Activate(user, target, channel);
        }
    }
}
