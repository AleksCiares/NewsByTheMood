﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class ArticleSettingsEditModel
    {
        [Required]
        public required ArticleSettingsModel Article { get; set; }

        [Required]
        public required List<SelectListItem> Tags { get; set; }

        [Required]
        public required List<SelectListItem> Sources { get; set; }
    }
}
