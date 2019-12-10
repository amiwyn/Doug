namespace Doug.Slack.Dto
{
    public class IdentityResponse
    {
        public bool Ok { get; set; }
        public UserIdentity User { get; set; }
    }

    public class UserIdentity
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string image_48 { get; set; }
    }
}
