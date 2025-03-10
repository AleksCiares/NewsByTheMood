using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    // Pagination model, used by the user for pagination (a very comprehensive description XD)
    public class PaginationModel
    {
        // Current page
        [Range(1, Int32.MaxValue)]
        public int Page { get; set; } = 1;
        // Number of displayed items on the page
        [Range(5, 20)]
        public int PageSize { get; set; } = 12;
    }
}
