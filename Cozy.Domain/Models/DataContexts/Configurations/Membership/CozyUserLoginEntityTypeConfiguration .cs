using Cozy.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cozy.Domain.Models.DataContexts.Configurations
{
    public class CozyUserLoginEntityTypeConfiguration : IEntityTypeConfiguration<CozyUserLogin>
    {
        public void Configure(EntityTypeBuilder<CozyUserLogin> builder)
        {
            builder.ToTable("UserLogins", "Membership");
        }
    }
}
