using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class TopicSettingsModel
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public required string Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name is too small or long (maximum is 100 characters)")]
        [Remote(action: "NameIsAvailable", controller: "Topics",
            areaName: "Settings", HttpMethod = "Post", AdditionalFields = nameof(Id), ErrorMessage = "A topic with the same name already exists")]
        public required string Name { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Icon Css Class is too small or long (maximum is 100 characters)")]
        public string? IconCssClass { get; set; }
    }
}
