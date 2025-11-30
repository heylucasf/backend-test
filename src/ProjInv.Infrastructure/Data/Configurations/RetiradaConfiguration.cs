using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjInv.Domain.Entities;

namespace ProjInv.Infrastructure.Data.Configurations
{
    public class RetiradaConfiguration : IEntityTypeConfiguration<Retirada>
    {
        public void Configure(EntityTypeBuilder<Retirada> builder)
        {
            builder.ToTable("Retiradas");
            
            builder.HasKey(r => r.Id);
            
            builder.Property(r => r.InvestimentoId)
                .IsRequired();
            
            builder.Property(r => r.DataRetirada)
                .IsRequired();
            
            builder.Property(r => r.ValorBruto)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            
            builder.Property(r => r.Impostos)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            
            builder.Property(r => r.ValorLiquido)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        }
    }
}
