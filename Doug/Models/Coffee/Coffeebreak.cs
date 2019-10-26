using System;

namespace Doug.Models.Coffee
{
    public class CoffeeBreak
    {
        public int Id { get; set; }
        public string BotToken { get; set; }
        public string UserToken { get; set; }
        public string CoffeeRemindJobId { get; set; }
        public int FatCounter { get; set; }
        public bool IsCoffeeBreak { get; set; }
        public DateTime LastCoffee { get; set; }
    }
}
