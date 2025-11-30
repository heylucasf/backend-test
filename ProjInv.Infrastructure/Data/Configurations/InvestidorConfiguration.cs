using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjInv.Domain.Entities;

namespace ProjInv.Infrastructure.Data.Configurations
{
    public class InvestidorConfiguration : IEntityTypeConfiguration<Investidor>
    {
        public void Configure(EntityTypeBuilder<Investidor> builder)
        {
            builder.ToTable("Investidores");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasMany(i => i.Investimentos)
                .WithOne(inv => inv.Investidor)
                .HasForeignKey(inv => inv.InvestidorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
