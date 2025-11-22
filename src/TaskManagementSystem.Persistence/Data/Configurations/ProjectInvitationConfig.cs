using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data.Configurations
{
    internal class ProjectInvitationConfig : IEntityTypeConfiguration<ProjectInvitation>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProjectInvitation> builder)
        {


            builder.Property(I => I.Status).HasConversion(S => S.ToString(),

                S => Enum.Parse<InvitationStatus>(S)

            );


            builder.HasOne(I => I.InvitedUser).WithMany(u => u.ProjectInvitations).HasForeignKey(I => I.InvitedUserId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(I => I.Project).WithMany(p => p.ProjectInvitations).HasForeignKey(I => I.ProjectId)
    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
