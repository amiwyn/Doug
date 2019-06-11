namespace Doug
{
    public static class DougMessages
    {
        public const string CreditEmoji = ":ruppurple:";
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
        public const string Balance = "You have {0} " + CreditEmoji;
        public const string UserGaveCredits = "{0} gave {1} " + CreditEmoji + " to {2}";
        public const string StatsOf = "Stats of {0}";
        public const string CreditStats = CreditEmoji + " Money : {0}";
        public const string UserIdStats = ":robot_face: User ID : {0}";
        public const string SlursAddedStats = ":see_no_evil: Slurs added : {0}";
        public const string WonGamble = "{0} flipped a coin and won {1} " + CreditEmoji;
        public const string LostGamble = "{0} flipped a coin and lost {1} " + CreditEmoji;
        public const string ChallengeSent = "{0} challenged {1} to a coinflip for {2} " + CreditEmoji;
        public const string GambleChallenge = "{0} won {1} " + CreditEmoji + " in a coin flip against {2}";
        public const string GambleDeclined = "{0} declined {1}'s request, what a loser.";
        public const string InsufficientCredits = "{0} need to have at least {1} " + CreditEmoji;
        public const string GambleChallengeTip = "You were challenged to a coin flip, type in `/gamblechallenge accept` to accept or `/gamblechallenge decline` to decline";


        public const string DougError = "Beep boop, it's not working : {0}";
        public const string NotAnAdmin = "You are not an admin.";
        public const string InvalidArgumentCount = "You provided an invalid argument count.";
        public const string InvalidAmount = "InvalidAmount";
        public const string NotEnoughCredits = "You need {0} " + CreditEmoji + " to do this and you have {1} " + CreditEmoji;
        public const string SlursAreClean = "There is nothing to cleanup.";
        public const string YouAreTooRich = "You are too rich for this.";
        public const string AlreadyChallenged = "This user is already challenged.";
        public const string NotChallenged = "You are not challenged.";
    }
}
