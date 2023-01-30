using Cozy.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cozy.Domain.Models.DataContexts.Configurations
{
    public class CozyUserClaimEntityTypeConfiguration : IEntityTypeConfiguration<CozyUserClaim>
    {
        public void Configure(EntityTypeBuilder<CozyUserClaim> builder)
        {
            builder.ToTable("UserClaims", "Membership");
        }
    }
}
