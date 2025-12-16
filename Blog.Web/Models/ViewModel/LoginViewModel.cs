using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(8,ErrorMessage = "Password has to be at least 8 characters")]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
