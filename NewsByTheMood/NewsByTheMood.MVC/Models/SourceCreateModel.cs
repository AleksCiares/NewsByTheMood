using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewsByTheMood.MVC.Models
{
    public class SourceCreateModel
    {
        public SourceModel? Source { get; set; }
        public List<SelectListItem>? Topics { get; set; }
    }
}
