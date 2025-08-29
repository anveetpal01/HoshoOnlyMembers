using Microsoft.EntityFrameworkCore;
using OnlyMembers.Models;

namespace OnlyMembers.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<PointTransaction> PointTransactions { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
    }
}
