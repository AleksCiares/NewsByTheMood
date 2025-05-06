using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    // Display source model
    public class SourceSettingsModel
    {
        [StringLength(100, MinimumLength = 1)]
        public string Id { get; set; } = "0";

        [Required]
        [Display(Name = "Automatic news collection")]
        public bool IsActive { get; set; } // is responsible for automatic news collection

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name is too small or long (maximum is 100 characters)")]
        [Remote(action: "NameIsAvailable", controller: "Sources",
            areaName: "Settings", HttpMethod = "Post", AdditionalFields = nameof(Id), ErrorMessage = "A source with the same name already exists")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^((http|https):\/\/)(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$",
            MatchTimeoutInMilliseconds = 500,
            ErrorMessage = "Url does not fit typical http or https protocol site links")]
        public string Url { get; set; }

        [Required]
        [Range(10, 267840)]
        [Display(Name = "Survey period")]
        public int SurveyPeriod { get; set; }

        [Required]
        [Display(Name = "Use random period")]
        public bool IsRandomPeriod { get; set; }

        [Required]
        [Display(Name = "Has dynamic page")]
        public bool HasDynamicPage { get; set; }

        [Required]
        [Display(Name = "Accept insecure certs")]
        public bool AcceptInsecureCerts { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, 
            ErrorMessage = "PageElementLoaded is too small or long (maximum is 100 characters)")]
        [Display(Name = "Page element loaded")]
        public string PageElementLoaded { get; set; }

        [Required]
        [Range(0, 600)]
        [Display(Name = "Page load timeout")]
        public int PageLoadTimeout { get; set; }

        [Required]
        [Range(0, 300)]
        [Display(Name = "Element load timeout")]
        public int ElementLoadTimeout { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, 
            ErrorMessage = "ArticleCollectionsPath is too small or long (maximum is 250 characters)")]
        [Display(Name = "Article collections path")]
        public string ArticleCollectionsPath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, 
            ErrorMessage = "ArticleItemPath is too small or long (maximum is 250 characters)")]
        [Display(Name = "Article item path")]
        public string ArticleItemPath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, 
            ErrorMessage = "ArticleUriPath is too small or long (maximum is 250 characters)")]
        [Display(Name = "Article url path")]
        public string ArticleUrlPath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, 
            ErrorMessage = "ArticleTitlePath is too small or long (maximum is 250 characters)")]
        [Display(Name = "Article title path")]
        public string ArticleTitlePath { get; set; }

        [StringLength(250, MinimumLength = 1, 
            ErrorMessage = "ArticlePreviewImgPath is too small or long (maximum is 250 characters)")]
        [Display(Name = "Article preview image path")]
        public string? ArticlePreviewImgPath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, 
            ErrorMessage = "ArticleBodyCollectionsPath is too small or long (maximum is 250 characters)")]
        [Display(Name = "Article body collections path")]
        public string ArticleBodyCollectionsPath { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1, 
            ErrorMessage = "ArticleBodyItemPath is too small or long (maximum is 250 characters)")]
        [Display(Name = "Article body item path")]
        public string ArticleBodyItemPath { get; set; }

        [StringLength(250, ErrorMessage = "ArticlePdatePath is too long (maximum is 250 characters)")]
        [Display(Name = "Article publish date path")]
        public string? ArticlePdatePath { get; set; }

        [StringLength(250, ErrorMessage = "ArticleTagPath is too  long (maximum is 250 characters)")]
        [Display(Name = "Article tag path")]
        public string? ArticleTagPath { get; set; }

        public int? RelatedArticles { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        [Display(Name = "Topic")]
        public string TopicId { get; set; }

        public List<SelectListItem>? Topics { get; set; }
    }
}
