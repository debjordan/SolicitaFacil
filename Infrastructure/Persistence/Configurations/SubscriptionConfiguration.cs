using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolicitaFacil.Domain.Entities;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Status)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(s => s.PayementDate)
            .IsRequired();
        
        builder.Property(s => s.ExpirationDate)
            .IsRequired();
    }
}