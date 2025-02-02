using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class PaginationModel
    {
        [Range(1, Int32.MaxValue)]
        public int PageNumber { get; set; } = 1;
        [Range(5, 20)]
        public int PageSize { get; set; } = 10;
    }
}
