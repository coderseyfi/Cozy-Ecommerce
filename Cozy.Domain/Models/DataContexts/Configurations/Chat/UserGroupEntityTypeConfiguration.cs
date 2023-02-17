using Cozy.Domain.Models.Entites.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Cozy.Domain.Models.DataContexts.Configurations.Chat
{
    public class UserGroupEntityTypeConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.HasKey(k => new { k.UserId, k.GroupId });

            builder.ToTable("UserGroups", "Chat");
        }
    }
}
