using System.ComponentModel.DataAnnotations;

namespace dealer.mobiva.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email zorunludur.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Message { get; set; }
    }
}