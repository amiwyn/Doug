namespace Doug.Slack.Dto
{
    public class UserInfoResponse
    {
        public bool Ok { get; set; }
        public UserInfo User { get; set; }
    }

    public class UserInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBot { get; set; }
    }
}
