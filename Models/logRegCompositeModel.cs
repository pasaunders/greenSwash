using System.ComponentModel.DataAnnotations;

namespace greenSwash.Models
{
    public class logRegCompositeModel
    {
        public RegistrationViewModel registration {get; set;}
        public loginViewModel login {get; set;}
    }
}