using Microsoft.EntityFrameworkCore;
using ProjInv.Domain.Entities;
using ProjInv.Infrastructure.Data.Configurations;

namespace ProjInv.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {}

        public DbSet<Investidor> Investidores { get; set; }
        public DbSet<Investimento> Investimentos { get; set; }
        public DbSet<Retirada> Retiradas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InvestidorConfiguration());
            modelBuilder.ApplyConfiguration(new InvestimentoConfiguration());
            modelBuilder.ApplyConfiguration(new RetiradaConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
