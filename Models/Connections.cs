using System.ComponentModel.DataAnnotations;

namespace greenSwash.Models
{
    public class Connections
    {
        [Key]
        public int connectionId { get; set; }
 
        public int connectorId { get; set; }
        public Users connector { get; set; }
 
        public int connectedId { get; set; }
        public Users connected { get; set; }
        public int confirmed { get; set;}
    }
}
