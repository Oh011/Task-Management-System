using Domain.Entities.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {


            builder.Property(R => R.CreatedAt).HasDefaultValueSql("GETUTCDATE()");


            builder.HasOne(R => R.User)  // A RefreshToken has one associated User.
         .WithMany()  // A User can have many RefreshTokens.
         .HasForeignKey(R => R.UserId)  // Foreign key property in RefreshToken.
         .OnDelete(DeleteBehavior.Cascade);  // Optionally, set the delete behavior.
        }
    }
}
