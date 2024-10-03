using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Domain;

namespace MyInvestAPI.Data
{
    public class MyInvestContext : IdentityDbContext<User>
    {
        public MyInvestContext(DbContextOptions<MyInvestContext> options) : base(options) 
        { }

        public DbSet<Purse>? Purses { get; set; }
        public DbSet<Active>? Actives { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
