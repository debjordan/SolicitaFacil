using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolicitaFacil.Domain.Entities;

namespace SolicitaFacile.Infrastructure.Persistence.Configurations;
public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(s => s.Description)
            .HasMaxLength(2000)
            .IsRequired();
        
        builder.Property(s => s.PriceRange)
            .HasMaxLength(50);

        builder.Property(s => s.Category)
            .HasMaxLength(50);
        
        builder.HasOne(s => s.User)
            .WithMany(s => s.Services)  
            .HasForeignKey(s => s.UserId);

        builder.HasOne(s => s.Address)
            .WithMany(s => s.Services)
            .HasForeignKey(s => s.AddressId)
            .OnDelete(DeleteBehavior.SetNull);
        
    }
}