using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace greenSwash.Models
{
    public class Users
    {
        public Users()
        {
            connector = new List<Connections>();
            connected = new List<Connections>();

        }
        public int usersId {get; set;}
        public string name {get; set;}
        public string email {get; set;}
        public string password {get; set;}
        public string description {get; set;}

        [InverseProperty("connected")]
        public List<Connections> connector {get; set;}

        [InverseProperty("connector")]
        public List<Connections> connected {get; set;}

    }
}