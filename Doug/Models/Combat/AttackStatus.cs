using System;

namespace Doug.Models.Combat
{
    public enum AttackStatus
    {
        Normal,
        Critical,
        Missed,
        Invincible
    }

    public static class AttackStatusExtension
    {
        public static string ToMessage(this AttackStatus status, string attacker, string target, int damage)
        {
            switch (status)
            {
                case AttackStatus.Normal:
                    return string.Format(DougMessages.UserAttackedTarget, attacker, target, damage);
                case AttackStatus.Critical:
                    return string.Format(DougMessages.CriticalHit, attacker, target, damage);
                case AttackStatus.Missed:
                    return string.Format(DougMessages.Missed, attacker, target);
                case AttackStatus.Invincible:
                    return string.Format(DougMessages.UserIsInvincible, target);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
