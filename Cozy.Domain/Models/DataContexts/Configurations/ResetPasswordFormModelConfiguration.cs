using Cozy.Domain.Models.FormModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cozy.Domain.Models.DataContexts.Configurations
{

    public class ResetPasswordFormModelConfiguration : IEntityTypeConfiguration<ResetPasswordFormModel>
    {
        public void Configure(EntityTypeBuilder<ResetPasswordFormModel> builder)
        {
            builder.HasKey(x => x.Email);

            builder.Property(x => x.Email)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasAnnotation("RegularExpression", @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .HasAnnotation("RegularExpressionErrorMessage", "Invalid email address");

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasAnnotation("StringLength", 100)
                .HasAnnotation("StringLengthErrorMessage", "The password must be at least {MinimumLength} characters long.");

            builder.Property(x => x.ConfirmPassword)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasAnnotation("Compare", nameof(ResetPasswordFormModel.Password))
                .HasAnnotation("CompareErrorMessage", "The password and confirmation password do not match.");

            builder.Property(x => x.Token)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasAnnotation("StringLength", 100)
                .HasAnnotation("StringLengthErrorMessage", "The token must be at least {MinimumLength} characters long.");
        }
    }
}
