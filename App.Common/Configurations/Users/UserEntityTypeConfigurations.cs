using App.Common.Configurations.Global;
using App.Domain.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Configurations.Users
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.OwnsMany(u => u.RefreshTokens, token =>
            {
                token.WithOwner().HasForeignKey("UserId");
                token.Property(t => t.Token).IsRequired();
                token.Property(t => t.CreatedOn).IsRequired();
                token.Property(t => t.ExpiresOn).IsRequired();
                token.Property(t => t.RefreshTokenExpireOn).IsRequired();
                token.ToTable("UserRefreshTokens");
            });

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.PhoneNumber).IsUnique();

            builder.Property(u => u.FirstName)
                   .HasMaxLength(ColumnEntityConfiguration.DefaultNameSize);

            builder.Property(u => u.LastName)
                   .HasMaxLength(ColumnEntityConfiguration.DefaultNameSize);

            builder.Property(u => u.ProfilePicture)
                   .HasMaxLength(ColumnEntityConfiguration.DefaultImageSize);

            builder.Property(u => u.IsActive)
                   .HasDefaultValue(ColumnEntityConfiguration.DefaultIsActive);

            builder.Property(u => u.IsDeleted)
                   .HasDefaultValue(ColumnEntityConfiguration.DefaultIsDeleted);

            builder.Property(u => u.IsVerified)
                   .HasDefaultValue(ColumnEntityConfiguration.DefaultIsVerified);

            builder.Property(u => u.CreatedOn)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
