using System.Threading.Tasks;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Models.User;
using Doug.Skills;

namespace Doug.Items
{
    public abstract class SkillBook : EquipmentItem
    {
        public Skill Skill { get; set; }

        protected SkillBook()
        {
            Icon = ":skillbook:";
            Slot = EquipmentSlot.Skill;
        }

        public async Task<DougResponse> Activate(User user, ICombatable target, string channel)
        {
            return await Skill.Activate(user, target, channel);
        }
    }
}
