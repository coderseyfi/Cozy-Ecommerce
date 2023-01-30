using Cozy.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cozy.Domain.Models.DataContexts.Configurations
{
    public class CozyRoleClaimEntityTypeConfiguration : IEntityTypeConfiguration<CozyRoleClaim>
    {
        public void Configure(EntityTypeBuilder<CozyRoleClaim> builder)
        {
            builder.ToTable("RoleClaims", "Membership");
        }
    }
}
