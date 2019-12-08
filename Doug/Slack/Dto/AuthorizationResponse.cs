namespace Doug.Slack.Dto
{
    public class AuthorizationResponse
    {
        public string AccessToken { get; set; }
        public string Scope { get; set; }
        public string TeamName { get; set; }
        public string TeamId { get; set; }
    }
}
