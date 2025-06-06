using Domain.Entities.ProjectModels;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data.Configurations
{
    public class ProjectTaskConfig : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProjectTask> builder)
        {


            builder.HasKey(PT => PT.Id);

            builder.HasOne(PT => PT.Project)
                   .WithMany(P => P.ProjectTasks)
                   .HasForeignKey(PT => PT.ProjectId);

            builder.HasOne(PT => PT.TaskItems)
                   .WithOne(TI => TI.ProjectTask)
                   .HasForeignKey<ProjectTask>(PT => PT.TaskItemId);
        }
    }
}
