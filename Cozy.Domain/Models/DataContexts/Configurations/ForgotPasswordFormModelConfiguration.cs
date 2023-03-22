using Cozy.Domain.Models.FormModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cozy.Domain.Models.DataContexts.Configurations
{
    public class ForgotPasswordFormModelConfiguration : IEntityTypeConfiguration<ForgotPasswordFormModel>
    {
        public void Configure(EntityTypeBuilder<ForgotPasswordFormModel> builder)
        {
            builder.HasKey(x => x.Email);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("Email")
                .HasColumnType("varchar(256)");
        }
    }
}
