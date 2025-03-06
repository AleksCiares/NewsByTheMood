using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class SourceEditModel
    {
        [Required]
        public required SourceModel Source { get; set; }

        public List<SelectListItem>? Topics { get; set; }

        public ArticlePreviewModel[]? RelatedArticles { get; set; }
    }
}
