using Microsoft.EntityFrameworkCore;
using WebAPICore.Model;

namespace webapi.Data.Entities
{
    public class ReactJSDemoContext: DbContext
    {
        public ReactJSDemoContext(DbContextOptions<ReactJSDemoContext> context) : base(context)
        {
        }
        public DbSet<SuperVillain> SuperVillain { get; set; }
        
        public DbSet<User> Users { get; set; }
    }
}
