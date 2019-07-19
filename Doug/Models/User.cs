using System;
using System.Collections.Generic;
using Doug.Items;
using System.Linq;
using Doug.Effects;

namespace Doug.Models
{
    public class User
    {
        private const int BaseAttackCooldown = 30;
        private const int BaseStealCooldown = 30;

        private int _health;
        private int _energy;

        public string Id { get; set; }
        public int Credits { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }
        public List<UserEffect> Effects { get; set; }
        public Loadout Loadout { get; set; }
        public long Experience { get; set; }
        public DateTime AttackCooldown { get; set; }
        public DateTime StealCooldown { get; set; }

        public int Health
        {
            get => _health;
            set
            {
                if (value <= 0)
                {
                    _health = 0;
                    return;
                }

                if (value >= TotalHealth())
                {
                    _health = TotalHealth();
                    return;
                }

                _health = value;
            }
        }

        public int Energy
        {
            get => _energy;
            set
            {
                if (value <= 0)
                {
                    _energy = 0;
                    return;
                }

                if (value >= TotalEnergy())
                {
                    _energy = TotalEnergy();
                    return;
                }

                _energy = value;
            }
        }

        public int Luck { get; set; }
        public int Agility { get; set; }
        public int Strength { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }

        public int Level => (int)Math.Floor(Math.Sqrt(Experience) * 0.1 + 1);
        public int TotalStatsPoints => (int)Math.Floor(Level + 5 * Math.Floor(Level * 0.1)) + 4;
        public int FreeStatsPoints => TotalStatsPoints + 25 - (Luck + Agility + Strength + Constitution + Intelligence);
        public int Attack => (int)Math.Floor(TotalStrength() * 2.0);

        public User()
        {
            InventoryItems = new List<InventoryItem>();
            Effects = new List<UserEffect>();
            Loadout = new Loadout();
            
            Luck = 5;
            Agility = 5;
            Strength = 5;
            Constitution = 5;
            Intelligence = 5;
        }

        public void LevelUp()
        {
            Health = TotalHealth();
            Energy = TotalEnergy();
        }

        public int TotalLuck() => Loadout.Luck + Luck + Effects.Sum(userEffect => userEffect.Effect.Luck);
        public int TotalAgility() => Loadout.Agility + Agility + Effects.Sum(userEffect => userEffect.Effect.Agility);
        public int TotalStrength() => Loadout.Strength + Strength + Effects.Sum(userEffect => userEffect.Effect.Strength);
        public int TotalConstitution() => Loadout.Constitution + Constitution + Effects.Sum(userEffect => userEffect.Effect.Constitution);
        public int TotalIntelligence() => Loadout.Intelligence + Intelligence + Effects.Sum(userEffect => userEffect.Effect.Intelligence);
        public int TotalAttack() => Loadout.Attack + Attack + Effects.Sum(userEffect => userEffect.Effect.Attack);
        public int TotalDefense() => Loadout.Defense + (int)Math.Floor(2.0 * TotalConstitution()) + Effects.Sum(userEffect => userEffect.Effect.Intelligence);
        public int TotalDodge() => Loadout.Dodge + TotalAgility() + Effects.Sum(userEffect => userEffect.Effect.Dodge);
        public int TotalHitrate() => Loadout.Hitrate + 5 + Effects.Sum(userEffect => userEffect.Effect.Hitrate);

        public void LoadItems(IItemFactory itemFactory)
        {
            InventoryItems.ForEach(item => item.CreateItem(itemFactory));
            Loadout.CreateEquipment(itemFactory);
        }

        public void LoadEffects(IEffectFactory effectFactory)
        {
            Effects.ForEach(effect => effect.CreateEffect(effectFactory));
        }

        public double GetExperienceAdvancement()
        {
            var nextLevelExp = Math.Pow((Level + 1) * 10 - 10, 2);
            var prevLevelExp = Math.Pow((Level - 1) * 10, 2);

            return (Experience - prevLevelExp) / (nextLevelExp - prevLevelExp);
        }

        public int TotalHealth()
        {
            var healthFromLevel = (int)Math.Floor(15.0 * Level + 85);
            var healthFromConstitution = (int)Math.Floor(15.0 * TotalConstitution() - 75);
            var healthFromEffects = Effects.Sum(userEffect => userEffect.Effect.Health);
            return healthFromLevel + healthFromConstitution + healthFromEffects + Loadout.Health;
        }

        public int TotalEnergy()
        {
            var energyFromLevel = (int)Math.Floor(5.0 * Level + 20);
            var energyFromIntelligence = (int)Math.Floor(5.0 * TotalIntelligence() - 25);
            var energyFromEffects = Effects.Sum(userEffect => userEffect.Effect.Energy);
            return energyFromLevel + energyFromIntelligence + energyFromEffects + Loadout.Energy;
        }

        public double BaseGambleChance()
        {
            var luckInfluence = Math.Log(TotalLuck() / 5.0) / (Math.Log(1.2) * 100);
            return 0.5 + luckInfluence;
        }

        public double BaseStealSuccessRate()
        {
            var luckInfluence = (Math.Sqrt(TotalLuck()) - Math.Sqrt(5)) * 0.1;
            return 0.25 + luckInfluence;
        }

        public void Dies()
        {
            Health = 1;
            Energy = 0;

            var nextLevelExp = (long)Math.Pow((Level + 1) * 10 - 10, 2);
            var prevLevelExp = (long)Math.Pow((Level - 1) * 10, 2);

            var expLoss = (long)(0.1 * (nextLevelExp - prevLevelExp));
            Experience = Experience - expLoss <= prevLevelExp ? prevLevelExp : Experience - expLoss;
        }

        public int ApplyPhysicalDamage(int damage)
        {
            var reducedDamage = damage - (damage * (Loadout.Resistance / 100) + TotalDefense());
            reducedDamage = reducedDamage <= 0 ? 1 : reducedDamage;

            Health -= reducedDamage;

            return reducedDamage;
        }

        public int ApplyMagicalDamage(int damage)
        {
            Health -= damage;
            return damage;
        }

        public AttackStatus AttackUser(User user, out int damage, IEventDispatcher eventDispatcher)
        {
            var random = new Random();
            var status = AttackStatus.Normal;

            if (Loadout.GetDamageType() == DamageType.Magical)
            {
                damage = TotalIntelligence() * 2;
                damage = eventDispatcher.OnAttacking(this, user, damage);
                damage = user.ApplyMagicalDamage(damage);
                return status;
            }

            damage = TotalAttack();
            if (random.NextDouble() < 0.1)
            {
                damage = TotalAttack() * 2;
                status = AttackStatus.Critical;
            }

            var missChance = (user.TotalDodge() - TotalHitrate()) * 0.01;
            if (random.NextDouble() < missChance)
            {
                return AttackStatus.Missed;
            }
            
            damage = eventDispatcher.OnAttacking(this, user, damage);

            damage = user.ApplyPhysicalDamage(damage);
            return status;
        }

        public void RegenerateHealth() => Health += (int)(TotalHealth() * 0.2);
        public double BaseOpponentStealSuccessRate() => 0.75;
        public int BaseStealAmount() => (int)Math.Floor(3 * (Math.Sqrt(TotalAgility()) - Math.Sqrt(5)) + 1);
        public bool HasEnoughCreditsForAmount(int amount) => Credits - amount >= 0;
        public string NotEnoughCreditsForAmountResponse(int amount) => string.Format(DougMessages.NotEnoughCredits, amount, Credits);
        public bool HasEmptyInventory() => !InventoryItems.Any();
        public bool IsDead() => Health <= 0;
        public bool IsAttackOnCooldown() => DateTime.UtcNow <= AttackCooldown;
        public bool IsStealOnCooldown() => DateTime.UtcNow <= StealCooldown;
        public int CalculateAttackCooldownRemaining() => (int)(AttackCooldown - DateTime.UtcNow).TotalSeconds;
        public int CalculateStealCooldownRemaining() => (int)(StealCooldown - DateTime.UtcNow).TotalSeconds;
        public TimeSpan GetStealCooldown() => TimeSpan.FromSeconds(BaseStealCooldown);
        public TimeSpan GetAttackCooldown() => TimeSpan.FromSeconds(BaseAttackCooldown / (Math.Abs(Loadout.AttackSpeed) < 0.001 ? 1 : Loadout.AttackSpeed));
    }
}
