using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string CoffeeRemindJobId { get; set; }
        public int FatCounter { get; set; }
        public bool IsCoffee { get; set; }
    }
}
