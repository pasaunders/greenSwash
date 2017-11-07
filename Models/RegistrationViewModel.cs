using System.ComponentModel.DataAnnotations;

namespace greenSwash.Models
{
    public class RegistrationViewModel
    {
        [Required]
        public string name {get; set;}
        [Required]
        [EmailAddress]
        public string email {get; set;}
        [Required]
        public string password {get; set;}
        [Required]
        [Compare("password", ErrorMessage="must match")]
        public string comparePassword {get; set;}
        [Required]
        public string description {get; set;}
    }
}