using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class SourceSettingsCreateModel
    {
        public SourceSettingsModel? Source { get; set; }

        [Required]
        public required List<SelectListItem> Topics { get; set; }
    }
}
