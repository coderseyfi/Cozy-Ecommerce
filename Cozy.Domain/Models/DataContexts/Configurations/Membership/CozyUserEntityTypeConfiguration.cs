using Cozy.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cozy.Domain.Models.DataContexts.Configurations
{
    public class CozyUserEntityTypeConfiguration : IEntityTypeConfiguration<CozyUser>
    {
        public void Configure(EntityTypeBuilder<CozyUser> builder)
        {
            builder.ToTable("Users", "Membership");
        }
    }
}
