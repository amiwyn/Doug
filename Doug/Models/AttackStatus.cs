using System;

namespace Doug.Models
{
    public enum AttackStatus
    {
        Normal,
        Critical,
        Missed
    }

    public static class AttackStatusExtension
    {
        public static string ToMessage(this AttackStatus status, string user, string mention, int damage)
        {
            switch (status)
            {
                case AttackStatus.Normal:
                    return string.Format(DougMessages.UserAttackedTarget, user, mention, damage);
                case AttackStatus.Critical:
                    return string.Format(DougMessages.CriticalHit, user, mention, damage);
                case AttackStatus.Missed:
                    return string.Format(DougMessages.Missed, user, mention);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
