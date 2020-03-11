using System;
using System.Collections.Generic;
using System.Linq;
using Doug.Effects;
using Doug.Items;
using Doug.Models.Combat;
using Doug.Models.Monsters;

namespace Doug.Models.User
{
    public class User : ICombatable
    {
        private const int BaseAttackCooldown = 30;
        private const int BaseAttackSpeed = 100;
        private const int BaseHealthRegen = 1;
        private const int BaseEnergyRegen = 1;

        private int _health;
        private int _energy;

        public string Id { get; set; }
        public int Credits { get; set; }

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

        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public string Token { get; set; }
        public int LotteryTickets { get; set; }


        public long Experience { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }
        public List<UserEffect> Effects { get; set; }
        public Loadout Loadout { get; set; }
        public DateTime AttackCooldown { get; set; }
        public DateTime SkillCooldown { get; set; }

        public int Level => (int)Math.Floor(Math.Sqrt(Experience) * 0.05 + 1);
        public int TotalStatsPoints => (int)Math.Floor(Level + 5 * Math.Floor(Level * 0.1)) + 4;
        public int FreeStatsPoints => TotalStatsPoints + 25 - (Luck + Agility + Strength + Constitution + Intelligence);
        public int Attack => (int)Math.Floor(TotalStrength() * 2.5);

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

        public int TotalLuck() => (int)Math.Floor((Loadout.Sum(stat => stat.Luck) + Luck + Effects.Sum(userEffect => userEffect.Effect.Luck)) * (1 + (Loadout.Sum(stat => stat.LuckFactor) * 0.01) + Effects.Sum(userEffect => userEffect.Effect.LuckFactor) * 0.01));
        public int TotalAgility() => (int)Math.Floor((Loadout.Sum(stat => stat.Agility) + Agility + Effects.Sum(userEffect => userEffect.Effect.Agility)) * (1 + (Loadout.Sum(stat => stat.AgilityFactor) * 0.01) + Effects.Sum(userEffect => userEffect.Effect.AgilityFactor) * 0.01));
        public int TotalStrength() => (int)Math.Floor((Loadout.Sum(stat => stat.Strength) + Strength + Effects.Sum(userEffect => userEffect.Effect.Strength)) * (1 + (Loadout.Sum(stat => stat.StrengthFactor) * 0.01) + Effects.Sum(userEffect => userEffect.Effect.StrengthFactor) * 0.01));
        public int TotalConstitution() => (int)Math.Floor((Loadout.Sum(stat => stat.Constitution) + Constitution + Effects.Sum(userEffect => userEffect.Effect.Constitution)) * (1 + (Loadout.Sum(stat => stat.ConstitutionFactor) * 0.01) + Effects.Sum(userEffect => userEffect.Effect.ConstitutionFactor) * 0.01));
        public int TotalIntelligence() => (int)Math.Floor((Loadout.Sum(stat => stat.Intelligence) + Intelligence + Effects.Sum(userEffect => userEffect.Effect.Intelligence)) * (1 + (Loadout.Sum(stat => stat.IntelligenceFactor) * 0.01) + Effects.Sum(userEffect => userEffect.Effect.IntelligenceFactor) * 0.01));
        public int TotalDefense() => (int)Math.Floor((Loadout.Sum(stat => stat.Defense) + (int)Math.Floor(2.0 * TotalConstitution())) * (1 + Loadout.Sum(stat => stat.DefenseFactor) * 0.01));
        public int TotalDodge() => Loadout.Sum(stat => stat.Dodge) + TotalAgility() + Effects.Sum(userEffect => userEffect.Effect.Dodge);
        public int TotalHitrate() => (int)Math.Floor((Loadout.Sum(stat => stat.Hitrate) + TotalAgility() + Effects.Sum(userEffect => userEffect.Effect.Hitrate)) * (1 + (Loadout.Sum(stat => stat.HitRateFactor) * 0.01) + Effects.Sum(userEffect => userEffect.Effect.HitrateFactor) * 0.01));
        public int Pierce() => (int)Math.Floor((Loadout.Sum(stat => stat.Pierce) + (TotalAgility() / 2) + Effects.Sum(userEffect => userEffect.Effect.Pierce)) * (1 + (Loadout.Sum(stat => stat.PierceFactor) * 0.01) + Effects.Sum(userEffect => userEffect.Effect.PierceFactor) * 0.01));
        public int MaxAttack() => Loadout.Sum(stat => stat.MaxAttack) + Attack + Effects.Sum(userEffect => userEffect.Effect.Attack);
        public int MinAttack() => Loadout.Sum(stat => stat.MinAttack) + Attack + Effects.Sum(userEffect => userEffect.Effect.Attack);
        public int TotalResistance() => Loadout.Sum(stats => stats.Resistance);
        public int TotalAttackSpeed() => BaseAttackSpeed + Loadout.Sum(stat => stat.AttackSpeed) + TotalAgility() / 2;
        public double CooldownReduction() => ( ((21 * TotalIntelligence()) / (TotalIntelligence() + 12)) + Loadout.Sum(stat => stat.CooldownReduction) ) * 0.01;
        public int TotalHealthRegen() => BaseHealthRegen + Loadout.Sum(stats => stats.HealthRegen);
        public int TotalFlatHealthRegen() => (TotalStrength() / 2) + Loadout.Sum(stats => stats.FlatHealthRegen) + Effects.Sum(userEffect => userEffect.Effect.FlatHealthRegen);
        public int TotalEnergyRegen() => BaseEnergyRegen + Loadout.Sum(stats => stats.EnergyRegen) + Effects.Sum(userEffect => userEffect.Effect.EnergyRegen);
        public int TotalFlatEnergyRegen() => (TotalIntelligence() / 4) + Loadout.Sum(stats => stats.FlatEnergyRegen) + Effects.Sum(userEffect => userEffect.Effect.FlatEnergyRegen);


        public double BaseOpponentStealSuccessRate() => 0.75;
        public int BaseStealAmount() => (int)Math.Floor(3 * (Math.Sqrt(TotalAgility()) - Math.Sqrt(5)) + 1);
        public double BaseDetectionChance() => (Math.Sqrt(Math.Max((TotalIntelligence() - 5), 1)) * 0.08);
        public double BaseDetectionAvoidance() => Math.Sqrt((TotalAgility() + TotalLuck()) / 2.0) * 0.15;
        public bool HasWeaponType(Type type) => Loadout.HasWeaponType(type);
        public bool HasEnoughCreditsForAmount(int amount) => Credits - amount >= 0;
        public string NotEnoughCreditsForAmountResponse(int amount) => string.Format(DougMessages.NotEnoughCredits, amount, Credits);
        public bool HasEmptyInventory() => !InventoryItems.Any();
        public bool HasEnoughEnergyForCost(int cost) => Energy - cost >= 0;
        public bool IsDead() => Health <= 0;
        public bool IsAttackOnCooldown() => DateTime.UtcNow <= AttackCooldown;
        public bool IsSkillOnCooldown() => DateTime.UtcNow <= SkillCooldown;
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

        public void LoadItems(IEquipmentEffectFactory equipmentEffectFactory)
        {
            InventoryItems.ForEach(item => item.CreateItemEffects(equipmentEffectFactory));
            Loadout.CreateEffects(equipmentEffectFactory);
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
            return (int)Math.Ceiling((healthFromLevel + healthFromConstitution + healthFromEffects + Loadout.Sum(stats => stats.Health)) * (1 + Loadout.Sum(stat => stat.HealthFactor) * 0.01));
        }

        public int TotalEnergy()
        {
            var energyFromLevel = (int)Math.Floor(5.0 * Level + 20);
            var energyFromIntelligence = (int)Math.Floor(5.0 * TotalIntelligence() - 25);
            var energyFromEffects = Effects.Sum(userEffect => userEffect.Effect.Energy);
            return (int)Math.Ceiling((energyFromLevel + energyFromIntelligence + energyFromEffects + Loadout.Sum(stats => stats.Energy)) * (1 + Loadout.Sum(stat => stat.EnergyFactor) * 0.01));
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

        public double CriticalHitChance()
        {
            return (Math.Sqrt(TotalLuck()) * 0.04 + Loadout.Sum(stat => stat.CriticalHitChanceFactor) + Effects.Sum(userEffect => userEffect.Effect.CritChanceFactor)) * 0.01;
        }

        public double CriticalDamageFactor()
        {
            return Math.Min(1, (TotalLuck() * 0.02 + 1.9) + Loadout.Sum(stat => stat.CriticalDamageFactor) + Effects.Sum(userEffect => userEffect.Effect.CritDamageFactor));
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
            Attack attack = new PhysicalAttack(this, MinAttack(), MaxAttack(), TotalHitrate(), CriticalHitChance(), CriticalDamageFactor(), Pierce());

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

            var totalPierce = TotalDefense() * attack.Pierce * 0.011;
            var reducedDamage = attack.Damage - (int)Math.Ceiling(attack.Damage * TotalResistance() * 0.01 + TotalDefense() - totalPierce);
            reducedDamage = reducedDamage <= 0 ? 1 : reducedDamage;

            attack.Damage = reducedDamage;
            if (attack.Status == AttackStatus.Critical)
            {
                attack.Damage = (int)(Math.Ceiling(attack.Damage * attack.CriticalFactor));
            }

            Health -= attack.Damage;

            return attack;
        }

        private void ApplyMagicalDamage(int damage)
        {
            var reducedDamage = damage - (int)Math.Ceiling(damage * TotalResistance() * 0.01);
            Health -= reducedDamage;
        }

        public void RegenerateHealthAndEnergy()
        {
            Health += (int)Math.Ceiling(TotalHealth() * TotalHealthRegen() * 0.01) + TotalFlatHealthRegen();
            Energy += (int)Math.Ceiling(TotalEnergy() * TotalEnergyRegen() * 0.01) + TotalFlatEnergyRegen();
        }

        public void AddLotteryTicketsBasedOnLuck()
        {
            LotteryTickets = TotalLuck();
        }

        public int CalculateExperienceGainedFromMonster(Monster monster, int partyMemberCount)
        {
            var levelDifference = Level - monster.Level;
            var experienceMultiplier = (Math.Max(0, levelDifference * (-0.01) * levelDifference + 1));
            var monsterExperienceValue = (double)monster.ExperienceValue / partyMemberCount;
            return (int)Math.Floor(monsterExperienceValue * experienceMultiplier);
        }

        public void ReceiveExpFromMonster(Monster monster, int partyMemberCount)
        {
            Experience += CalculateExperienceGainedFromMonster(monster, partyMemberCount);
        }

        public void ResetStats()
        {
            Luck = 5;
            Agility = 5;
            Strength = 5;
            Constitution = 5;
            Intelligence = 5;
        }
    }
}
