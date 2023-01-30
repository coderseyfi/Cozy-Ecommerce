using Cozy.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cozy.Domain.Models.DataContexts.Configurations
{
    public class CozyRoleEntityTypeConfiguration : IEntityTypeConfiguration<CozyRole>
    {
        public void Configure(EntityTypeBuilder<CozyRole> builder)
        {
            builder.ToTable("Roles", "Membership");
        }
    }
}
