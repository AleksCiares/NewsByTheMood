using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NewsByTheMood.MVC.Models
{
    
    public class TopicModel
    {
        [Required]
        [StringLength(100)]
        public string TopicName {  get; set; }
    }
}
