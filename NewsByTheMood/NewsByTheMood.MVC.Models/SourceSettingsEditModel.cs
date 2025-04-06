using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class SourceSettingsEditModel
    {
        [Required]
        public required SourceSettingsModel Source { get; set; }

        [Required]
        public required List<SelectListItem> Topics { get; set; }

        [Required]
        [Range(0, Int64.MaxValue)]
        public required int RelatedArticlesCount { get; set; }
    }
}
