namespace Doug
{
    public static class DougMessages
    {
        public const string CreditEmoji = ":rupee:";
        public const string UpVote = "+1";
        public const string Downvote = "-1";

        public const string JoinedCoffee = "{0} joined the coffee break.";
        public const string KickedCoffee = "{0} was kicked from the coffee break.";
        public const string SkippedCoffee = "{0} will skip the coffee break.";
        public const string Remind = "*{0}/{1}* - {2}";
        public const string CoffeeStart = "Alright, let's do this. <!here> GO!";
        public const string BackToWork = "<!here> Go back to work, ya bunch o' lazy dogs!";
        public const string GainedCredit = "You gained {0} " + CreditEmoji;
        public const string SlurAdded = "The slur was added.";
        public const string SlursCleaned = "The following slurs have been cleaned up";
        public const string SlurCreatedBy = "{0} created that slur.";

        public const string DougError = "Beep boop, it's not working : {0}";
        public const string NotAnAdmin = "You are not an admin.";
        public const string InvalidArgumentCount = "You provided an invalid argument count.";
        public const string InvalidAmount = "InvalidAmount";
        public const string NotEnoughCredits = "You need {0} " + CreditEmoji + " to do this and you have {1} " + CreditEmoji;
        public const string SlursAreClean = "There is nothing to cleanup.";
    }
}
