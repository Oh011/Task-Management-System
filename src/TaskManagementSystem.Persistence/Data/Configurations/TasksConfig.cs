using Domain.Entities.ProjectModels;
using Domain.Entities.TaskModels;
using Microsoft.EntityFrameworkCore;

using TasksStatus = Domain.Entities.TaskModels.TasksStatus;

namespace Persistence.Data.Configurations
{
    public class TasksConfig : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TaskItem> builder)
        {


            builder.Property(T => T.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(T => T.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");


            builder.Property(T => T.Priority).HasConversion(Priority => Priority.ToString(),

                p => Enum.Parse<TaskPriority>(p)
                );

            builder.Property(T => T.Status).HasConversion(status => status.ToString(),

               s => Enum.Parse<TasksStatus>(s)
               );


            builder.Property(T => T.TaskType).HasConversion(TaskType => TaskType.ToString(),

               p => Enum.Parse<TaskType>(p)
               );


            builder.HasOne(T => T.ProjectTask).WithOne(TP => TP.TaskItems).
                HasForeignKey<ProjectTask>(TP => TP.TaskItemId).OnDelete(DeleteBehavior.Cascade);



            builder.HasOne(T => T.CreatedByUser).WithMany(U => U.CreatedTasks)
                .HasForeignKey(U => U.CreatedByUserId).OnDelete(DeleteBehavior.NoAction);


            builder.HasOne(T => T.AssignedToUser).WithMany(U => U.AssignedTasks).HasForeignKey(T => T.AssignedToUserId).
                OnDelete(DeleteBehavior.NoAction);


        }
    }
}
