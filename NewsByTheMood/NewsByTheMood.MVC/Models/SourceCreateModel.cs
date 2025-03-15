using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class SourceCreateModel
    {
        public SourceModel? Source { get; set; }

        [Required]
        public required List<SelectListItem> Topics { get; set; }
    }
}
