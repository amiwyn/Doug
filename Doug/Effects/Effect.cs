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
    }
}
