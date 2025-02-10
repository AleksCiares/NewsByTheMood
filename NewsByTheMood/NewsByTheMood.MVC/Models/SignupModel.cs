using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NewsByTheMood.MVC.Models
{
    // User registration model
    public class SignupModel
    {
        [Required]
        [StringLength(30, MinimumLength = 6)]
        [RegularExpression("^[a-zA-Z0-9]+$")]
        [Remote(action:"CheckUserName", controller:"Singup")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^([^\.][\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$")]
        [Remote(action: "CheckEmail", controller: "Singup")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{8,25}$")]
        public  string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password dont match")]
        public  string PasswordConfirmation { get; set; }
    }
}
