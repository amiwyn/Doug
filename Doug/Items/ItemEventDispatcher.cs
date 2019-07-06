using Doug.Models;
using System.Linq;

namespace Doug.Items
{
    public interface IItemEventDispatcher
    {
        string OnFlaming(User caller, User target, Command command, string slur);
        double OnGambling(User user, double baseChance);
        double OnStealingChance(User user, double baseChance);
        double OnGettingStolenChance(User user, double baseChance);
        int OnStealingAmount(User user, int baseAmount);
        string OnMention(User user, string mention);
        bool OnDeath(User user);
        bool OnDeathByUser(User user, User killer);
    }

    public class ItemEventDispatcher : IItemEventDispatcher
    {
        public bool OnDeath(User user)
        {
            return user.Loadout.Equipment.Aggregate(true, (isDead, item) => item.Value.OnDeath() && isDead);
        }

        public bool OnDeathByUser(User user, User killer)
        {
            return user.Loadout.Equipment.Aggregate(true, (isDead, item) => item.Value.OnDeathByUser(killer) && isDead);
        }

        public string OnFlaming(User caller, User target, Command command, string slur)
        {
            slur = target.Loadout.Equipment.Aggregate(slur, (acc, item) => item.Value.OnGettingFlamed(command, acc));

            return caller.Loadout.Equipment.Aggregate(slur, (acc, item) => item.Value.OnFlaming(command, acc));
        }

        public double OnGambling(User user, double baseChance)
        {
            return user.Loadout.Equipment.Aggregate(baseChance, (chance, item) => item.Value.OnGambling(chance));
        }

        public double OnStealingChance(User user, double baseChance)
        {
            return user.Loadout.Equipment.Aggregate(baseChance, (chance, item) => item.Value.OnStealingChance(chance));
        }

        public double OnGettingStolenChance(User user, double baseChance)
        {
            return user.Loadout.Equipment.Aggregate(baseChance, (chance, item) => item.Value.OnGettingStolenChance(chance));
        }

        public int OnStealingAmount(User user, int baseAmount)
        {
            return user.Loadout.Equipment.Aggregate(baseAmount, (amount, item) => item.Value.OnStealingAmount(amount));
        }

        public string OnMention(User user, string mention)
        {
            return user.Loadout.Equipment.Aggregate(mention, (amount, item) => item.Value.OnMention(mention));
        }
    }
}
