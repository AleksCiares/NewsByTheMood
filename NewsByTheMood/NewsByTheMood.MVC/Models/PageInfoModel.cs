using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    // Page information model (used to calculate pagination on the server side)
    public class PageInfoModel
    {
        // Current page
        [Range(1, Int32.MaxValue)]
        public int Page { get; set; } = 1;
        // Number of displayed items on the page
        [Range(5, 20)]
        public int PageSize { get; set; } = 10;
        // Total number of items
        [Range(0, Int32.MaxValue)]
        public int TotalItems { get; set; }
        // Number of pages required to display all the elements
        [Range(0, Int32.MaxValue)]
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
