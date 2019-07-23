using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter Your email")]
        public string Email { get; set; }
         
        [Required(ErrorMessage = "Please enter Your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}