using Discord;

namespace Guide
{
    public static class Constants
    {
        public const string ConfigKeyToken = "DiscordToken";
        public const string JsonDirectory = "data";
        public const string LanguageFile = "Lang/english.json";

#if DEBUG
        public const ulong ScoreboardId = 513467883825922063;
        public const ulong TutorialGuildId = 513451922586468353;
        public const ulong WaitingRoomId = 560835975052263434;
        public const ulong GeneralId = 538334113501937667;
        public const ulong MemberRoleId = 560836164647649329;
        public const ulong HelperRoleId = 560836219953479681;
#else
        public const ulong ScoreboardId = 560870504144306188;
        public const ulong TutorialGuildId = 377879473158356992;
        public const ulong WaitingRoomId = 411864218548043785;
        public const ulong GeneralId = 377879473644765185;
        public const ulong MemberRoleId = 411865173318696961;
        public const ulong HelperRoleId = 480033369812500491;
#endif

        public const string PKWelcomeTitle = "WELCOME_EMBED_TITLE";
        public const string PKWhileYouWait = "WELCOME_EMBED_WHILE_YOU_WAIT";
        public const string PKFunServerFact = "FUN_SERVER_FACT";
        public const string PKUserJoinedTitle = "USER_JOINED_TITLE";
        public static readonly Color PrimaryColor = new Color(41, 182, 246);
    }
}
