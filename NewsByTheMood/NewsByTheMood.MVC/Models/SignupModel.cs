using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NewsByTheMood.MVC.Models
{
    // User registration model
    public class SignupModel
    {
        [Required]
        [StringLength(39, MinimumLength = 6, ErrorMessage = "Username is too small or long (maximum is 39 characters)")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username may only contain letters and numbers")]
        //[Remote(action:"CheckUserName", controller: "Signup", HttpMethod = "Post", ErrorMessage = "Username is not available")]
        public required string Username { get; set; }

        [Required]
        [RegularExpression(@"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$", 
            ErrorMessage = "Email is invalid or already taken")]
        //[Remote(action: "CheckEmail", controller: "Signup", HttpMethod = "Post", ErrorMessage = "Email is invalid or already taken")]
        public required string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", 
            ErrorMessage = "Password should be at least 8 characters and contains capital lowercase letters numbers and special characters")]
        public required string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords dont match")]
        public required string PasswordConfirmation { get; set; }
    }
}
