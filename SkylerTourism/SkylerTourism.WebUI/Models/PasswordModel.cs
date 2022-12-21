using System.ComponentModel.DataAnnotations;

namespace SkylerTourism.WebUI.Models
{  
        public class PasswordModel
        {
            public string UserId { get; set; }
            [Required(ErrorMessage = "Mnadatory")]
            public string Password { get; set; }
            [Compare("Password", ErrorMessage = "Passwords don't match!")]
            public string RePassword { get; set; }
        }
    }

