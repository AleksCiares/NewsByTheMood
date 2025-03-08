using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    // Display source model
    public class SourceModel
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
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
        [Range(10, 267840)]
        public required int SurveyPeriod { get; set; }

        [Required]
        public required bool IsRandomPeriod { get; set; }

        [Required]
        public required bool HasDynamicPage { get; set; }

        [Required]
        public required bool AcceptInsecureCerts { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "PageElementLoaded is too small or long (maximum is 100 characters)")]
        public required string PageElementLoaded { get; set; }

        [Required]
        [Range(0, 600)]
        public required int PageLoadTimeout { get; set; }

        [Required]
        [Range(0, 300)]
        public required int ElementLoadTimeout { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "ArticleCollectionsPath is too small or long (maximum is 250 characters)")]
        public required string ArticleCollectionsPath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "ArticleItemPath is too small or long (maximum is 250 characters)")]
        public required string ArticleItemPath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "ArticleUriPath is too small or long (maximum is 250 characters)")]
        public required string ArticleUrlPath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "ArticleTitlePath is too small or long (maximum is 250 characters)")]
        public required string ArticleTitlePath { get; set; }

        [StringLength(250, MinimumLength = 1, ErrorMessage = "ArticlePreviewImgPath is too small or long (maximum is 250 characters)")]
        public string? ArticlePreviewImgPath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "ArticleBodyCollectionsPath is too small or long (maximum is 250 characters)")]
        public required string ArticleBodyCollectionsPath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "ArticleBodyItemPath is too small or long (maximum is 250 characters)")]
        public required string ArticleBodyItemPath { get; set; }

        [StringLength(250, ErrorMessage = "ArticlePdatePath is too long (maximum is 250 characters)")]
        public string? ArticlePdatePath { get; set; }

        [StringLength(250, ErrorMessage = "ArticleTagPath is too  long (maximum is 250 characters)")]
        public string? ArticleTagPath { get; set; }

        [Required]
        [Range(0, Int64.MaxValue)]
        public required string TopicId { get; set; }
    }
}
