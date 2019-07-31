using Doug.Models;
using Doug.Models.Combat;
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

        public DougResponse Activate(User user, ICombatable target, string channel)
        {
            return Skill.Activate(user, target, channel);
        }
    }
}
