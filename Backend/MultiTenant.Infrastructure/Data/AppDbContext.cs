using Microsoft.EntityFrameworkCore;
using MultiTenant.Domain.Entities;
using MultiTenant.Infrastructure.Tenant;

namespace MultiTenant.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantProvider tenantProvider)
        : base(options) => _tenantProvider = tenantProvider;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configuration from separate files if needed
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = _tenantProvider.GetConnectionString();
            optionsBuilder.UseSqlServer(conn);
        }
    }
}

