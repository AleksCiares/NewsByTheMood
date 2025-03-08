namespace NewsByTheMood.MVC.Options
{
    public class UserIconsOptions
    {
        public const string Position = "UserIcons";

        public required string CssFilePath { get; set; }
        public string[]? BaseCssClasses { get; set; }
        public required string CssClassRegex { get; set; }
    }
}
