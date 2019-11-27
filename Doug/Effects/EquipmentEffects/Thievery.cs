using Doug.Models.Combat;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;
using System;

namespace Doug.Effects.EquipmentEffects
{
    public class Thievery : EquipmentEffect
    {
        private readonly ICreditsRepository _creditsRepository;
        private readonly ISlackWebApi _slack;
        public const string EffectId = "thievery";

        public Thievery(ICreditsRepository creditsRepository, ISlackWebApi slack)
        {
            _creditsRepository = creditsRepository;
            _slack = slack;
            Id = EffectId;
            Name = "Thievery";
        }

        public override int OnCriticalHit(User attacker, ICombatable target, int damage, string channel)
        {
            var min = (int)Math.Floor(damage * 0.01);
            var max = (int)Math.Ceiling(damage * 0.05);
            var amount = Math.Max(1, new Random().Next(min, max));
            var message = string.Format(DougMessages.GainedCredit, amount);

            _creditsRepository.AddCredits(attacker.Id, amount);
            _slack.SendEphemeralMessage(message, attacker.Id, channel);

            return base.OnCriticalHit(attacker, target, damage, channel);
        }
    }
}
