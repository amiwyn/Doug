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
        string OnStealingFailed(User user, string targetUserMention, string response);
        string OnDeath(User user, User killer, string response);
    }

    public class ItemEventDispatcher : IItemEventDispatcher
    {
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
            return user.Loadout.Equipment.Aggregate(mention, (acc, item) => item.Value.OnMention(mention));
        }

        public string OnStealingFailed(User user, string targetUserMention, string response)
        {
            return user.Loadout.Equipment.Aggregate(response, (acc, item) => item.Value.OnStealingFailed(response, targetUserMention));
        }

        public string OnDeath(User user, User killer, string response)
        {
            return user.Loadout.Equipment.Aggregate(response, (acc, item) => item.Value.OnDeath(response, user, killer));
        }
    }
}
