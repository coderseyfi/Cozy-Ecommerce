using Cozy.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cozy.Domain.Models.DataContexts.Configurations
{
    public class CozyUserTokenEntityTypeConfiguration : IEntityTypeConfiguration<CozyUserToken>
    {
        public void Configure(EntityTypeBuilder<CozyUserToken> builder)
        {
            builder.ToTable("UserTokens", "Membership");
        }
    }
}
