namespace NewsByTheMood.Core.Settings
{
    public struct AccessLevels
    {
        public const string Admininistrator = "Admininistrator";
        public const string Editor = "Editor";
        public const string User = "User";
        public static string[] AllRoles => new[] { Admininistrator, Editor, User };
    }
}
