using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class ArticleSettingsModel
    {
        [StringLength(100, MinimumLength = 1)]
        public string Id { get; set; } = "0";

        [Required]
        [RegularExpression(@"^((http|https):\/\/)(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$",
            MatchTimeoutInMilliseconds = 500,
            ErrorMessage = "Url does not fit typical http or https protocol site links")]
        [Remote(action: "UrlIsAvailable", controller: "Articles",
            areaName: "Settings", HttpMethod = "Post", AdditionalFields = nameof(Id), ErrorMessage = "A article with the same url already exists")]
        public string Url { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Title is too small or long (maximum is 150 characters)")]
        public string Title { get; set; }

        [Display(Name = "Preview image url")]
        [RegularExpression(@"^((http|https):\/\/)(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$",
            MatchTimeoutInMilliseconds = 500,
            ErrorMessage = "Url does not fit typical http or https protocol site links")]
        public string? PreviewImgUrl { get; set; }

        public string? Body { get; set; }

        [Display(Name = "Publish date")]
        [DataType(DataType.DateTime)]
        public DateTime? PublishDate { get; set; }

        [Required]
        [Range(0, 10)]
        public short Positivity { get; set; }

        [Required]
        [Range(0, Int64.MaxValue)]
        public int Rating { get; set; }

        [Display(Name = "Activity")]
        [Required]
        public bool IsActive { get; set; }

        [Display(Name = "Failed loaded")]
        [Required]
        public bool FailedLoaded { get; set; }

        [Display(Name = "Source")]
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string SourceId { get; set; }

        public List<string>? ArticleTags { get; set; } = new List<string>();

        public List<SelectListItem>? Sources { get; set; }
        
        public List<SelectListItem>? Tags { get; set; }
    }
}
