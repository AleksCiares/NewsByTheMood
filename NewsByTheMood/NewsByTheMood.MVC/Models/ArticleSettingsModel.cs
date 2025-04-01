using Microsoft.AspNetCore.Mvc.Rendering;
using NewsByTheMood.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class ArticleSettingsModel
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public required string Id { get; set; }

        [Required]
        [RegularExpression(@"^((http|https):\/\/)(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$",
            MatchTimeoutInMilliseconds = 500,
            ErrorMessage = "Url does not fit typical http or https protocol site links")]
        public required string Url { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Title is too small or long (maximum is 150 characters)")]
        public required string Title { get; set; }

        [RegularExpression(@"^((http|https):\/\/)(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$",
            MatchTimeoutInMilliseconds = 500,
            ErrorMessage = "Url does not fit typical http or https protocol site links")]
        public string? PreviewImgUrl { get; set; }

        public string? Body { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? PublishDate { get; set; }

        [Required]
        [Range(0, 10)]
        public required short Positivity { get; set; }

        [Required]
        [Range(0, Int64.MaxValue)]
        public required int Rating { get; set; }

        [Required]
        public required bool IsActive { get; set; }

        [Required]
        [Range(0, Int64.MaxValue)]
        public required string SourceId { get; set; }

        public required Tag[] Tags { get; set; }
    }
}
