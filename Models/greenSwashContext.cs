using Microsoft.EntityFrameworkCore;

namespace greenSwash.Models
{
    public class greenSwashContext : DbContext
    {
        public greenSwashContext(DbContextOptions<greenSwashContext> options) : base(options) {}
        public DbSet<Users> users {get; set;}
        public DbSet<Connections> connections {get; set;}
    }
}