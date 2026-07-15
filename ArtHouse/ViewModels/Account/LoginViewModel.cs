using System.ComponentModel.DataAnnotations;

namespace ArtHouse.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username or Email is required")]
        public string UserNameOrEmail { get; set; } = string.Empty;


        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;


        public bool RememberMe { get; set; }
    }
}