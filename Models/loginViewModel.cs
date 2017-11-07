using System.ComponentModel.DataAnnotations;

namespace greenSwash.Models
{
    public class loginViewModel
    {
        [Required]
        public string email {get; set;}
        [Required]
        public string password {get; set;}
    }
}