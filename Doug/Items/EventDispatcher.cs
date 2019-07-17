using System;
using System.Collections.Generic;
using Doug.Models;
using System.Linq;

namespace Doug.Items
{
    public interface IEventDispatcher
    {
        string OnFlaming(User caller, User target, Command command, string slur);
        double OnGambling(User user, double baseChance);
        double OnStealingChance(User user, double baseChance);
        double OnGettingStolenChance(User user, double baseChance);
        int OnStealingAmount(User user, int baseAmount);
        string OnMention(User user, string mention);
        bool OnDeath(User user);
        void OnDeathByUser(User user, User killer);
        bool OnKick(User user, User kicker, string channel);
        int OnAttacking(User attacker, User target, int damage);
    }

    public class EventDispatcher : IEventDispatcher
    {
        private T PropagateItemEvents<T>(User user, T baseValue, Func<T, EquipmentItem, T> aggregateFunction)
        {
            var equipment = new List<EquipmentItem>(user.Loadout.Equipment.Select(equip => equip.Value));

            return equipment.Aggregate(baseValue, aggregateFunction);
        }

        private T PropagateEffectEvents<T>(User user, T baseValue, Func<T, UserEffect, T> aggregateFunction)
        {
            var effects = new List<UserEffect>(user.Effects);

            return effects.Aggregate(baseValue, aggregateFunction);
        }

        public bool OnDeath(User user)
        {
            return PropagateItemEvents(user, true, (isDead, item) => item.OnDeath() && isDead);
        }

        public void OnDeathByUser(User user, User killer)
        {
            var equipment = new List<EquipmentItem>(user.Loadout.Equipment.Select(equip => equip.Value));
            equipment.ForEach(equip => equip.OnDeathByUser(killer));
        }

        public bool OnKick(User user, User kicker, string channel)
        {
            return PropagateEffectEvents(user, true, (isKicked, effect) => effect.Effect.OnKick(kicker, channel) && isKicked);
        }

        public int OnAttacking(User attacker, User target, int damage)
        {
            var attackerDamage = PropagateEffectEvents(attacker, damage, (damageSum, effect) => effect.Effect.OnAttacking(attacker, target, damageSum));
            return PropagateEffectEvents(target, attackerDamage, (damageSum, effect) => effect.Effect.OnGettingAttacked(attacker, target, damageSum));
        }

        public string OnFlaming(User caller, User target, Command command, string slur)
        {
            slur = PropagateItemEvents(target, slur, (acc, item) => item.OnGettingFlamed(command, acc));
            slur = PropagateEffectEvents(target, slur, (acc, effect) => effect.Effect.OnGettingFlamed(command, acc));

            return PropagateItemEvents(caller, slur, (acc, item) => item.OnFlaming(command, acc));
        }

        public double OnGambling(User user, double baseChance)
        {
            return PropagateItemEvents(user, baseChance, (chance, item) => item.OnGambling(chance));
        }

        public double OnStealingChance(User user, double baseChance)
        {
            return PropagateItemEvents(user, baseChance, (chance, item) => item.OnStealingChance(chance));
        }

        public double OnGettingStolenChance(User user, double baseChance)
        {
            return PropagateItemEvents(user, baseChance, (chance, item) => item.OnGettingStolenChance(chance));
        }

        public int OnStealingAmount(User user, int baseAmount)
        {
            return PropagateItemEvents(user, baseAmount, (amount, item) => item.OnStealingAmount(amount));
        }

        public string OnMention(User user, string mention)
        {
            return PropagateItemEvents(user, mention, (amount, item) => item.OnMention(mention));
        }
    }
}
