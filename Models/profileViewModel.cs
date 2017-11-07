using System.Collections.Generic;

namespace greenSwash.Models
{
    public class profileViewModel
    {
        public string name {get; set;}
        public string description {get; set;}
        public List<Users> network {get; set;}
        public List<Connections> invitations {get; set;}
    }
}