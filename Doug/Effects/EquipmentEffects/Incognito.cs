namespace Doug.Effects.EquipmentEffects
{
    public class Incognito : EquipmentEffect
    {
        public const string EffectId = "incognito";

        public Incognito()
        {
            Id = EffectId;
            Name = "Incognito";
        }

        public override string OnMention(string mention)
        {
            return ":sunglasses:";
        }
    }
}
