using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolicitaFacil.Domain.Entities;

namespace SolicitaFacile.Infrastructure.Persistence.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Street)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(s => s.Number)
            .HasMaxLength(10)
            .IsRequired();
        
        builder.Property(s => s.ZipCode)
            .HasMaxLength(10)
            .IsRequired();
        
        builder.Property(s => s.City)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(s => s.State)
            .HasMaxLength(100)
            .IsRequired();
    }
}
