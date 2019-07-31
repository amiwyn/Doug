using System;
using System.Collections.Generic;
using Doug.Items;
using System.Linq;
using Doug.Effects;
using Doug.Models.Combat;

namespace Doug.Models
{
    public class User : ICombatable
    {
        private const int BaseAttackCooldown = 30;
        private const int BaseAttackSpeed = 100;

        private int _health;
        private int _energy;

        public string Id { get; set; }
        public int Credits { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }
        public List<UserEffect> Effects { get; set; }
        public Loadout Loadout { get; set; }
        public long Experience { get; set; }
        public DateTime AttackCooldown { get; set; }
        public DateTime SkillCooldown { get; set; }

        public int Luck { get; set; }
        public int Agility { get; set; }
        public int Strength { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }

        public int Level => (int)Math.Floor(Math.Sqrt(Experience) * 0.05 + 1);
        public int TotalStatsPoints => (int)Math.Floor(Level + 5 * Math.Floor(Level * 0.1)) + 4;
        public int FreeStatsPoints => TotalStatsPoints + 25 - (Luck + Agility + Strength + Constitution + Intelligence);
        public int Attack => (int)Math.Floor(Strength * 2.5);

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

        public int TotalLuck() => Loadout.Luck + Luck + Effects.Sum(userEffect => userEffect.Effect.Luck);
        public int TotalAgility() => Loadout.Agility + Agility + Effects.Sum(userEffect => userEffect.Effect.Agility);
        public int TotalStrength() => Loadout.Strength + Strength + Effects.Sum(userEffect => userEffect.Effect.Strength);
        public int TotalConstitution() => Loadout.Constitution + Constitution + Effects.Sum(userEffect => userEffect.Effect.Constitution);
        public int TotalIntelligence() => Loadout.Intelligence + Intelligence + Effects.Sum(userEffect => userEffect.Effect.Intelligence);
        public int TotalDefense() => Loadout.Defense + (int)Math.Floor(2.0 * TotalConstitution()) + Effects.Sum(userEffect => userEffect.Effect.Intelligence);
        public int TotalDodge() => Loadout.Dodge + TotalAgility() + Effects.Sum(userEffect => userEffect.Effect.Dodge);
        public int TotalHitrate() => Loadout.Hitrate + 5 + Effects.Sum(userEffect => userEffect.Effect.Hitrate);
        public int MaxAttack() => Loadout.MaxAttack + Attack + Effects.Sum(userEffect => userEffect.Effect.Attack);
        public int MinAttack() => Loadout.MinAttack + Attack + Effects.Sum(userEffect => userEffect.Effect.Attack);
        public int TotalAttackSpeed() => BaseAttackSpeed + Loadout.AttackSpeed + TotalAgility() / 2;

        public void RegenerateHealth() => Health += (int)(TotalHealth() * 0.2);
        public double BaseOpponentStealSuccessRate() => 0.75;
        public int BaseStealAmount() => (int)Math.Floor(3 * (Math.Sqrt(TotalAgility()) - Math.Sqrt(5)) + 1);
        public bool HasEnoughCreditsForAmount(int amount) => Credits - amount >= 0;
        public string NotEnoughCreditsForAmountResponse(int amount) => string.Format(DougMessages.NotEnoughCredits, amount, Credits);
        public bool HasEmptyInventory() => !InventoryItems.Any();
        public bool HasEnoughEnergyForCost(int cost) => Energy - cost >= 0;
        public bool IsDead() => Health <= 0;
        public bool IsAttackOnCooldown() => DateTime.UtcNow <= AttackCooldown;
        public bool IsStealOnCooldown() => DateTime.UtcNow <= SkillCooldown;
        public int CalculateAttackCooldownRemaining() => (int)(AttackCooldown - DateTime.UtcNow).TotalSeconds;
        public int CalculateStealCooldownRemaining() => (int)(SkillCooldown - DateTime.UtcNow).TotalSeconds;
        public TimeSpan GetAttackCooldown() => TimeSpan.FromSeconds(BaseAttackCooldown * 100.0 / TotalAttackSpeed());
        public double GetExperienceAdvancement() => (Experience - PrevLevelExp()) / (NextLevelExp() - PrevLevelExp());
        private double NextLevelExp() => Math.Pow((Level + 1) * 20 - 20, 2);
        private double PrevLevelExp() => Math.Pow((Level - 1) * 20, 2);

        public void LevelUp()
        {
            Health = TotalHealth();
            Energy = TotalEnergy();
        }

        public void LoadItems(IItemFactory itemFactory)
        {
            InventoryItems.ForEach(item => item.CreateItem(itemFactory));
            Loadout.CreateEquipment(itemFactory);
        }

        public void LoadEffects(IEffectFactory effectFactory)
        {
            Effects.ForEach(effect => effect.CreateEffect(effectFactory));
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

        public double ExtraDropChance()
        {
            return Math.Log(TotalLuck() / 5.0) / (Math.Log(1.2) * 100);
        }

        public void Dies()
        {
            Health = 1;
            Energy = 0;

            var expLoss = (long)(0.1 * (NextLevelExp() - PrevLevelExp()));
            Experience = Experience - expLoss <= PrevLevelExp() ? (long)PrevLevelExp() : Experience - expLoss;
        }

        public List<EquipmentItem> Equip(EquipmentItem item)
        {
            return CanEquip(item) ? Loadout.Equip(item) : new List<EquipmentItem>();
        }

        public bool CanEquip(EquipmentItem item)
        {
            return Level >= item.LevelRequirement &&
                   TotalStrength() >= item.StrengthRequirement &&
                   TotalAgility() >= item.AgilityRequirement &&
                   TotalIntelligence() >= item.IntelligenceRequirement &&
                   TotalLuck() >= item.LuckRequirement &&
                   TotalConstitution() >= item.ConstitutionRequirement;
        }

        public Attack AttackTarget(ICombatable target, IEventDispatcher eventDispatcher)
        {
            Attack attack = new PhysicalAttack(this, MinAttack(), MaxAttack(), TotalHitrate(), TotalLuck());

            if (Loadout.GetDamageType() == DamageType.Magical)
            {
                attack = new MagicAttack(this, TotalIntelligence());
            }

            attack.Damage = eventDispatcher.OnAttacking(this, target, attack.Damage);

            return target.ReceiveAttack(attack, eventDispatcher);
        }

        public Attack ReceiveAttack(Attack attack, IEventDispatcher eventDispatcher)
        {
            if (eventDispatcher.OnAttackedInvincibility(attack.Attacker, this))
            {
                attack.Status = AttackStatus.Invincible;
                attack.Damage = 0;
                return attack;
            }

            if (attack is PhysicalAttack physicalAttack)
            {
                return ApplyPhysicalDamage(physicalAttack);
            }

            ApplyMagicalDamage(attack.Damage);
            return attack;
        }

        private PhysicalAttack ApplyPhysicalDamage(PhysicalAttack attack)
        {
            var missChance = (TotalDodge() - attack.AttackersHitrate) * 0.01;
            if (new Random().NextDouble() < missChance)
            {
                attack.Status = AttackStatus.Missed;
                attack.Damage = 0;
                return attack;
            }

            var reducedDamage = attack.Damage - (int)Math.Ceiling(attack.Damage * Loadout.Resistance * 0.01 + TotalDefense());
            reducedDamage = reducedDamage <= 0 ? 1 : reducedDamage;

            attack.Damage = reducedDamage;
            if (attack.Status == AttackStatus.Critical)
            {
                attack.Damage *= 2;
            }

            Health -= attack.Damage;

            return attack;
        }

        private void ApplyMagicalDamage(int damage)
        {
            Health -= damage;
        }
    }
}
