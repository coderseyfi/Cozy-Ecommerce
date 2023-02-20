using Cozy.Domain.Models.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cozy.Domain.Models.Entities.Configuration.Membership
{
    public class ProductRateEntityTypeConfiguration : IEntityTypeConfiguration<ProductRate>
    {
        public void Configure(EntityTypeBuilder<ProductRate> builder)
        {
            builder.HasKey(key =>
            new
            {
                key.ProductId,
                key.CreatedByUserId
            });

            builder.ToTable("ProductRates");
        }
    }
}