using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Domain;

namespace MyInvestAPI.Data
{
    public class MyInvestContext : IdentityDbContext
    {
        public MyInvestContext(DbContextOptions<MyInvestContext> options) : base(options) 
        { }

        public DbSet<User>? Users { get; set; }
        public DbSet<Purse>? Purses { get; set; }
        public DbSet<Active>? Actives { get; set; }
    }
}
