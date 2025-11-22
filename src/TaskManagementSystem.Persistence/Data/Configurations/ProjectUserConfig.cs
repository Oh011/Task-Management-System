using Domain.Entities;
using Domain.Entities.ProjectModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    internal class ProjectUserConfig : IEntityTypeConfiguration<ProjectUser>
    {
        public void Configure(EntityTypeBuilder<ProjectUser> builder)
        {

            builder.Property(pu => pu.Role).HasConversion(Role => Role.ToString()
            , Role => Enum.Parse<ProjectRole>(Role ?? ""));

            builder.HasIndex(pu => new { pu.ProjectId, pu.UserId }).IsUnique();

        }
    }
}
