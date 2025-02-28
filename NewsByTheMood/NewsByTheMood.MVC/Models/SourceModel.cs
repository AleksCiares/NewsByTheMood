using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    // Full information source model
    public class SourceModel
    {
        public required string Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name is too small or long (maximum is 100 characters)")]
        public required string Name { get; set; }

        [Required]
        [RegularExpression(@"^((http|https):\/\/)(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$",
            MatchTimeoutInMilliseconds = 500,
            ErrorMessage = "Url does not fit typical http or https protocol site links")]
        public required string Url { get; set; }

        [Required]
        [Range(10, Int32.MaxValue)]
        public required int SurveyPeriod { get; set; }

        [Required]
        public required bool IsRandomPeriod { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "ArticleListPath is too small or long (maximum is 250 characters)")]
        public required string ArticleListPath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "ArticleItemPath is too small or long (maximum is 250 characters)")]
        public required string ArticleItemPath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "ArticleUriPath is too small or long (maximum is 250 characters)")]
        public required string ArticleUriPath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "ArticleTitlePath is too small or long (maximum is 250 characters)")]
        public required string ArticleTitlePath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "ArticleBodyPath is too small or long (maximum is 250 characters)")]
        public required string ArticleBodyPath { get; set; }

        [StringLength(250, ErrorMessage = "ArticlePdatePath is too long (maximum is 250 characters)")]
        public string? ArticlePdatePath { get; set; }

        [StringLength(250, ErrorMessage = "ArticleTagPath is too  long (maximum is 250 characters)")]
        public string? ArticleTagPath { get; set; }

        [Required]
        [Range(0, Int64.MaxValue)]
        public required string TopicId { get; set; }

        [Range(0, Int64.MaxValue)]
        public int ArticleAmmount { get; set; } = 0;
    }
}
