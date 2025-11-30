using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjInv.Domain.Entities;

namespace ProjInv.Infrastructure.Data.Configurations
{
    public class InvestimentoConfiguration : IEntityTypeConfiguration<Investimento>
    {
        public void Configure(EntityTypeBuilder<Investimento> builder)
        {
            builder.ToTable("Investimentos");
            
            builder.HasKey(i => i.Id);
            
            builder.Property(i => i.InvestidorId)
                .IsRequired();
            
            builder.Property(i => i.ValorInicial)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            
            builder.Property(i => i.DataCriacao)
                .IsRequired();

            builder.HasOne(i => i.Investidor)
                .WithMany(inv => inv.Investimentos)
                .HasForeignKey(i => i.InvestidorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Retirada)
                .WithOne(r => r.Investimento)
                .HasForeignKey<Retirada>(r => r.InvestimentoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
