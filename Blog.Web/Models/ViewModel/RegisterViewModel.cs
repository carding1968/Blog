using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password has to be at least 8 characters")]
        public string Password { get; set; }
    }
}
