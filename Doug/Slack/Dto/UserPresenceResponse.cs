namespace Doug.Slack.Dto
{
    public class UserPresenceResponse
    {
        public string Presence { get; set; }

        public bool IsPresent()
        {
            return Presence == "active";
        }
    }
}
