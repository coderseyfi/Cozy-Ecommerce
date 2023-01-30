using Cozy.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cozy.Domain.Models.DataContexts.Configurations
{
    public class CozyUserRoleEntityTypeConfiguration : IEntityTypeConfiguration<CozyUserRole>
    {
        public void Configure(EntityTypeBuilder<CozyUserRole> builder)
        {
            builder.ToTable("UserRole", "Membership");
        }
    }
}
