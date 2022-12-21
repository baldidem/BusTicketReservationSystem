using System.ComponentModel.DataAnnotations;

namespace SkylerTourism.WebUI.Models
{
    public class RegisterModel
    {

        [Required(ErrorMessage = "Mandatory.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Mandatory.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Mandatory.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mandatory.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mandatory.")]
        //[DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Mandatory.")]
        //[DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Same password must be written!")]
        public string RePassword { get; set; }
    }
}
