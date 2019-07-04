namespace Doug
{
    public static class DougMessages
    {
        public const string CoffeeParrotEmoji = ":coffeeparrot:";
        public const string CreditEmoji = ":rupee:";
        public const string UpVote = "+1";
        public const string Downvote = "-1";
        public const string Top5 = "Leaderboard";

        public const string JoinedCoffee = "{0} joined the coffee break.";
        public const string KickedCoffee = "{0} was kicked from the coffee break.";
        public const string SkippedCoffee = "{0} will skip the coffee break.";
        public const string Remind = "*{0}/{1}* - {2}";
        public const string Remind69 = "*{0}/{1}* - YEAH - {2}";
        public const string CoffeeStart = "Alright, let's do this. <!here> GO!";
        public const string BackToWork = "<!here> Go back to work, ya bunch o' lazy dogs!";
        public const string GainedCredit = "You gained {0} " + CreditEmoji;
        public const string SlursCleaned = "The following slurs have been cleaned up";
        public const string SlurCreatedBy = "{0} created that slur.";
        public const string Balance = "You have {0} " + CreditEmoji;
        public const string UserGaveCredits = "{0} gave {1} " + CreditEmoji + " to {2}";
        public const string StatsOf = "Profile and stats of {0}";
        public const string CreditStats = CreditEmoji + " {0}";
        public const string UserIdStats = ":robot_face: User ID : {0}";
        public const string LevelStats = "Level {0}";
        public const string ExperienceStats = "{0:0.##}%";
        public const string HealthStats = "Health Points :";
        public const string EnergyStats = "Energy Points :";
        public const string CharismaStats = ":information_desk_person: Charisma : {0}";
        public const string AgilityStats = ":runner: Agility : {0}";
        public const string LuckStats = ":four_leaf_clover: Luck : {0}";
        public const string ConstitutionStats = ":heart: Constitution : {0}";
        public const string StaminaStats = ":male_mage: Stamina : {0}";
        public const string FreeStatPoints = "{0} stats points left";
        public const string AddStatPoint = ":heavy_plus_sign:";
        public const string WonGamble = "{0} flipped a coin and won {1} " + CreditEmoji;
        public const string LostGamble = "{0} flipped a coin and lost {1} " + CreditEmoji;
        public const string ChallengeSent = "{0} challenged {1} to a coinflip for {2} " + CreditEmoji;
        public const string GambleChallenge = "{0} won {1} " + CreditEmoji + " in a coin flip against {2}";
        public const string GambleDeclined = "{0} declined {1}'s request, what a loser.";
        public const string InsufficientCredits = "{0} need to have at least {1} " + CreditEmoji;
        public const string GambleChallengeTip = "You were challenged to a coin flip, type in `/gamblechallenge accept` to accept or `/gamblechallenge decline` to decline";
        public const string UserFlamedYou = "{0} flamed you.";
        public const string YouIdiot = "You idiot.";
        public const string StealCredits = "You stole {0} " + CreditEmoji + " from {1}! You have {2} energy left.";
        public const string StealFail = "{0} got caught trying to steal from {1}!";
        public const string RecoverItem = "You used *{0}* and recovered {1} {2}.";
        public const string UserGaveItem = "{0} gave *{1}* to {2}.";
        public const string ConsumedItem = "You consumed the item.";
        public const string LevelUp = ":confetti_ball: {0} is now level *{1}*!";
        public const string GainedExp = "You gained {0} experience points.";
        public const string EquippedItem = "You equipped *{0}*.";
        public const string ShopSpeech = "*Oi!* Welcome to my shop! I sell various stuff from all around the continent.\n\n *Come in, have a look:*";
        public const string BuyFor = "Buy for {0} " + CreditEmoji;
        public const string UserDied = "Oh dear, {0} died!";
        public const string Use = "Use";
        public const string Equip = "Equip";
        public const string UnEquip = "Unequip";
        public const string Sell = "Sell for {0} " + CreditEmoji;
        public const string Quantity = "*Quantity:* {0} ";
        public const string SoldItem = "You sold *{0}* for {1} " + CreditEmoji;
        public const string Inventory = "Inventory";
        public const string Equipment = "Equipment";
        public const string Info = "Info";
        public const string UnequippedItem = "You unequipped *{0}*.";
        public const string Target = "Target ...";
        public const string Give = "Give ...";
        public const string SelectTarget = "Select a target";
        public const string SelectTargetText = "Please select a user to target with this action.";
        public const string UserAttackedTarget = "{0} dealt {2} damage to {1}!";
        public const string UsedItemOnTarget = "{0} used a *{1}* on {2}";

        public const string DougError = "Beep boop, it's not working : {0}";
        public const string NotAnAdmin = "You are not an admin.";
        public const string InvalidArgumentCount = "You provided an invalid argument count.";
        public const string InvalidAmount = "Invalid amount";
        public const string NotEnoughCredits = "You need {0} " + CreditEmoji + " to do this and you have {1} " + CreditEmoji;
        public const string SlursAreClean = "There is nothing to cleanup.";
        public const string AlreadyChallenged = "This user is already challenged.";
        public const string NotChallenged = "You are not challenged.";
        public const string SlurAlreadyExists = "That slur already exists.";
        public const string NotEnoughEnergy = "You don't have enough energy.";
        public const string TargetNoMoney = "Your target don't have enough " + CreditEmoji;
        public const string NoItemInSlot = "There is no item in slot {0}.";
        public const string ItemCantBeUsed = "This item cannot be used";
        public const string InvalidUserArgument = "You must pass a valid user in arguments";
        public const string ItemNotEquipAble = "This item is not equipable.";
        public const string NoMoreStatsPoints = "No more available stats points.";
        public const string NoEquipmentInSlot = "No equipment in slot {0}";

        public const string EmptyInventory = "Oops, yer loot seems empty, buy more at th' shop now.";
    }
}

