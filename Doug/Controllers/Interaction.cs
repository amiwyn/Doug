using System;
using System.Linq;
using Doug.Menus;

namespace Doug.Controllers
{
    public class Interaction
    {
        public string UserId { get; set; }
        public string ChannelId { get; set; }
        public string Action { get; set; }
        public string Value { get; set; }
        public string BlockId { get; set; }
        public string Timestamp { get; set; }
        public string ResponseUrl { get; set; }

        public Actions GetAction()
        {
            var actionString = Action;
            if (Action.Contains(":"))
            {
                actionString = Action.Split(":").First();
            }

            Enum.TryParse(actionString, out Actions action);
            return action;
        }

        public string GetValueFromAction()
        {
            return Action.Split(":").Last();
        }

        public T GetActionFromValue<T>() where T : struct
        {
            Enum.TryParse(Value.Split(":").First(), out T action);
            return action;
        }
    }
}
