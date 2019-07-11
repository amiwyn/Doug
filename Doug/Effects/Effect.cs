using Doug.Models;

namespace Doug.Effects
{
    public abstract class Effect
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Rank Rank { get; set; }
        public string Icon { get; set; }

        public int Luck { get; set; }
        public int Agility { get; set; }
        public int Strength { get; set; }
        public int Constitution { get; set; }
        public int Stamina { get; set; }

        public virtual bool IsBuff()
        {
            return true;
        }

        /// <summary>
        /// Event raised when the user get kicked. Return false to prevent the holder from being kicked.
        /// </summary>
        /// <returns></returns>
        public virtual bool OnKick(User kicker, string channel)
        {
            return true;
        }

        public virtual string OnGettingFlamed(Command command, string slur)
        {
            return slur;
        }
    }
}
